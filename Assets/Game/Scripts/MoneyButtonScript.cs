//
//お金のボタンを管理するクラス
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyButtonScript : MonoBehaviour
{
    //ゲームオブジェクトのmoneyを登録する
    [SerializeField] GameObject money;

    //moneyにアタッチされたスクリプトを保持する
    MoneyScript moneyScript;

    //ボタン
    Button moneyButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moneyScript = money.GetComponent<MoneyScript>();

        moneyButton = GetComponent<Button>();

        moneyButton.onClick.AddListener(() =>
        {
            moneyScript.levelUP = true;
        });
    }
}
