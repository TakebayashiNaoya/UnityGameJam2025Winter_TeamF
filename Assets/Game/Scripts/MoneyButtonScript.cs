//
//お金のボタンを管理するクラス
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyButtonScript : MonoBehaviour
{
    // ゲームオブジェクトのmoneyを登録する
    [SerializeField] GameObject MoneyManager;

    // moneyにアタッチされたスクリプトを保持する
    MoneyScript moneyScript;

    // ボタン
    Button moneyButton;

    // 現在のスプライト
    public Sprite CurrentSprite;

    // レベルアップ可能になった時のスプライト
    public Sprite CanLevelUpSprite;

    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        // Imageコンポーネントを取得
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyScript = money.GetComponent<MoneyScript>();

        moneyButton = GetComponent<Button>();

        moneyButton.onClick.AddListener(() =>
        {
            moneyScript.LevelUp();

            // レベルアップしたら画像を元に戻す
            _image.sprite = CurrentSprite;
        });

        // もしレベルアップ可能だったら
        if(moneyScript.CanLevelUP)
        {
            // レベルアップ可能用の画像にする
            _image.sprite = CanLevelUpSprite;
        }

        // それ以外は
        else
        {
            // そのまま
            _image.sprite = CurrentSprite;
        }
    }
}
