using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Print = Printf.Print;


public class ExpManager : MonoBehaviour
{
    public TMP_Text expValueText;
    public UnityEngine.UI.Image expBar;

    public int exp = 0;
    public int level = 0;
    public int expNeed = 1000;    //每级经验值上限

    public int skillPoint = 0;    //技能点

    private const float expNeedMultiplier = 0.2f;    //每级经验值上限乘数器
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateExpText();
    }

    public void GetExp(int NewExp)    //获取新的经验值
    {
        exp += NewExp;
        //Print.Log("Get Exp:", NewExp, "Current Exp:", exp, "Exp Need To Next Level:", expNeed);
        CalculateLevel();

    }

    private void CalculateLevel()
    {
        int tempLevel = level;   //计算所需
        int remainingExp = exp;    //计算所需

        while (remainingExp >= expNeed)
        {
            remainingExp -= expNeed;
            tempLevel++;
            expNeed = (int)(1000 * expNeedMultiplier * tempLevel + 1000);
        }

        if (tempLevel > level)
        {
            level = tempLevel;
            exp = remainingExp;    // 保存剩余经验

            skillPoint++;
            //Print.Log("New Level:", level, "Exp Need To Next Level:", expNeed, "Remaining Exp:", exp);
            //Print.Log("Skill Point:",skillPoint);
        }

        UpdateExpText();
    }

    public void UpdateExpText()
    {
        if (expValueText.IsActive())
        {
            expValueText.text = $"EXP  {exp}/{expNeed}";
            expBar.fillAmount = exp * 1f / expNeed;
        }
    }

}