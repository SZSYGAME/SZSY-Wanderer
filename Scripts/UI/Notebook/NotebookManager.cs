using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum NotebookPages
{
    tasks,    //任务界面
    states,    //角色状态
    talents,    //天赋

    //之后再加吧

}

public class NotebookManager : MonoBehaviour
{
    private KeyCode openNotebook = KeyCode.J;    //J键打开笔记本
    public static NotebookManager instance;

    [Header("绑定物体")]
    public GameObject notebookCanvas;
    public GameObject player;
    public TMP_Text notebookTitle;    //笔记本（左侧）主标题
    public TMP_Text rightTitle;    //笔记本右侧标题

    private TextMeshProUGUI nT;    //对应notebookTitle
    private TextMeshProUGUI rT;    //对应rightTitle

    public bool canOpenNote = true;    //能否打开笔记本
    public bool noteActive = false;    //笔记本是否打开
    private NotebookPages page = NotebookPages.tasks;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nT = notebookTitle.GetComponent<TextMeshProUGUI>();
        rT = rightTitle.GetComponent<TextMeshProUGUI>();
        notebookCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandleNotebookActive();
        UpdateTexts();
        HandleCursorVisible();
    }

    /// <summary>
    /// 开启笔记本
    /// </summary>
    void HandleNotebookActive()
    {
        noteActive = notebookCanvas.activeSelf;
        if (canOpenNote)
        {
            if (Input.GetKeyDown(openNotebook))
            {
                noteActive = !noteActive;
                notebookCanvas.SetActive(noteActive);
            }
        }
        else if (!canOpenNote)
        {
            Printf.Print.Log("Cannot Open Notebook");
        }
    }

    /// <summary>
    /// 处理光标可见状态
    /// </summary>
    void HandleCursorVisible()
    {
        if (notebookCanvas.activeSelf)    //如果笔记本激活
        {
            PlayerController.instance.cursorVisible = notebookCanvas.activeSelf;    //设置默认光标状态
        }
        
    }

    /// <summary>
    /// 更新笔记本内文本
    /// </summary>
    void UpdateTexts()
    {
        if (page == NotebookPages.tasks)
        {
            nT.text = "任务";
        }
        else if (page == NotebookPages.states)
        {
            nT.text = "状态";
        }
        else if (page == NotebookPages.talents)
        {
            nT.text = "天赋";
        }
    }


    /// <summary>
    /// 更改页面
    /// </summary>
    /// <param name="pageName"></param>
    public void ChangePage(string pageName)
    {
        if (pageName.ToLower() == "task")
        {
            page = NotebookPages.tasks;
        }
        else if (pageName.ToLower() == "state")
        {
            page = NotebookPages.states;
        }
        else if(pageName.ToLower() == "talents")
        {
            page = NotebookPages.talents;
        }
        else
        {
            Debug.LogWarning($"未找到对应页：{pageName}");
        }
    }
}
