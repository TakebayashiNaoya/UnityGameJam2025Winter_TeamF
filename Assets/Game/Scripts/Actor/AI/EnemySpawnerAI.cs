using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerAI : MonoBehaviour
{
    [Header("スポーンインターバルの最小値"), SerializeField]
    private float minSpawnInterval_;

    [Header("スポーンインターバルの最大値"), SerializeField]
    private float maxSpawnInterval_;

    [Header("バトルマネージャー"), SerializeField]
    private GameObject battleManager_;

    [Header("スポーンマネージャー"), SerializeField]
    private GameObject spawnManager_;

    [Header("スポーンする敵の種類"), SerializeField]
    private List<GameObject> enemyPrefabs_ = new List<GameObject>();


    BattleManagerScript btManagerSc_;           // バトルマネージャースクリプト
    SpawnManager spMangerSc_;                   // スポーンマネージャースクリプト
    private float spawnIntervalTimer_ = 0.0f;   // スポーンインターバルタイマー
    private float nextSpawnInterval_ = 0.0f;    // 次のスポーンインターバル


    private void OnValidate()
    {
        CorrectSpawnInterval();

    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeObject();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObject();
    }


    private void InitializeObject()
    {
        spawnIntervalTimer_ = 0.0f;
        nextSpawnInterval_ = 0.0f;


        nextSpawnInterval_ = Random.Range(minSpawnInterval_, maxSpawnInterval_);

        CorrectEnemyPrefabsInfo();

        if (battleManager_ == null)
        {
            Debug.LogWarning("EnemySpawnAI: バトルマネージャーオブジェクトがセットされていません");
            return;
        }
        else
        {
            btManagerSc_ = battleManager_.GetComponent<BattleManagerScript>();
        }

        if (spawnManager_ == null)
        {
            Debug.LogWarning("EnemySpawnAI: スポーンマネージャーオブジェクトがセットされていません");
            return;
        }
        else
        {
            spMangerSc_ = spawnManager_.GetComponent<SpawnManager>();
        }
    }


    private void UpdateObject()
    {
        if (btManagerSc_.IsGameOver())
        {
            return;
        }

        //タイマーを進める
        spawnIntervalTimer_ += Time.deltaTime;

        //次のスポーンインターバルに達していなかったら抜ける
        if (spawnIntervalTimer_ <= nextSpawnInterval_)
        {
            return;
        }

        //タイマーをリセット
        spawnIntervalTimer_ = 0.0f;
        RandomSpawn();
    }


    /// <summary>
    /// ランダムに敵を選んでスポーンさせる
    /// </summary>
    private void RandomSpawn()
    {
        int nextEnemyIndex = Random.Range(0, enemyPrefabs_.Count);
        spMangerSc_.SpawnRequest(enemyPrefabs_[nextEnemyIndex]);
        nextSpawnInterval_ = Random.Range(minSpawnInterval_, maxSpawnInterval_);
    }


    /// <summary>
    /// スポーンインターバルの値を補正する
    /// </summary>
    private void CorrectSpawnInterval()
    {
        //インターバルの最小値が0未満なら0にする
        if (minSpawnInterval_ < 1.0f)
        {
            minSpawnInterval_ = 1.0f;
        }
        if (minSpawnInterval_ > 20.0f)
        {
            minSpawnInterval_ = 20.0f;
        }
        //インターバルの最大値が最小値未満なら最小値にする
        if (maxSpawnInterval_ < minSpawnInterval_)
        {
            maxSpawnInterval_ = minSpawnInterval_;
        }
        //インターバルの最大値が30秒を超えていたら30秒にする
        if (maxSpawnInterval_ > 30.0f)
        {
            maxSpawnInterval_ = 30.0f;
        }
    }


    /// <summary>
    /// 登録した敵のプレハブ情報を補正する
    /// </summary>
    private void CorrectEnemyPrefabsInfo()
    {
        //敵のプレハブが設定されていなかったら警告を出す
        if (enemyPrefabs_.Count == 0)
        {
            Debug.LogWarning("EnemySpawnerAI: 敵のプレハブが設定されていません。");
        }
        if (enemyPrefabs_.Count > 5)
        {
            Debug.LogWarning("EnemySpawnerAI: 敵のプレハブが多すぎます。5個以下にしてください。");
        }
    }
}
