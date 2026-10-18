using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 消息对象
/// </summary>
public class Message
{
    public object message;
}

/// <summary>
/// 封装的消息方法管理类
/// </summary>
public class MessageManager
{
    //需要监听消息对象的列表
    public static List<MessageBase> Managers = new List<MessageBase>();

    /// <summary>
    /// 注册成为监听器
    /// </summary>
    /// <param name="mBase"></param>
    public static void RegisterAsListener(MessageBase mBase)
    {
        Managers.Add(mBase);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="message"></param>
    public static void SendMessage(Message message)
    {
        foreach (MessageBase mb in Managers) 
        { 
            mb.Receivemessage(message);
        }
    }
}

/// <summary>
/// 消息监听父类
/// </summary>
public class MessageBase : MonoBehaviour
{
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="message"></param>
    public void SendMessage(Message message) 
    {
        MessageManager.SendMessage(message);
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="message"></param>
    public virtual void Receivemessage(Message message)
    {

    }
}
