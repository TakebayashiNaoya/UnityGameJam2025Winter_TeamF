///
/// お金管理
///
using UnityEngine;
using TMPro;

public class MoneyScript : MonoBehaviour
{
    // 現在のお金
    private int _currentMoney = 0;

    // お金レベル
    public int MoneyLevel { get; private set; }

    // お金レベルの上限
    private int _maxMoneyLevel = 7;

    [Header("増えるお金の量"), SerializeField]
    private int[] _addAmountOfChanges;

    [Header("お金の上限"), SerializeField]
    private int[] _maxMoneys;

    [Header("レベルアップに必要な金額"), SerializeField]
    private int[] _levelUpCosts;

    // 時間
    private float _timer = 0f;

    // レベルアップ可能かどうかのフラグ
    public bool CanLevelUP = false;

    public TextMeshProUGUI MoneyLevelText;

    public TextMeshProUGUI CurrentMoneyText;

    public TextMeshProUGUI LevelUpCostText;

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    void Update()
    {

        // 今持ってるお金が上限より多かったら
        if (_maxMoneys[MoneyLevel] <= _currentMoney)
        {
            // 上限で止める
            _currentMoney = _maxMoneys[MoneyLevel];
        }

        // 今のお金のレベルが上限なら
        if (_maxMoneyLevel <= MoneyLevel)
        {
            // 上限で止める
            MoneyLevel = _maxMoneyLevel;
        }

        // お金レベルが上限よりも下だった場合
        if (MoneyLevel < _maxMoneyLevel)
        {
            // お金のレベルアップに必要なお金以上にお金があった時かつ、レベルアップのフラグが立った時
            // if (_levelUpCosts[MoneyLevel] < _currentMoney && CanLevelUP)

            //レベルアップに必要な金額が貯まっている時
            if (_currentMoney > _levelUpCosts[MoneyLevel])
            {
                CanLevelUP = true;
            }

            else
            {
                CanLevelUP = false;
            }
        }

        // 現在のお金の文字表示
        CurrentMoneyText.text = _currentMoney.ToString() + " / " + _maxMoneys[MoneyLevel].ToString();

        if (MoneyLevel < _maxMoneyLevel)
        {
            // レベルアップに必要なお金の文字表示
            LevelUpCostText.text = "LevelUpCost : " + _levelUpCosts[MoneyLevel].ToString();

            // お金レベルの文字表示
            MoneyLevelText.text = "Level " + MoneyLevel;
        }

        // もしお金のレベルがMaxなら
        else
        {
            //コストの文字を無しと表示する
            LevelUpCostText.text = "LevelUpCost :";

            // レベルをMaxと表示する
            MoneyLevelText.text = "Level Max";
        }

        // ここから下でお金を増やしてる
        _timer += Time.deltaTime;

        if (_timer >= 1.0f)
        {
            _currentMoney += _addAmountOfChanges[MoneyLevel];
            _timer = 0.0f;
        }

        Debug.Log("現在のお金:" + _currentMoney);
    }

    public void LevelUp()
    {
        if (CanLevelUP)
        {
            _currentMoney -= _levelUpCosts[MoneyLevel];
            MoneyLevel++;
            CanLevelUP = false;
        }
    }

}
