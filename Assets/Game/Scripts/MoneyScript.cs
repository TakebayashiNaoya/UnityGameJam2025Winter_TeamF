
///
///お金を管理するクラス
///
using UnityEngine;
using TMPro;

public class MoneyScript : MonoBehaviour
{
    //現在のお金
    private int _currentMoney = 0;

    //お金レベル
    public int MoneyLevel = 0;

    //お金レベルの上限
    private int _maximumMoneyLevel = 7;

    //増えるお金の量
    [Header("増えるお金の量"),SerializeField] private int[] _addAmountOfChange;

    //お金の上限
    [Header("お金の上限"),SerializeField] private int[] _maximumMoney;

    //お金がレベルアップ可能に必要なお金
    [Header("お金がレベルアップ可能に必要なお金"),SerializeField] private int[] _levelUpCost;

    //時間
    private float _timer = 0;

    //レベルアップ可能かどうかのフラグ
    public bool LevelUP = false;

    public TextMeshProUGUI MoneyLevelText;

    public TextMeshProUGUI CurrentMoneyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //今持ってるお金が上限より多かったら
        if (_maximumMoney[MoneyLevel] <= _currentMoney)
        {
            //上限で止める
            _currentMoney = _maximumMoney[MoneyLevel];
        }

        //今のお金のレベルが上限なら
        if (_maximumMoneyLevel <= MoneyLevel)
        {
            //上限で止める
            MoneyLevel = _maximumMoneyLevel;
        }

        //���x���A�b�v�ɕK�v�Ȃ��������݂̂�������������x���A�b�v�̃t���O�������Ă�����
        if (_levelUpCost[MoneyLevel] < _currentMoney && LevelUP == true)
        {
            //お金のレベルアップに必要なお金以上にお金があった時かつ、レベルアップのフラグが立った時
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

        //お金レベルの文字表示
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
