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
    private CharacterBase characterBaseScript_;

    // ゲームオブジェクトのMoneyManagerを登録する
    [SerializeField] private GameObject moneyManager_;

    // MoneyManagerにアタッチされたスクリプトを保持する
    private MoneyScript moneyScript_;

    // ボタン
    private Button spawnButton_;

    [Header("スポーンSE"), SerializeField] private AudioClip spawnSe_;

    // コスト用テキスト
    public TextMeshProUGUI costText;

    [Header("リスポーン用のゲージ"), SerializeField] private Slider respawnGaugeSlider_;

    // リスポーンタイマー
    private float reSpawnTimer_ = 0.0f;

    // リスポーン可能かどうかのフラグ
    private bool canReSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        characterBaseScript_ = playerObject_.GetComponent<CharacterBase>();

        moneyScript_ = moneyManager_.GetComponent<MoneyScript>();

        spawnButton_ = GetComponent<Button>();

        spawnButton_.onClick.AddListener(Spawn);

        reSpawnTimer_ = characterBaseScript_.GetSpawnInterval();
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    void Update()
    {

        // コストテキストの表示
        costText.text = "Cost : " + characterBaseScript_.GetNeedMoney().ToString();

        // コストよりも現在のお金があるのなら
        if (moneyScript_.currentMoney > characterBaseScript_.GetNeedMoney() && !canReSpawn)
        {
            GetComponent<Image>().color = new Color(255.0f, 244.0f, 0.0f, 255.0f);
            moneyScript_.canSpawn = true;
        }

        else
        {
            GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
            moneyScript_.canSpawn = false;
        }

        // リスポーン処理
        ReSpawn();
    }

    void Spawn()
    {

        //お金の最終チェック
        int cost = characterBaseScript_.GetNeedMoney();

        if (moneyScript_.currentMoney >= cost && !canReSpawn)
        {
            //お金を消費してスポーンさせる
            moneyScript_.Spawn(cost);

            //SpawnManagerに生成を依頼する
            if(SpawnManager.instance != null)
            {
                SpawnManager.instance.SpawnRequest(playerObject_);
                SoundManager.instance.PlaySe(spawnSe_);

                // リスポーン可能にする
                canReSpawn = true;
            }
        }
    }

    void ReSpawn()
    {
        // もしリスポーン可能なら
        if (canReSpawn)
        {
            // 経過時間を計算
            float elapsed = characterBaseScript_.GetSpawnInterval() - reSpawnTimer_;

            // リスポーンゲージを変化させる
            reSpawnTimer_ -= Time.deltaTime;

            // リスポーンゲージの現在値を設定
            respawnGaugeSlider_.value = elapsed;

            moneyScript_.canSpawn = false;
        }

        // リスポーンタイマーが0以下になったら
        if (reSpawnTimer_ <= 0.0f)
        {
            // リスポーンゲージをリセット
            reSpawnTimer_ = characterBaseScript_.GetSpawnInterval();
            respawnGaugeSlider_.value = reSpawnTimer_;

            // リスポーンのフラグを折る
            canReSpawn = false;

            moneyScript_.canSpawn = true;
        }
    }

}
