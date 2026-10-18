using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通物体
/// </summary>
public class NormalItem
{
    public string name;    //名称
    public string imagePath;    //图片路径（相对）
    public string description;    //描述
    public float weight;    //重量
    public int maxStackAmount;    //最大堆叠数量
}

/// <summary>
/// 食物
/// </summary>
public class Food
{
    public string name;    //名称
    public string imagePath;    //图片路径（相对）
    public string description;    //描述
    public int durability;    //耐久
    public int maxDurability;    //最大耐久
    public float weight;    //重量
    public int maxStackAmount;    //最大堆叠数量

    /// <summary>
    /// 食用食物
    /// </summary>
    /// <param name="amount"></param>
    public void Have(int amount)
    {
        durability -= amount;
        Mathf.Clamp(durability, 0, maxDurability);
    }
}

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
