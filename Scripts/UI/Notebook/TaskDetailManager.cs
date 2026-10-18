using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 新增：任务数据结构类
[System.Serializable]
public class TaskData
{
    public string title;
    public string description;
    public int expReward;
    public int coinReward;
}

public class TaskDetailManager : MonoBehaviour
{
    public TMP_Dropdown taskDropDown;
    public TextMeshProUGUI currentOption;
    public TextMeshProUGUI taskTitle;
    public TextMeshProUGUI taskDescription;
    public TextMeshProUGUI taskReward;

    public List<string> taskTitles = new List<string>() { null };
    public List<string> taskDescriptions = new List<string>() { null };
    public Dictionary<string, float> taskRewards = new Dictionary<string, float>()
    {
        {"exp", 0},
        {"coin", 0},
    };

    // 存储所有任务数据的列表
    private List<TaskData> allTasks = new List<TaskData>();

    // 当前选中的任务索引
    private int currentTaskIndex = -1;

    public static TaskDetailManager instance;

    void Awake()
    {
        instance = this;
        taskDropDown.ClearOptions();
        // 监听下拉框变化事件
        taskDropDown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void Start()
    {
        // 如果初始有数据，设置默认选项
        if (taskTitles.Count > 0 && taskTitles[0] != null)
        {
            taskDropDown.SetValueWithoutNotify(0);
            currentTaskIndex = 0;
            SetTaskTexts();
        }
    }

    void Update()
    {
        // 保持原有的Update调用
        SetTaskTexts();
    }

    /// <summary>
    /// 下拉框选项改变时的回调
    /// </summary>
    private void OnDropdownValueChanged(int index)
    {
        currentTaskIndex = index;
        SetTaskTexts();
    }

    /// <summary>
    /// 设置各项文本（保持原有方法名）
    /// </summary>
    public void SetTaskTexts()
    {
        if (currentTaskIndex < 0 || currentTaskIndex >= allTasks.Count)
            return;

        TaskData currentTask = allTasks[currentTaskIndex];

        // 更新标题
        taskTitle.text = currentTask.title;

        // 更新描述
        taskDescription.text = currentTask.description;

        // 更新当前选项文本
        if (currentOption != null)
        {
            currentOption.text = currentTask.title;
        }

        // 更新奖励显示
        UpdateRewardDisplay(currentTask);
    }

    /// <summary>
    /// 更新奖励显示（新增方法）
    /// </summary>
    private void UpdateRewardDisplay(TaskData task)
    {
        // 同时更新原有的Dictionary（保持兼容性）
        taskRewards["exp"] = task.expReward;
        taskRewards["coin"] = task.coinReward;

        // 格式化奖励文本
        List<string> rewardStrings = new List<string>();
        if (task.expReward > 0)
            rewardStrings.Add($"Exp +{task.expReward}");
        if (task.coinReward > 0)
            rewardStrings.Add($"SY Coin +{task.coinReward}");

        taskReward.text = rewardStrings.Count > 0
            ? "奖励\n" + string.Join("\n", rewardStrings)
            : "奖励";
    }

    /// <summary>
    /// 新增：添加任务数据
    /// </summary>
    public void AddTaskData(TaskData taskData)
    {
        allTasks.Add(taskData);
    }

    /// <summary>
    /// 新增：清空所有任务数据
    /// </summary>
    public void ClearAllTasks()
    {
        allTasks.Clear();
        taskTitles.Clear();
        taskDescriptions.Clear();
        currentTaskIndex = -1;
    }

    /// <summary>
    /// 新增：刷新下拉框显示
    /// </summary>
    public void RefreshTaskDropdown()
    {
        taskDropDown.ClearOptions();

        // 更新原有的List变量（保持兼容性）
        taskTitles.Clear();
        taskDescriptions.Clear();

        foreach (TaskData task in allTasks)
        {
            taskDropDown.options.Add(new TMP_Dropdown.OptionData(task.title));
            taskTitles.Add(task.title);
            taskDescriptions.Add(task.description);
        }

        if (allTasks.Count > 0)
        {
            taskDropDown.SetValueWithoutNotify(0);
            currentTaskIndex = 0;
            SetTaskTexts();
        }
    }

    /// <summary>
    /// 新增：获取指定索引的任务数据
    /// </summary>
    public TaskData GetTaskData(int index)
    {
        if (index >= 0 && index < allTasks.Count)
            return allTasks[index];
        return null;
    }

    /// <summary>
    /// 新增：根据标题获取任务数据
    /// </summary>
    public TaskData GetTaskDataByTitle(string title)
    {
        return allTasks.Find(task => task.title == title);
    }
}