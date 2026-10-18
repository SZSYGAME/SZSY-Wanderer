using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Printf : MonoBehaviour
{

    public class Print    //一个类似于Python的print的替代品
    {

        /// <summary>
        /// 将多参数合并，通过*UnityEngine.Debug.Log()*输出到Unity Console上
        /// </summary>
        /// <param name="obj"></param>
        public static void Log(params object[] obj)
        {
            object debugObj = null;
            for (int i = 0; i < obj.Length; i++)   
            {
                debugObj += obj[i] + " ";    
            }
            UnityEngine.Debug.Log(debugObj);    
        }

        /// <summary>
        /// 将多参数合并、添加每个参数的数据种类，通过*UnityEngine.Debug.Log()*输出到Unity Console上
        /// </summary>
        /// <param name="obj"></param>
        public static void DebugLog(params object[] obj)
        {
            object debugObj = null;
            for (int i = 0; i < obj.Length; i++)
            {
                debugObj += obj[i].GetType().Name + ":" + obj[i] + " ";

            }

            UnityEngine.Debug.Log(debugObj);
        }
        
    }
}
