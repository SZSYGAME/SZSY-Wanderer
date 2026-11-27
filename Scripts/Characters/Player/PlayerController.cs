using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using Image = UnityEngine.UI.Image;
using Input = UnityEngine.Input;
using Vector2 = UnityEngine.Vector2;

public enum State  //玩家状态
{
    idle,
    walk,
    run,
    sleep,
    eat,
    wc,  //这是上厕所吗？
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;    //单例
    public GameObject stateBg;    //状态栏背景
    public Image energyBar;
    public Image staminaBar;
    public TMP_Text energyValue;    //（UI）精力值文字
    public TMP_Text staminaValue;    //（UI）耐力文字
    public TMP_Text hungerValue;    //（UI）饱食度文字
    public TMP_Text thirstValue;    //（UI）水分文字

    private Vector2 MoveDirection;    //移动向量
    private Vector2 lookDirection = Vector2.right;
    private Rigidbody2D rb;
    private Animator ani;

    //Phisical Attributes生理属性
    //public float maxHP = 100f;
    //public float HP = 100f;
    public float speed = 6f;    //奔跑速度
    public int energy = 100;    //精力值，单位%
    public int maxEnergy = 100;    //最大精力值
    public float stamina = 100f;  //耐力，单位%
    public float maxStamina = 100f;    //最大耐力（耐力上限）
    public int thirst = 2000;    //水分，单位mL
    public int maxThirst = 2000;    //最大水分（口渴上限）
    public int hunger = 8400;    //饱食度，单位kJ
    public int maxHunger = 8400;    //最大饱食度（饱食度上限）

    //Game Attributes游戏属性
    public State state;     //玩家当前状态
    public bool movable;    //能否移动
    public bool cursorVisible;      //光标是否可见

    void Awake()
    {
        instance = this;

        //初始化属性
        state = State.idle;
        movable = true;
        cursorVisible = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        //启动协程
        StartCoroutine(ReduceThirstAndHunger());
        StartCoroutine(ReduceEnergy());
    }

    void Update()
    {
        SwitchState();
        SetCursorVisible();
        UpdateBars();
    }

    /// <summary>
    /// 更新各种条和文本
    /// </summary>
    public void UpdateBars()
    {
        if (stateBg.activeSelf)
        {
            energyBar.fillAmount = (float)energy / maxEnergy;
            staminaBar.fillAmount = stamina / maxStamina;
            energyValue.text = $"{energy}/{maxEnergy}";
            staminaValue.text = $"{stamina:F0}/{maxStamina:F0}";
            hungerValue.text = Mathf.RoundToInt((hunger * 1f / maxHunger) * 100f).ToString();
            thirstValue.text = Mathf.RoundToInt((thirst * 1f / maxThirst) * 100f).ToString();
        }
    }

    /// <summary>
    /// 显示状态栏
    /// </summary>
    public void ShowStateBg()
    {
        stateBg.SetActive(true);
    }

    /// <summary>
    /// 隐藏状态栏
    /// </summary>
    public void HideStateBg()
    {
        stateBg.SetActive(false);
    }

    /// <summary>
    /// 更改精力值
    /// </summary>
    /// <param name="value"></param>
    public void ChangeEnergy(int value)
    {
        energy += value;
        energy = Mathf.Clamp(energy, 0, maxEnergy);
    }

    /// <summary>
    /// 更改饱食度
    /// </summary>
    /// <param name="h"></param>
    public void ChangeHunger(int h)
    {
        hunger += h;
        hunger = Mathf.Clamp(hunger, 0, maxHunger);
    }

    /// <summary>
    /// 更改水分
    /// </summary>
    /// <param name="t"></param>
    public void ChangeThirst(int t)
    {
        thirst += t;
        thirst = Mathf.Clamp(thirst, 0,maxThirst);
    }

    /// <summary>
    /// 移动（刚体或Transform）
    /// </summary>
    void Move(float currentSpeed)
    {
        if (MoveDirection != Vector2.zero && movable)   //如果向量不为零且可移动
        {
            
            if (rb != null)
            {
                rb.velocity = MoveDirection * currentSpeed;  //先用2D刚体尝试移动
            }
            else
            {
                //如果没有2D刚体 
                transform.Translate(currentSpeed * Time.deltaTime * MoveDirection);  //就用transform
            }

            if (currentSpeed == speed)    //如果在奔跑
            {
                stamina -= 15 * Time.deltaTime;    //耐力条削减
            }
            else if (currentSpeed != speed && stamina < maxStamina)
            {
                stamina += 5 * Time.deltaTime;
            }

        }
        else    //如果向量为零或不可移动(静止状态)
        {
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }

            if (stamina < maxStamina)
            {
                stamina += 8 * Time.deltaTime;    //静止时回复耐力更快
            }
            
        }

        //播放动画
        PlayAnim(MoveDirection);
    }

    /// <summary>
    /// 根据角色当前状态执行行为逻辑
    /// </summary>
    void SwitchState()
    {
        //获取输入轴(判断依据)
        float horizontal = Input.GetAxis("Horizontal");    //横轴
        float vertical = Input.GetAxis("Vertical");      //纵轴 
        MoveDirection = new Vector2(horizontal, vertical);    //移动向量

        //判断是否奔跑
        bool isRunning = Input.GetKey(KeyCode.LeftControl) && stamina > 0;    //左Ctrl键
        float currentSpeed = isRunning ? speed : speed * 0.5f;    //根据是否奔跑设置当前速度

        //判断是否睡觉(按下Z键)
        bool isSleeping = Input.GetKey(KeyCode.Z);

        //判断是否上厕所(同时按下W和C键)
        bool isWc = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.C);

        //切换状态
        if(isSleeping)
        {
            state = State.sleep;
            MoveDirection = Vector2.zero;    //睡觉时不移动
        }
        else if (isWc)
        {
            state = State.wc;
            MoveDirection = Vector2.zero;    //上厕所时不移动
        }
        else if (isRunning)
        {
            state = State.run;
        }
        else if (!isRunning && MoveDirection != Vector2.zero)
        {
            state = State.walk;
        }
        else
        {
            state = State.idle;
        }

        switch (state)
        {
            case State.run:
                Move(currentSpeed);
                break;
            case State.walk:
                Move(currentSpeed);
                break;
            case State.idle:
                Move(currentSpeed);
                break;
            case State.sleep:
                break;
            case State.eat:
                Move(currentSpeed);
                break;
            case State.wc:
                break;
        }
    }

    void SetCursorVisible()
    {
        if (!cursorVisible)    //如果默认光标不可见
        {
            if (Input.GetKey(KeyCode.LeftAlt))    //如果按下左Alt键
            {
                Cursor.lockState = CursorLockMode.None;    //显示光标
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;    //锁定光标
            }

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    /// <summary>
    /// 根据移动输入更新动画方向；停止时保留最后的朝向
    /// </summary>
    /// <param name="moveDirection"></param>
    private void PlayAnim(Vector2 moveDirection)
    {
        bool isMoving = moveDirection != Vector2.zero && movable;
        if (!Mathf.Approximately(moveDirection.x, 0f) || !Mathf.Approximately(moveDirection.y, 0f))
        {
            lookDirection.Set(moveDirection.x, moveDirection.y);
            lookDirection.Normalize();
        }

        ani.SetFloat("horizontal", lookDirection.x);
        ani.SetFloat("vertical", lookDirection.y);
        ani.SetBool("isMoving", isMoving);
    }

    /// <summary>
    /// 定时消耗水分和饱食度
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReduceThirstAndHunger()
    {
        while (true)
        {
            //每等待20秒（真实时间，不受TimeScale影响）饥饿、口渴降低
            //2026-5-3：算了，还是用WaitForSeconds吧...
            yield return new WaitForSeconds(1f);    
            hunger -= 20;
            thirst -= 15;
            hunger = Mathf.Max(hunger, 0);
            thirst = Mathf.Max(thirst, 0);
            if (hunger <= 0)
            {
                Debug.LogWarning("饿死我了！！！");
            }
            if (thirst <= 0)
            {
                Debug.LogWarning("渴死我了！！！");
            }
        }
    }

    /// <summary>
    /// 定时消耗精力值
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReduceEnergy()
    {
        while (true) 
        {
            yield return new WaitForSeconds(1f);
            energy -= 1;
            energy = Mathf.Max(0,energy);
            if (energy <= 0)
            {
                Debug.LogWarning("I'm tierd!");
            }
        }

    }
}
