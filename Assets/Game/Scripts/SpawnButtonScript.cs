///
///キャラスポーンボタンクラス
///
using TMPro;
using Unity.VisualScripting;
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

    // 経過時間を設定
    private float elapsedTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        characterBaseScript_ = playerObject_.GetComponent<CharacterBase>();

        moneyScript_ = moneyManager_.GetComponent<MoneyScript>();

        spawnButton_ = GetComponent<Button>();

        spawnButton_.onClick.AddListener(Spawn);

        reSpawnTimer_ = characterBaseScript_.GetSpawnInterval();

        respawnGaugeSlider_.maxValue = characterBaseScript_.GetSpawnInterval();

        // 最初はゲージを非表示にする
        respawnGaugeSlider_.gameObject.SetActive(false);

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

                // ゲージを表示する
                respawnGaugeSlider_.gameObject.SetActive(true);

                //リスポーン関係の情報を一度初期化する
                elapsedTime = 0.0f;
                respawnGaugeSlider_.maxValue = characterBaseScript_.GetSpawnInterval();
                respawnGaugeSlider_.value = 0.0f;
                moneyScript_.canSpawn = false;

            }
        }
    }

    void ReSpawn()
    {
        // もしリスポーン可能なら
        if (canReSpawn)
        {
            // 経過時間を加算していく
            elapsedTime += Time.deltaTime;

            // ゲージを更新する
            respawnGaugeSlider_.value = elapsedTime;

            // 経過時間がリスポーン時間を超えたら
            if (elapsedTime >= characterBaseScript_.GetSpawnInterval())
            {
                // リスポーン関連の情報を初期化する
                respawnGaugeSlider_.maxValue = characterBaseScript_.GetSpawnInterval();

                // ゲージを満タンにする
                respawnGaugeSlider_.value = respawnGaugeSlider_.maxValue;

                // リスポーンのフラグを折る
                canReSpawn = false;

                moneyScript_.canSpawn = true;

                // ゲージを非表示にする
                respawnGaugeSlider_.gameObject.SetActive(false);
            }

        }
    }

}
