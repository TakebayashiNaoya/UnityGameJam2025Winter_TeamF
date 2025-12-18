///
///キャラスポーンボタンクラス
///
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonScript : MonoBehaviour
{
    // ゲームオブジェクトのプレイヤーを登録する
    [SerializeField] private GameObject playerObject_;

    // PlayerObjectにアタッチされたスクリプトを保持する
    private CharacterBaseScript characterBaseScript_;

    // ゲームオブジェクトのMoneyManagerを登録する
    [SerializeField] private GameObject moneyManager_;

    // MoneyManagerにアタッチされたスクリプトを保持する
    private MoneyScript moneyScript_;

    // ボタン
    private Button spawnButton_;

    [Header("スポーンSE"), SerializeField] private AudioClip spawnSe_;

    // コスト用テキスト
    public TextMeshProUGUI costText;

    // Start is called before the first frame update
    void Start()
    {
        characterBaseScript_ = playerObject_.GetComponent<CharacterBaseScript>();

        moneyScript_ = moneyManager_.GetComponent<MoneyScript>();

        spawnButton_ = GetComponent<Button>();

        spawnButton_.onClick.AddListener(Spawn);
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    void Update()
    {

        // コストテキストの表示
        costText.text = "Cost : " + characterBaseScript_.GetNeedMoney().ToString();

        // コストよりも現在のお金があるのなら
        if (moneyScript_.currentMoney > characterBaseScript_.GetNeedMoney())
        {
            GetComponent<Image>().color = new Color(255.0f, 244.0f, 0.0f, 255.0f);
            moneyScript_.canSpawn = true;
        }

        else
        {
            GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
            moneyScript_.canSpawn = false;
        }
    }

    void Spawn()
    {

        //お金の最終チェック
        int cost = characterBaseScript_.GetNeedMoney();

        if (moneyScript_.currentMoney >= cost)
        {
            //お金を消費してスポーンさせる
            moneyScript_.Spawn(cost);

            //SpawnManagerに生成を依頼する
            if(SpawnManager.instance != null)
            {
                SpawnManager.instance.SpawnRequest(playerObject_);
            }
        }

        SoundManager.instance.PlaySe(spawnSe_);
    }
}
