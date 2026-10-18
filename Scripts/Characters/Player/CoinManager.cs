using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    //public GameObject coinValue;
    //private TextMeshProUGUI coinValueText;

    public int coins = 0;
    // Start is called before the first frame update
    void Start()
    {
        //coinValueText = coinValue.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateCoinText();
    }

    void UpdateCoinText()
    {
        //if (coinValue.activeSelf)
        //{
        //    //coinValueText.text = $"SY Coin:{coins}";
        //}
    }

    /// <summary>
    /// 增加SY金币
    /// </summary>
    /// <param name="coin"></param>
    public void AddCoin(int coin)
    {
        coins += coin;
    }

    /// <summary>
    /// 减少SY金币
    /// </summary>
    /// <param name="coin"></param>
    public void ReduceCoin(int coin) 
    { 
        coins -= coin;
        coins = Mathf.Max(coins, 0);
    }
}
