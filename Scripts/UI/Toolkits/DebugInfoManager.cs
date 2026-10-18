using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

using Text = UnityEngine.UI.Text;


public class DebugInfoManager : MonoBehaviour
{
    public KeyCode debugKey = KeyCode.F3;    //调试按键F3

    public static DebugInfoManager instance;   //单例
    public GameObject debugText;
    public GameObject expManager;

    private TextMeshProUGUI dT;
    private ExpManager eM;
    

    public bool isDebugging = false;  //是否处于调试状态
    private string text = "";
    private float gameTime = 0;

    void Awake()  //单例
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        dT = debugText.GetComponent<TextMeshProUGUI>();
        eM = expManager.GetComponent<ExpManager>();

        debugText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        gameTime = Time.time;
        SetTextActive();
        UpdateText();
    }

    /// <summary>
    /// 切换调试文本显隐
    /// </summary>
    void SetTextActive()
    {
        if (Input.GetKeyDown(debugKey))
        {
            isDebugging = ! isDebugging;
            debugText.SetActive(isDebugging);
        }
    }

    /// <summary>
    /// 更新文本（仅在显示时更新）
    /// </summary>
    void UpdateText()
    {
        if (isDebugging)
        {
            int playerExp = eM.exp;
            int playerLv = eM.level;

            text = "Debug Information" +
                $"\tPlayerLevel:{playerLv}" +
                $"\nPlayerExp:{playerExp}" +
                $"\nFPS:{FPSCalculator.instance.GetFPS():F2}";

            dT.text = text;
        }
    }

}
