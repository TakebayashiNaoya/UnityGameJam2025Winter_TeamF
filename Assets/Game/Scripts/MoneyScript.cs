///
///�����Ǘ�����N���X
///
using UnityEngine;
using TMPro;

public class MoneyScript : MonoBehaviour
{
    //���݂̂���
    private int _currentMoney = 0;

    //����̃��x��
    public int MoneyLevel = 0;

    //����̃��x�����
    private int _maximumMoneyLevel = 7;

    //�����邨��̕ω���
    [Header("�����邨��̗�"),SerializeField] private int[] _addAmountOfChange;

    //����̏��
    [Header("����̏��"),SerializeField] private int[] _maximumMoney;

    //���x���A�b�v�\�ɕK�v�Ȃ���
    [Header("���x���A�b�v�\�ɕK�v�Ȃ���"),SerializeField] private int[] _levelUpCost;

    //����
    private float _timer = 0;

    //���x���A�b�v�\���ǂ����̃t���O
    public bool LevelUP = false;

    //����̃��x���̃e�L�X�g
    public TextMeshProUGUI MoneyLevelText;

    public TextMeshProUGUI CurrentMoneyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���������݂̂�����傫���Ȃ�ꍇ
        if (_maximumMoney[MoneyLevel] <= _currentMoney)
        {
            //����𒴂��Ȃ��悤�ɂ���
            _currentMoney = _maximumMoney[MoneyLevel];
        }

        //���������݂̂�����x�����傫���Ȃ�ꍇ
        if (_maximumMoneyLevel <= MoneyLevel)
        {
            //����𒴂��Ȃ��悤�ɂ���
            MoneyLevel = _maximumMoneyLevel;
        }

        //���x���A�b�v�ɕK�v�Ȃ��������݂̂�������������x���A�b�v�̃t���O�������Ă�����
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

        //���݂̂���̕����\��
        CurrentMoneyText.text = _currentMoney.ToString();

        //����̃��x���̕����\��
        MoneyLevelText.text = "Level " + MoneyLevel;

        //�������牺�ł���𑝂₵�Ă�
        _timer += Time.deltaTime;

        if(_timer >= 1.0f)
        {
            _currentMoney += _addAmountOfChange[MoneyLevel];
            _timer = 0.0f;
        }

        Debug.Log("���݂̂���:" + _currentMoney);
    }

}
