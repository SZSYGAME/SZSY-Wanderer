using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupWindowManager : MonoBehaviour
{
    public static PopupWindowManager instance;

    [Header("绑定物体")]
    public GameObject popupWindowPanelPre;
    public GameObject player;

    private GameObject globalCanvas;

    // 保存当前活动窗口实例的引用
    private GameObject currentWindowInstance;
    private PopupWindowComponents currentComponents;

    public bool windowActive = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        globalCanvas = GameObject.Find("GlobalCanvas");

        if (globalCanvas == null)
        {
            Debug.LogError("未找到GlobalCanvas！");
        }
    }

    void Update()
    {
        HandleCursorVisible();
    }

    void HandleCursorVisible()
    {
        if (currentWindowInstance != null && currentWindowInstance.activeSelf)
        {
            PlayerController.instance.cursorVisible = true;
        }
        else
        {
            
            windowActive = false;
        }
    }

    /// <summary>
    /// 新提示窗口
    /// </summary>
    public PopupWindowComponents NewNoticeWindow(string aTitle, string aContent)
    {
        CreateWindowInstance();

        if (currentComponents != null)
        {
            currentComponents.title.text = aTitle;
            currentComponents.content.text = aContent;
            currentComponents.singleInputGroup.SetActive(false);
            currentComponents.loginInputGroup.SetActive(false);
        }

        return currentComponents;
    }

    /// <summary>
    /// 新空窗口
    /// </summary>
    public PopupWindowComponents NewEmptyWindow()
    {
        CreateWindowInstance();

        if (currentComponents != null)
        {
            currentComponents.singleInputGroup.SetActive(false);
            currentComponents.loginInputGroup.SetActive(false);
        }

        return currentComponents;
    }

    /// <summary>
    /// 新单独输入框窗口
    /// </summary>
    public PopupWindowComponents NewInputWindow(string aTitle, string aContent)
    {
        CreateWindowInstance();

        if (currentComponents != null)
        {
            currentComponents.title.text = aTitle;
            currentComponents.content.text = aContent;
            currentComponents.singleInputGroup.SetActive(true);
            currentComponents.loginInputGroup.SetActive(false);

            // 清空输入框
            if (currentComponents.singleInputField != null)
            {
                currentComponents.singleInputField.text = "";
            }
        }

        return currentComponents;
    }

    /// <summary>
    /// 新登录窗口
    /// </summary>
    public PopupWindowComponents NewLoginWindow(string aTitle, string aContent)
    {
        CreateWindowInstance();

        if (currentComponents != null)
        {
            currentComponents.title.text = aTitle;
            currentComponents.content.text = aContent;
            currentComponents.singleInputGroup.SetActive(false);
            currentComponents.loginInputGroup.SetActive(true);

            // 清空输入框
            if (currentComponents.accountInput != null)
            {
                currentComponents.accountInput.text = "";
            }
            if (currentComponents.passwordInput != null)
            {
                currentComponents.passwordInput.text = "";
            }
        }

        return currentComponents;
    }

    /// <summary>
    /// 创建窗口实例
    /// </summary>
    private void CreateWindowInstance()
    {
        // 1. 校验预制体是否为空（关键！）
        if (popupWindowPanelPre == null)
        {
            Debug.LogError("PopupWindowManager: 登录窗口预制体popupWindowPanelPre未赋值！");
            return; // 预制体为空，直接终止创建
        }

        // 如果已有窗口，先销毁
        if (currentWindowInstance != null)
        {
            Destroy(currentWindowInstance);
        }

        // 实例化为GlobalCanvas的子物体
        currentWindowInstance = Instantiate(popupWindowPanelPre, globalCanvas.transform);

        // 2. 严谨获取组件（只从预制体读取，不动态添加）
        currentComponents = currentWindowInstance.GetComponent<PopupWindowComponents>();
        if (currentComponents == null)
        {
            Debug.LogError("PopupWindowManager: 窗口预制体未挂载PopupWindowComponents组件！");
            Destroy(currentWindowInstance); // 组件缺失，销毁无效窗口
            currentWindowInstance = null;
            windowActive = false; // 重置状态
            return;
        }

        // 3. 校验登录窗口关键UI组件（针对NewLoginWindow的专属校验）
        if (currentComponents.accountInput == null || currentComponents.passwordInput == null)
        {
            Debug.LogError("PopupWindowManager: PopupWindowComponents中账号/密码输入框未绑定！");
            Destroy(currentWindowInstance);
            currentWindowInstance = null;
            currentComponents = null;
            windowActive = false;
            return;
        }

        // 设置窗口为最上层
        currentWindowInstance.transform.SetAsLastSibling();

        currentWindowInstance.SetActive(true);
        windowActive = true;
    }

    /// <summary>
    /// 关闭当前窗口
    /// </summary>
    public void CloseCurrentWindow()
    {
        if (currentWindowInstance != null)
        {
            Destroy(currentWindowInstance);
            currentWindowInstance = null;
            currentComponents = null;
            windowActive = false;
            PlayerController.instance.cursorVisible = false;
        }
    }

    /// <summary>
    /// 获取当前窗口组件
    /// </summary>
    public PopupWindowComponents GetCurrentWindowComponents()
    {
        return currentComponents;
    }

    /// <summary>
    /// 获取单独输入框的内容
    /// </summary>
    public string GetSingleInputText()
    {
        if (currentComponents != null && currentComponents.singleInputField != null)
        {
            return currentComponents.singleInputField.text;
        }
        return "";
    }

    /// <summary>
    /// 获取登录信息
    /// </summary>
    public (string account, string password) GetLoginInfo()
    {
        string account = "";
        string password = "";

        if (currentComponents != null)
        {
            if (currentComponents.accountInput != null)
                account = currentComponents.accountInput.text;
            if (currentComponents.passwordInput != null)
                password = currentComponents.passwordInput.text;
        }

        return (account, password);
    }
}