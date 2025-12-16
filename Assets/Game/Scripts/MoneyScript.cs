///
///お金を管理するクラス
///
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MoneyScript : MonoBehaviour
{
    //現在のお金
    int currentMoney = 0;

    //お金のレベル
    int moneyLevel = 0;

    //お金のレベル上限
    int maximumMoneyLevel = 7;

    //増えるお金の変化量
    [SerializeField] private int[] addAmountOfChange;

    //お金の上限
    [SerializeField] private int[] maximumMoney;

    //時間
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //上限よりも現在のお金が大きくなる場合
        if (maximumMoney[moneyLevel] <= currentMoney)
        {
            //上限を超えないようにする
            currentMoney = maximumMoney[moneyLevel];
        }

        //上限よりも現在のお金レベルが大きくなる場合
        if (maximumMoneyLevel <= moneyLevel)
        {
            //上限を超えないようにする
            moneyLevel = maximumMoneyLevel;
        }

        timer += Time.deltaTime;

        if(timer >= 1.0f)
        {
            currentMoney += addAmountOfChange[moneyLevel];
            timer = 0.0f;
        }

        Debug.Log("現在のお金:" + currentMoney);
    }

    void AddMoney()
    {
       
    }

    void MaximumMoneyManager()
    {
        
    }
}
