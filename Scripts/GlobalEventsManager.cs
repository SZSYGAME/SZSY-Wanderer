using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
    {
        menu,    //标题界面
        dormitory,    //宿舍
        toilet,    //厕所
        test    //仅测试用
    }

public class GlobalEventsManager : MonoBehaviour
{
    public static GlobalEventsManager instance;

    public KeyCode pauseKey = KeyCode.Escape;    //暂停键

    public GameObject pauseBg;    //暂停界面
    public TMP_Text detail;    //版本详情（在暂停界面）
    public GameObject loadingBg;    //加载界面
    public TMP_Text loadingTitle;    //加载页面标题

    public Scenes currentScene { get; private set; } = Scenes.menu;    //设置当前场景
    public bool isPause = false;    //是否暂停
    private bool isFading = false;    //是否处于淡入淡出状态
    public bool cursorVisible = false;    //光标是否可见

    private CanvasGroup canvasGroup;
    void Awake()
    {
        instance = this;
        if (detail != null)
        {
            detail.text = $"SYGM Wanderer ver 0.1, Unity {Application.unityVersion}";
        }
        else
        {
            return;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (pauseBg != null)
        {
            pauseBg.SetActive(false);
        }
        
        canvasGroup = loadingBg.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        loadingBg.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
        SetCursorVisible();
    }

    void SetCursorVisible()
    {
        if (!cursorVisible && currentScene != Scenes.menu)    //如果默认光标不可见
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
    /// 设置暂停界面的可见性，并调整游戏速率
    /// </summary>
    public void Pause() 
    {
        if (Input.GetKeyDown(pauseKey) && pauseBg != null)
        {
            //Time.timeScale = 0f;    //时间速率设为零（先不要）
            isPause = true;
            pauseBg.SetActive(isPause);
            cursorVisible = true;
        }
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 返回游戏
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1.0f;
        isPause = false;
        pauseBg.SetActive(isPause);
        cursorVisible = false;
        FPSCalculator.instance.SetFPSLimit(SettingsManager.instance.fpsLimit);
    }

    /// <summary>
    /// 返回到标题界面
    /// </summary>
    public void ReturnMenu()
    {
        Debug.Log("返回菜单");
        StartCoroutine(HandleLoadScene("TitleScene","Dormitory",Scenes.menu));
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void PlayGame()
    {
        Debug.Log("进入游戏");
        StartCoroutine(HandleLoadScene( "Dormitory","TitleScene", Scenes.dormitory));
    }

    /// <summary>
    /// 固定时间的加载
    /// </summary>
    /// <param name="time"></param>
    /// <param name="title"></param>
    public void Loading(float time, string title = "加载中......")
    {
        if (!isPause && !loadingBg.activeSelf)
        {
            //Debug.Log("加载，启动！");
            loadingTitle.text = title;
            loadingBg.SetActive(true);
            StartCoroutine(WaitLoading(time));
        }
    }

    /// <summary>
    /// 显示加载画面
    /// </summary>
    /// <param name="title"></param>
    public void ShowLoading(string title = "加载中......") 
    {
        if (!isPause && !loadingBg.activeSelf)
        {
            loadingTitle.text = title;
            loadingBg.SetActive(true);
            StartCoroutine(HandleFade(0.5f));
            
        }
    }

    /// <summary>
    /// 隐藏加载画面
    /// </summary>
    /// <param name="title"></param>
    public void HideLoading()
    {
        StartCoroutine(HandleFade(0.5f,false));
    }

    /// <summary>
    /// 等待加载
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator WaitLoading(float time, float fadeTime = 0.5f)
    {
        if (!isFading)
        {
            isFading = true;
            yield return StartCoroutine(HandleFade(fadeTime));
            yield return new WaitForSecondsRealtime(time);
            yield return StartCoroutine(HandleFade(fadeTime, false));
            loadingBg.SetActive(false);
        }
        else
        {
            yield return null;
        }
        
    }

    /// <summary>
    /// 处理淡入淡出
    /// </summary>
    /// <param name="time"></param>
    /// <param name="appear"></param>
    /// <returns></returns>
    private IEnumerator HandleFade(float time, bool appear = true)
    {
        isFading = true;

        if (appear)    //如果是要显示出来
        { 
            yield return StartCoroutine(RenderFade(0f,1f,time));
        }
        else
        {
            yield return StartCoroutine(RenderFade(1f,0f,time));
        }
    }

    /// <summary>
    /// 渲染淡入淡出
    /// </summary>
    /// <param name="startAlpha"></param>
    /// <param name="endAlpha"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator RenderFade(float startAlpha = 0f, float endAlpha = 1.0f, float duration = 0.5f)
    {
        float time = 0f;
        while (time < duration) 
        {
            float value = time / duration;
            time += Time.deltaTime;    //随时间变化
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, value);    //线性插值，按比例变化
            yield return null;    //等待一帧
        }

        canvasGroup.alpha = endAlpha;
        isFading = false;
    }

    /// <summary>
    /// 处理加载场景（string版）
    /// </summary>
    /// <param name="loadName"></param>
    /// <param name="unloadName"></param>
    /// <param name="sceneType"></param>
    /// <returns></returns>
    private IEnumerator HandleLoadScene(string loadName = "", string unloadName = "", Scenes sceneType = Scenes.menu)
    {
        ShowLoading();
        currentScene = sceneType;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadName);
        asyncLoad.allowSceneActivation = false;    //禁止自动激活

        while (asyncLoad.progress < 1f)    //当进度<1 (100%)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;

        yield return asyncLoad;

        if (unloadName != "")
        {
            AsyncOperation unload = SceneManager.UnloadSceneAsync(unloadName);
            yield return unload;
        }

        Debug.Log("加载完毕");

        HideLoading();
    }

    /// <summary>
    /// 处理加载场景（int版）
    /// </summary>
    /// <param name="loadIndex"></param>
    /// <param name="unloadIndex"></param>
    /// <param name="sceneType"></param>
    /// <returns></returns>
    private IEnumerator HandleLoadScene(int loadIndex = 0, int unloadIndex = 0, Scenes sceneType = Scenes.menu)
    {
        ShowLoading();
        currentScene = sceneType;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadIndex);
        asyncLoad.allowSceneActivation = false;    //禁止自动激活

        while (asyncLoad.progress < 1f)    //当进度<1 (100%)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;

        yield return asyncLoad;

        if (unloadIndex != 0)
        {
            AsyncOperation unload = SceneManager.UnloadSceneAsync(unloadIndex);
            yield return unload;
        }

        HideLoading();
    }

}
