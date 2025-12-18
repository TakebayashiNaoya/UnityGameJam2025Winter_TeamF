///
/// スポーンを管理するクラス
///
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // どこからでも呼び出せるようにする（シングルトン化）
    public static SpawnManager instance;

    [Header("プレイヤーユニットのスポナー"), SerializeField]
    private GameObject playerSpawner;

    [Header("エネミーユニットのスポナー"), SerializeField]
    private GameObject enemySpawner;

    /// <summary>
    /// 引数に渡されたキャラクターをスポーンさせます。
    /// </summary>
    public void SpawnRequest(GameObject chara)
    {
        // キャラクターのタグを取得
        string charaTag = chara.tag;
        // タグに応じてスポーンさせるスポナーを決定
        if (charaTag == "Player")
        {
            Instantiate(chara, playerSpawner.transform.position, Quaternion.identity);
        }
        else if (charaTag == "Enemy")
        {
            Instantiate(chara, enemySpawner.transform.position, Quaternion.identity);
        }
    }


    void Awake()
    {
        // これが存在しない場合、自分を代入
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // シーン遷移しても破壊されないようにする
        }
        else
        {
            // すでに存在する場合、重複しないように自分を破壊
            Destroy(this.gameObject);
        }
    }


    void Start()
    {

    }


    void Update()
    {

    }
}