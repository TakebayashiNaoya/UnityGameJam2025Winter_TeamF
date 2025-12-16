///
///お金を管理するクラス
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyScript : MonoBehaviour
{
    //現在のお金
    int currentMoney = 0;

    //お金のレベル
    int moneyLevel = 1;

    //お金の上限
    int maximumMoney = 6000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AddMoney();
        MaximumMoneyManager();
    }

    void AddMoney()
    {
        switch(moneyLevel)
        {
            case 1:
                //お金を増やす
                currentMoney += 185;
                break;

            default:
                break;
        }
    }

    void MaximumMoneyManager()
    {
        //上限よりも現在のお金が大きくなる場合
        if (maximumMoney <= currentMoney)
        {
            //上限以上に行かないようにする
            currentMoney = maximumMoney;
        }
    }
}
