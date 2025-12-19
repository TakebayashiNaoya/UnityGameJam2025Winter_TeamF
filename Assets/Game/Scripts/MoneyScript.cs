
///
/// お金管理
///
using UnityEngine;
using TMPro;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

public class MoneyScript : MonoBehaviour
{
    // 現在のお金
    public int currentMoney { get; private set; }

    // お金レベル
    public int moneyLevel { get; private set; }

    // お金レベルの上限
    private int maxMoneyLevel_ = 7;

    // お金の変化量
    private static readonly int[] addAmountOfChanges_ = { 185, 193, 203, 212, 222, 231, 241, 250 };
    public static IReadOnlyList<int> AddAmountOfChanges => addAmountOfChanges_;

    // お金の上限
    private static readonly int[] maxMoneys_ = { 6000, 7500, 9000, 10500, 12000, 13500, 15000, 16500 };
    public static IReadOnlyList<int> MaxMoneys => maxMoneys_;

    // レベルアップに必要な金額
    private static readonly int[] levelUpCosts_ = { 560, 1120, 1680, 2240, 2800, 3360, 3920 };
    public static IReadOnlyList<int> LevelUpCosts => levelUpCosts_;

    // 時間
    private float timer_ = 0f;

    // レベルアップ可能かどうかのフラグ
    public bool canLevelUP = false;

    // キャラがスポーン可能かどうかのフラグ
    public bool canSpawn = false;

    public TextMeshProUGUI moneyLevelText;

    public TextMeshProUGUI currentMoneyText;

    public TextMeshProUGUI levelUpCostText;

    [Header("レベルアップSE"), SerializeField] private AudioClip levelUpSe_;

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    void Update()
    {

        // 今持ってるお金が上限より多かったら
        if (maxMoneys_[moneyLevel] <= currentMoney)
        {
            // 上限で止める
            currentMoney = maxMoneys_[moneyLevel];
        }

        // 今のお金のレベルが上限なら
        if (maxMoneyLevel_ <= moneyLevel)
        {
            // 上限で止める
            moneyLevel = maxMoneyLevel_;
        }

        // お金レベルが上限よりも下だった場合
        if (moneyLevel < maxMoneyLevel_)
        {
            // お金のレベルアップに必要なお金以上にお金があった時かつ、レベルアップのフラグが立った時
            // if (_levelUpCosts[MoneyLevel] < _currentMoney && CanLevelUP)

            // レベルアップに必要な金額が貯まっている時
            if (currentMoney > levelUpCosts_[moneyLevel])
            {
                canLevelUP = true;
            }

            else
            {
                canLevelUP = false;
            }
        }

        // 現在のお金の文字表示
        currentMoneyText.text = currentMoney.ToString() + " / " + maxMoneys_[moneyLevel].ToString();

        if (moneyLevel < maxMoneyLevel_)
        {
            // レベルアップに必要なお金の文字表示
            levelUpCostText.text = "LevelUpCost : " + levelUpCosts_[moneyLevel].ToString();

            // お金レベルの文字表示
            moneyLevelText.text = "Level " + moneyLevel;
        }

        // もしお金のレベルがMaxなら
        else
        {
            // コストの文字を無にする
            levelUpCostText.text = "LevelUpCost :";

            // レベルをMaxと表示する
            moneyLevelText.text = "Level Max";
        }

        // ここから下でお金を増やしてる
        timer_ += Time.deltaTime;

        if (timer_ >= 1.0f)
        {
            currentMoney += addAmountOfChanges_[moneyLevel];
            timer_ = 0.0f;
        }

        Debug.Log("現在のお金:" + currentMoney);
    }

    public void LevelUp()
    {
        if (canLevelUP)
        {
            currentMoney -= levelUpCosts_[moneyLevel];
            moneyLevel++;
            canLevelUP = false;
            SoundManager.instance.PlaySe(levelUpSe_);
        }
        
    }


    public void Spawn(int cost)
    {
        if (canSpawn)
        {
            currentMoney -= cost;
            canSpawn = false;
        }
        
    }

}
