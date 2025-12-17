///
/// サウンドを管理するクラス
///
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // どこからでも呼び出せるようにする（シングルトン化）
    public static SoundManager instance;

    [Header("BGM用オーディオソース"), SerializeField]
    private AudioSource bgmAudioSource;

    [Header("SE用オーディオソース"), SerializeField]
    private AudioSource seAudioSource;


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


    /// <summary>
    /// BGMを再生する関数
    /// </summary>
    /// <param name="clip"> 再生するBGMのAudioClip </param>
    public void PlayBgm(AudioClip clip)
    {
        // すでに同じ曲が流れているなら何もしない（リスタート防ぐため）
        if (bgmAudioSource.clip == clip) return;

        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }


    /// <summary>
    /// BGMを停止する関数
    /// </summary>
    public void StopBgm()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = null;
    }


    /// <summary>
    /// SEを再生する関数
    /// </summary>
    /// <param name="clip"> 再生するSEのAudioClip </param>
    public void PlaySe(AudioClip clip)
    {
        seAudioSource.PlayOneShot(clip);
    }
}