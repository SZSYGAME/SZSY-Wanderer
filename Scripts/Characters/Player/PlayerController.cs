using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

using Image = UnityEngine.UI.Image;
using Vector2 = UnityEngine.Vector2;
using Input = UnityEngine.Input;

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

    private Vector2 vector2;
    private Rigidbody2D rb;
    private Animator ani;

    //Phisical Attributes生理属性
    //public float maxHP = 100f;
    //public float HP = 100f;
    public float speed = 6f;
    public int energy = 100;    //精力值，单位%
    public int maxEnergy = 100;    //最大精力值
    public float stamina = 100f;  //耐力，单位%
    public float maxStamina = 100f;    //最大耐力（耐力上限）
    public int thirst = 2000;    //水分，单位mL
    public int maxThirst = 2000;
    public int hunger = 8400;    //饱食度，单位kJ
    public int maxHunger = 8400;

    //Game Attributes游戏属性
    public State state = State.idle;
    public bool movable = true;    //能否移动
    public bool cursorVisible = false;

    void Awake()
    {
        instance = this;
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
        //Flip();已经用动画控制器的参数来控制翻转了，这个方法没用了
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
    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");    //横轴
        float vertical = Input.GetAxis("Vertical");      //纵轴

        PlayAnim(horizontal, vertical);

        vector2 = new Vector2(horizontal, vertical);

        bool isRunning = Input.GetKey(KeyCode.LeftControl) && stamina > 0;    //左Ctrl键
        float currentSpeed = isRunning ? speed : speed * 0.5f;    //当前是否处于奔跑


        if (vector2 != Vector2.zero && movable)   //如果向量不为零且可移动
        {
            
            if (rb != null)
            {
                rb.velocity = new Vector2(horizontal * currentSpeed, vertical * currentSpeed);  //先用2D刚体尝试移动
            }
            else
            {
                //如果没有2D刚体 
                transform.Translate(currentSpeed * Time.deltaTime * vector2);  //就用transform
            }

            if (currentSpeed == speed)    //如果在奔跑
            {
                stamina -= 24 * Time.deltaTime;    //耐力条削减
            }
            else if (currentSpeed != speed && stamina < maxStamina)
            {
                stamina += 14 * Time.deltaTime;
            }

        }
        else
        {
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }

            if (stamina < maxStamina)
            {
                stamina += 18 * Time.deltaTime;    //静止时回复耐力更快
            }
            
        }

        if (currentSpeed == speed)
        {
            state = State.run;
        }
        else if (currentSpeed == speed * 0.5)
        {
            state = State.walk;
        }
        else
        {
            state = State.idle;
        }

    }

    /// <summary>
    /// 判断角色状态
    /// </summary>
    void SwitchState()
    {
        switch (state)
        {
            case State.run:
                Move();
                break;
            case State.walk:
                Move();
                break;
            case State.idle:
                Move();
                break;
            case State.sleep:
                break;
            case State.eat:
                Move();
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
    /// 翻转，（没用的东西，我要把他删了）
    /// </summary>
    /*
    private void Flip()
    {
        float horiziontal = Input.GetAxis("Horizontal"); //横轴
        float vertical = Input.GetAxis("Vertical"); //纵轴
        if (horiziontal > 0) 
        {
            transform.localScale = new UnityEngine.Vector3(-15, 15, 10);
        }
        else if (horiziontal < 0)
        {
            transform.localScale = new UnityEngine.Vector3(15, 15, 10);
        }

    }
    */
    

    /// <summary>
    /// 根据输入虚拟轴设置动画播放
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    private void PlayAnim(float horizontal, float vertical)
    {
        ani.SetFloat("horizontal", horizontal);
        ani.SetFloat("vertical",vertical);
        ani.SetBool("isMoving", horizontal != 0 || vertical != 0);
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
