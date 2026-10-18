using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SettingsWindows
{
    game,    //游戏性设置
    graphics,    //画质设置
    audio,    //音量设置
    about    //关于界面
}

public class SettingsManager : MonoBehaviour
{
    [Header("设置界面按钮绑定")]
    public static SettingsManager instance;
    public Camera mainCamera;    //主摄像机
    public GameObject settingsBg;    //设置背景
    public GameObject gameSuite;    //游戏性设置组
    public GameObject graphicsSuite;    //画质设置组
    public GameObject aboutSuite;    //关于组
    public GameObject audioSuite;    //音量设置组

    public int fpsLimit  { get; private set; } = 60;
    public SettingsWindows currentWindow = SettingsWindows.game;    //当前选项卡窗口

    private readonly List<int> fpsGear = new List<int>() {45, 60, 90, 120};    //帧率挡位
    private List<int> resWidth = new List<int>() { };    //分辨率宽度
    private List<int> resHeight = new List<int>() { };    //分辨率高度
    public int mainAudio = 100;    //主音量

    public TMP_Dropdown fpsOption;    //帧率设置列表
    public TMP_Dropdown resolutionOption;    //分辨率设置列表
    private AudioListener audioListener;

    void Awake()
    {
        instance = this;
        currentWindow = SettingsWindows.game;
        settingsBg.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioListener = mainCamera.GetComponent<AudioListener>();
        FPSCalculator.instance.SetFPSLimit(fpsLimit);
        fpsOption.SetValueWithoutNotify(fpsGear.IndexOf(fpsLimit));
        foreach (Resolution res in Screen.resolutions)
        {
            int width = res.width;
            int height = res.height;
            RefreshRate refreshRate = res.refreshRateRatio;
            resWidth.Add(width);
            resHeight.Add(height);
            resolutionOption.options.Add(new TMP_Dropdown.OptionData($"{width}×{height}  {(int)refreshRate.value}Hz"));
        }
        resolutionOption.options.Reverse();
        resWidth.Reverse();
        resHeight.Reverse();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSurface();
    }

    /// <summary>
    /// 在设置中调节帧率
    /// </summary>
    /// <param name="fps"></param>
    public void SetFpsLimit()
    {
        int index = fpsOption.value;
        int limit = fpsGear[index];
        FPSCalculator.instance.SetFPSLimit(limit);
        fpsLimit = limit;
    }

    /// <summary>
    /// 设置游戏分辨率
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="fullScreen"></param>
    public void SetResolution()
    {
        int index = resolutionOption.value;
        Screen.SetResolution(resWidth[index], resHeight[index], true);
        Debug.Log($"已设置分辨率：{resWidth[index]}×{resHeight[index]}");
        
    }

    /// <summary>
    /// 获取屏幕可用的分辨率预设(int)
    /// </summary>
    /// <returns></returns>
    public Resolution[] GetValidResolution()
    {
        return Screen.resolutions;
    }

    /// <summary>
    /// 设置设置窗口可见性
    /// </summary>
    public void HandleSettingsBgActive(bool value)
    {
        settingsBg.SetActive(value);
        //if (value)
        //{
        //    fpsOption = graphicsSuite.GetComponentInChildren<TMP_Dropdown>();    
        //    resolutionOption = graphicsSuite.GetComponentInChildren<TMP_Dropdown>();
        //}
    }

    /// <summary>
    /// 更改设置界面的选项卡窗口
    /// </summary>
    /// <param name="window"></param>
    public void SetWindow(string value)
    {
        if (value == "game")
        {
            currentWindow = SettingsWindows.game;
        }else if (value == "graphics")
        {
            currentWindow = SettingsWindows.graphics;
        }else if (value == "about")
        {
            currentWindow = SettingsWindows.about;
        }
        else if (value  == "audio")
        {
            currentWindow = SettingsWindows.audio;
        }
        else
        {
            Debug.LogWarning($"command {value} is not defined");
        }
        
    }

    /// <summary>
    /// 更新选项卡
    /// </summary>
    public void UpdateSurface()
    {
        if (settingsBg.activeSelf)
        {
            if (currentWindow == SettingsWindows.game)
            {
                gameSuite.SetActive(true);
                graphicsSuite.SetActive(false);
                aboutSuite.SetActive(false);
                audioSuite.SetActive(false);
            }
            else if (currentWindow == SettingsWindows.graphics)
            {
                gameSuite.SetActive(false);
                graphicsSuite.SetActive(true);
                aboutSuite.SetActive(false);
                audioSuite.SetActive(false);
            }
            else if (currentWindow == SettingsWindows.about)
            {
                gameSuite.SetActive(false);
                graphicsSuite.SetActive(false);
                aboutSuite.SetActive(true);
                audioSuite.SetActive(false);
            }
            else if (currentWindow == SettingsWindows.audio)
            {
                gameSuite.SetActive(false);
                graphicsSuite.SetActive(false);
                aboutSuite.SetActive(false);
                audioSuite.SetActive(true);
            }
        }
        
    }

    /// <summary>
    /// 设置音量
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetMainAudio(int value)
    {
        mainAudio = Mathf.Clamp(value, 0, 100);
    }

}
