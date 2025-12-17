///
/// 戦闘を管理するクラス
///
using UnityEngine;

public class BattleManagerScript : MonoBehaviour
{
    [Header("戦闘BGM"), SerializeField] private AudioClip battleBgm;

    [Header("勝利SE"), SerializeField] private AudioClip winFanfare;

    [Header("敗北SE"), SerializeField] private AudioClip loseFanfare;

    void Start()
    {
        SoundManager.instance.PlayBgm(battleBgm);
    }

    void Update()
    {
        // 戦闘終了の判定
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameClear();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            GameOver();
        }
    }


    /// <summary>
    /// ゲームクリア時に呼ばれる関数
    /// </summary>
    private void GameClear()
    {
        // 流れている戦闘BGMを止める
        SoundManager.instance.StopBgm();

        // 勝利ファンファーレを「SE」として1回だけ鳴らす
        if (winFanfare != null)
        {
            SoundManager.instance.PlaySe(winFanfare);
        }
    }


    /// <summary>
    /// ゲームオーバー時に呼ばれる関数
    /// </summary>
    private void GameOver()
    {
        // BGMを止める
        SoundManager.instance.StopBgm();

        // 敗北ファンファーレを鳴らす
        if (loseFanfare != null)
        {
            SoundManager.instance.PlaySe(loseFanfare);
        }
    }
}