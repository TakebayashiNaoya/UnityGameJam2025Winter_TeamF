///
///キャラスポーンボタンクラス
///
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonScript : MonoBehaviour
{
    [Header("マネーマネージャー"), SerializeField]
    private GameObject moneyManager_;

    [Header("スポーンマネージャー"), SerializeField]
    private GameObject spawnManager_;

    [Header("出撃させるユニットのプレハブ"), SerializeField]
    private GameObject unit_;

    [Header("リスポーン用のゲージ"), SerializeField]
    private Slider respawnGauge_;

    [Header("スポーンSE"), SerializeField]
    private AudioClip spawnSe_;


    // MoneyManagerにアタッチされたスクリプトを保持する
    private MoneyScript moneyScript_;

    // SpawnManagerにアタッチされたスクリプトを保持する
    private SpawnManager spawnManagerScript_;

    // ユニットにアタッチされたスクリプトを保持する
    private CharacterBase characterBaseScript_;

    // ボタン
    private Button spawnButton_;

    // コスト用テキスト
    public TextMeshProUGUI costText;


    // リスポーン可能かどうかのフラグ
    private bool canReSpawn_ = false;

    // 経過時間を設定
    private float elapsedTime = 0.0f;


    void Start()
    {
        moneyScript_ = moneyManager_.GetComponent<MoneyScript>();

        spawnManagerScript_ = spawnManager_.GetComponent<SpawnManager>();

        characterBaseScript_ = unit_.GetComponent<CharacterBase>();

        spawnButton_ = GetComponent<Button>();

        spawnButton_.onClick.AddListener(Spawn);

        // ゲージの最大値を設定
        respawnGauge_.maxValue = characterBaseScript_.GetSpawnInterval();

        // 最初はゲージを非表示にする
        respawnGauge_.gameObject.SetActive(false);
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    void Update()
    {
        if (costText != null)
        {
            // コストテキストの表示
            costText.text = "Cost : " + characterBaseScript_.GetNeedMoney().ToString();
        }


        // 出撃コスト以上のお金があって、かつリスポーン可能でなければ
        if (moneyScript_.currentMoney > characterBaseScript_.GetNeedMoney() && !canReSpawn_)
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

        if (moneyScript_.currentMoney >= cost && !canReSpawn_)
        {
            //お金を消費してスポーンさせる
            moneyScript_.Spawn(cost);

            if (spawnManagerScript_ == null)
            {
                return;
            }

            //SpawnManagerに生成を依頼する
            spawnManagerScript_.SpawnRequest(unit_);
            SoundManager.instance.PlaySe(spawnSe_);

            // リスポーン可能にする
            canReSpawn_ = true;

            // ゲージを表示する
            respawnGauge_.gameObject.SetActive(true);

            //リスポーン関係の情報を一度初期化する
            elapsedTime = 0.0f;
            respawnGauge_.maxValue = characterBaseScript_.GetSpawnInterval();
            respawnGauge_.value = 0.0f;
            moneyScript_.canSpawn = false;
        }
    }

    void ReSpawn()
    {
        // もしリスポーン可能なら
        if (canReSpawn_)
        {
            // 経過時間を加算していく
            elapsedTime += Time.deltaTime;

            // ゲージを更新する
            respawnGauge_.value = elapsedTime;

            // 経過時間がリスポーン時間を超えたら
            if (elapsedTime >= characterBaseScript_.GetSpawnInterval())
            {
                // リスポーン関連の情報を初期化する
                respawnGauge_.maxValue = characterBaseScript_.GetSpawnInterval();

                // ゲージを満タンにする
                respawnGauge_.value = respawnGauge_.maxValue;

                // リスポーンのフラグを折る
                canReSpawn_ = false;

                moneyScript_.canSpawn = true;

                // ゲージを非表示にする
                respawnGauge_.gameObject.SetActive(false);
            }

        }
    }

}
