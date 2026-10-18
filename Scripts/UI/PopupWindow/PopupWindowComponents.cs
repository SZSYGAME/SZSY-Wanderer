using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupWindowComponents : MonoBehaviour
{
    [Header("背景面板")]
    public GameObject popupWindowBg;  // 透明背景，防止点击穿透

    [Header("窗口主体")]
    public GameObject popupWindow;    // 窗口背景

    [Header("文本组件")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;

    [Header("按钮组件")]
    public Button confirmButton;
    public Button cancelButton;
    public Button closeButton;

    [Header("输入框组")]
    public GameObject singleInputGroup;      // 单独输入框组
    public TMP_InputField singleInputField;  // 单独输入框

    public GameObject loginInputGroup;       // 登录输入框组
    public TMP_InputField accountInput;      // 账号输入框
    public TMP_InputField passwordInput;     // 密码输入框

    [Header("脚本组件")]
    public PopupWindowButtonsListener buttonsListener;
    public Back backScript;

    // 在编辑器中自动查找组件
    private void OnValidate()
    {
        AutoFindComponents();
    }

    private void AutoFindComponents()
    {
        if (popupWindowBg == null)
            popupWindowBg = gameObject.transform.Find("PopupWindowBg")?.gameObject;

        if (popupWindow == null)
            popupWindow = gameObject.transform.Find("PopupWindowBg/PopupWindow")?.gameObject;

        // 查找文本组件
        if (title == null)
            title = gameObject.transform.Find("PopupWindowBg/PopupWindow/Texts/Title")?.GetComponent<TextMeshProUGUI>();

        if (content == null)
            content = gameObject.transform.Find("PopupWindowBg/PopupWindow/Texts/Content")?.GetComponent<TextMeshProUGUI>();

        // 查找按钮组件
        if (confirmButton == null)
            confirmButton = gameObject.transform.Find("PopupWindowBg/PopupWindow/Buttons/ConfirmButton")?.GetComponent<Button>();

        if (cancelButton == null)
            cancelButton = gameObject.transform.Find("PopupWindowBg/PopupWindow/Buttons/CancelButton")?.GetComponent<Button>();

        if (closeButton == null)
            closeButton = gameObject.transform.Find("PopupWindowBg/PopupWindow/Buttons/CloseButton")?.GetComponent<Button>();

        // 查找输入框组件
        if (singleInputGroup == null)
            singleInputGroup = gameObject.transform.Find("PopupWindowBg/PopupWindow/InputFields/SingleInput")?.gameObject;

        if (singleInputField == null && singleInputGroup != null)
            singleInputField = singleInputGroup.GetComponentInChildren<TMP_InputField>();

        if (loginInputGroup == null)
            loginInputGroup = gameObject.transform.Find("PopupWindowBg/PopupWindow/InputFields/LoginInput")?.gameObject;

        if (accountInput == null && loginInputGroup != null)
            accountInput = loginInputGroup.transform.Find("AccountInput")?.GetComponent<TMP_InputField>();

        if (passwordInput == null && loginInputGroup != null)
            passwordInput = loginInputGroup.transform.Find("PwdInput")?.GetComponent<TMP_InputField>();

        // 查找脚本组件
        if (buttonsListener == null)
            buttonsListener = GetComponentInChildren<PopupWindowButtonsListener>();

        if (backScript == null)
            backScript = GetComponentInChildren<Back>();
    }
}