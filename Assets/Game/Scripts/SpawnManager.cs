///
/// スポーンを管理するクラス
///
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
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
}