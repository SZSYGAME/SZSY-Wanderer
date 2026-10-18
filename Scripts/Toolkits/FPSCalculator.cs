using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCalculator : MonoBehaviour
{
    public static FPSCalculator instance;

    private const float frameCalculateTime = 1.0f;  //帧率计算时间
    private float time = 0f;  //帧间隔时间
    private float frame = 0f;
    private float fps = 0f;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetFPSLimit(120);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateFPS();
    }

    /// <summary>
    /// 计算帧率
    /// </summary>
    private void CalculateFPS()
    {
        time += Time.deltaTime;
        frame++;
        if (time > frameCalculateTime)
        {
            fps = frame / time;  //确保帧率更接近每秒帧率
            time = 0f;
            frame = 0f;
        }
    }

    /// <summary>
    /// 获取帧率
    /// </summary>
    /// <returns></returns>
    public float GetFPS()
    {
        return fps;
    }

    /// <summary>
    /// 限制帧率
    /// </summary>
    /// <param name="fpsLimit"></param>
    public void SetFPSLimit(int fpsLimit)
    {
        if (fpsLimit > 0)
        {
            Application.targetFrameRate = fpsLimit;
        }
        else
        {
            Debug.LogError($"帧率不可设置为负数：{fpsLimit}");
        }
    }
}
