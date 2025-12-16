///
///お金を管理するクラス
///
using UnityEngine;
using TMPro;

public class MoneyScript : MonoBehaviour
{
    //現在のお金
    private int _currentMoney = 0;

    //お金のレベル
    public int MoneyLevel = 0;

    //お金のレベル上限
    private int _maximumMoneyLevel = 7;

    //増えるお金の変化量
    [Header("増えるお金の量"),SerializeField] private int[] _addAmountOfChange;

    //お金の上限
    [Header("お金の上限"),SerializeField] private int[] _maximumMoney;

    //レベルアップ可能に必要なお金
    [Header("レベルアップ可能に必要なお金"),SerializeField] private int[] _levelUpCost;

    //時間
    private float _timer = 0;

    //レベルアップ可能かどうかのフラグ
    public bool LevelUP = false;

    //お金のレベルのテキスト
    public TextMeshProUGUI MoneyLevelText;

    public TextMeshProUGUI CurrentMoneyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //上限よりも現在のお金が大きくなる場合
        if (_maximumMoney[MoneyLevel] <= _currentMoney)
        {
            //上限を超えないようにする
            _currentMoney = _maximumMoney[MoneyLevel];
        }

        //上限よりも現在のお金レベルが大きくなる場合
        if (_maximumMoneyLevel <= MoneyLevel)
        {
            //上限を超えないようにする
            MoneyLevel = _maximumMoneyLevel;
        }

        if (MoneyLevel < 7)
        {
            //レベルアップに必要なお金よりも現在のお金が多いかつレベルアップのフラグが立っていたら
            if (_levelUpCost[MoneyLevel] < _currentMoney && LevelUP == true)
            {
                _currentMoney -= _levelUpCost[MoneyLevel];
                MoneyLevel++;
                LevelUP = false;
            }

            else
            {
                LevelUP = false;
            }
        }
        //現在のお金の文字表示
        CurrentMoneyText.text = _currentMoney.ToString();

        //お金のレベルの文字表示
        MoneyLevelText.text = "Level " + MoneyLevel;

        //ここから下でお金を増やしてる
        _timer += Time.deltaTime;

        if(_timer >= 1.0f)
        {
            _currentMoney += _addAmountOfChange[MoneyLevel];
            _timer = 0.0f;
        }

        Debug.Log("現在のお金:" + _currentMoney);
    }

}
