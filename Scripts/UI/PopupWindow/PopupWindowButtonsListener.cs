using UnityEngine;
using UnityEngine.UI;
using System;

public class PopupWindowButtonsListener : MonoBehaviour
{
    public static PopupWindowButtonsListener instance;

    [Header("按钮")]
    public Button confirmButton;
    public Button cancelButton;
    public Button closeButton;
    
    //判断是否按下对应按键的对外广播
    public Action<bool> onConfirm;
    public Action<bool> onCancel;
    public Action<bool> onClose;

    // 按钮状态标志
    private bool confirmWasClicked = false;
    private bool cancelWasClicked = false;
    private bool closeWasClicked = false;

    // 当前窗口实例引用
    private GameObject currentWindow;
    private PopupWindowComponents windowComponents;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 获取组件
        windowComponents = GetComponentInParent<PopupWindowComponents>();
        if (windowComponents != null)
        {
            currentWindow = windowComponents.gameObject;
        }

        // 自动查找未分配的按钮
        if (windowComponents != null)
        {
            if (confirmButton == null) confirmButton = windowComponents.confirmButton;
            if (cancelButton == null) cancelButton = windowComponents.cancelButton;
            if (closeButton == null) closeButton = windowComponents.closeButton;
        }

        // 绑定按钮事件
        BindButtonEvents();
    }

    private void BindButtonEvents()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(HandleConfirmClick);
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(HandleCancelClick);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HandleCloseClick);
        }
    }

    void OnDestroy()
    {
        // 清理事件监听
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveListener(HandleConfirmClick);
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveListener(HandleCancelClick);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(HandleCloseClick);
        }
    }

    private void HandleConfirmClick()
    {
        confirmWasClicked = true;
        Debug.Log("确认按钮被点击");

        onConfirm?.Invoke(true);  //对外广播对应按键被按下（下同）

        // 关闭窗口
        CloseWindow();
    }

    private void HandleCancelClick()
    {
        cancelWasClicked = true;
        Debug.Log("取消按钮被点击");

        onCancel?.Invoke(true);

        // 关闭窗口
        CloseWindow();
    }

    private void HandleCloseClick()
    {
        closeWasClicked = true;
        Debug.Log("关闭按钮被点击");

        onClose?.Invoke(true);

        // 关闭窗口
        CloseWindow();
    }

    private void CloseWindow()
    {
        if (currentWindow != null)
        {
            // 尝试使用Back脚本销毁
            Back back = currentWindow.GetComponentInChildren<Back>();
            if (back != null)
            {
                back.BackAndDestroy(currentWindow);
            }
            else
            {
                // 如果没有Back脚本，直接销毁
                Destroy(currentWindow,0.05f);
            }
        }
    }

    // 状态检查方法，好像没啥用
    public bool WasConfirmClicked()
    {
        bool result = confirmWasClicked;
        confirmWasClicked = false;
        return result;
    }
    
    public bool WasCancelClicked()
    {
        bool result = cancelWasClicked;
        cancelWasClicked = false;
        return result;
    }
    
    public bool WasCloseClicked()
    {
        bool result = closeWasClicked;
        closeWasClicked = false;
        return result;
    }

    // 获取当前状态（不重置）
    public bool IsConfirmClicked() => confirmWasClicked;
    public bool IsCancelClicked() => cancelWasClicked;
    public bool IsCloseClicked() => closeWasClicked;

    // 重置所有状态
    public void ResetAllStates()
    {
        confirmWasClicked = false;
        cancelWasClicked = false;
        closeWasClicked = false;
    }
}