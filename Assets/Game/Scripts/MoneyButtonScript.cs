//
//お金のボタンを管理するクラス
//
using UnityEngine;
using UnityEngine.UI;

public class MoneyButtonScript : MonoBehaviour
{
    // ゲームオブジェクトのMoneyManagerを登録する
    [SerializeField] private GameObject moneyManager_;

    // MoneyManagerにアタッチされたスクリプトを保持する
    private MoneyScript moneyScript_;

    // ボタン
    Button moneyButton;

    // 現在のスプライト
    public Sprite currentSprite;

    // レベルアップ可能になった時のスプライト
    public Sprite canLevelUpSprite;

    private Image image_;

    [Header("レベルアップSE"), SerializeField] private AudioClip LevelUpSE;

    // Start is called before the first frame update
    void Start()
    {
        // Imageコンポーネントを取得
        image_ = GetComponent<Image>();

        moneyScript_ = moneyManager_.GetComponent<MoneyScript>();

        moneyButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

       

        moneyButton.onClick.AddListener(() =>
        {
            moneyScript_.LevelUp();

            // レベルアップしたら画像を元に戻す
            image_.sprite = currentSprite;
        });

        // もしレベルアップ可能だったら
        if(moneyScript_.canLevelUP)
        {
            // レベルアップ可能用の画像にする
            image_.sprite = canLevelUpSprite;
        }

        // それ以外は
        else
        {
            // そのまま
            image_.sprite = currentSprite;
        }
    }
}
