using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeConversion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Printf.Print.Log($"1:{SecondsToMinutesInStr(300)},  2:{SecondsToMinutesInStr(300.00f)}");
    }

    /// <summary>
    /// 将秒数转换为时：分：秒格式，并以字符串形式返回
    /// </summary>
    /// <param name="seconds">任意格式的秒数(强制转换为float)</param>
    /// <param name="showHours">是否显示小时</param>
    /// <param name="PadZero">是否补零</param>
    /// <returns></returns>
    public static string FormatTime(float seconds,bool showHours = false,bool PadZero = true)
    {
        int hours = (int)(seconds / 3600);
        int minutes = (int)((seconds % 3600) / 60);
        float secondsRemaining = seconds % 60;
        if (showHours)
        {
            if (PadZero)
            {
                return $"{hours:D2}:{minutes:D2}:{secondsRemaining:D2}";
            }
            else
            {
                return $"{hours}:{minutes}:{secondsRemaining}";
            }
        }
        else
        {
            if (PadZero)
            {
                return $"{minutes:D2}:{secondsRemaining:D2}";
            }
            else
            {
                return $"{minutes}:{secondsRemaining}";
            }
        }
    }

}
