using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

// 保持原有的类定义不变
[Serializable]
public class Tasks
{
    public Task[] tasks;
}

[Serializable]
public class Task
{
    public string title;
    public string description;
    public Reward reward;
}

[Serializable]
public class Reward
{
    public int exp;
    public int coin;
}

// 新增：任务数据转换的扩展方法类
public static class TaskDataExtensions
{
    /// <summary>
    /// 将JSON解析的Task转换为TaskData
    /// </summary>
    public static TaskData ToTaskData(this Task task)
    {
        return new TaskData
        {
            title = task.title,
            description = task.description,
            expReward = task.reward?.exp ?? 0,
            coinReward = task.reward?.coin ?? 0
        };
    }
}

public class TaskDataReader : MonoBehaviour
{

    void Awake()
    {
        StartCoroutine(ReadJSON());
    }

    private IEnumerator ReadJSON()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "tasks.json");

        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonString = request.downloadHandler.text;
                Debug.Log("读取到的JSON内容: " + jsonString);

                Tasks tasksData = JsonUtility.FromJson<Tasks>(jsonString);

                if (tasksData != null && tasksData.tasks.Length > 0)
                {
                    // 清空现有数据
                    TaskDetailManager.instance.ClearAllTasks();

                    // 转换并添加任务数据
                    for (int i = 0; i < tasksData.tasks.Length; i++)
                    {
                        TaskData taskData = tasksData.tasks[i].ToTaskData();
                        TaskDetailManager.instance.AddTaskData(taskData);

                        //Debug.Log($"加载任务: {taskData.title}, 经验: {taskData.expReward}, 金币: {taskData.coinReward}");
                    }

                    // 刷新下拉框显示
                    TaskDetailManager.instance.RefreshTaskDropdown();
                }
            }
            else
            {
                Debug.LogError("读取文件失败: " + request.error);
            }
        }
    }
}