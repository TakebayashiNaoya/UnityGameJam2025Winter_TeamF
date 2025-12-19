//
//お金のボタンを管理するクラス
//
using System.Collections;
using System.Collections.Generic;
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

        moneyButton = GetComponent<Button>(); // ここで取得

        moneyScript_ = moneyManager_.GetComponent<MoneyScript>();

        // クリック処理の登録は Start で一度だけ行う
        moneyButton.onClick.AddListener(() =>
        {
            moneyScript_.LevelUp();
            image_.sprite = currentSprite;
        });
    }

    // Update is called once per frame
    void Update()
    {
        moneyScript_ = moneyManager_.GetComponent<MoneyScript>();

        //moneyButton = GetComponent<Button>();

        //moneyButton.onClick.AddListener(() =>
        //{
        //    moneyScript_.LevelUp();

        //    // レベルアップしたら画像を元に戻す
        //    image_.sprite = currentSprite;
        //});

        // もしレベルアップ可能だったら
        if (moneyScript_.canLevelUP)
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
