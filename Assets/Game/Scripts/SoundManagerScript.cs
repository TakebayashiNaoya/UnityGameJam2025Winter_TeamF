///
/// サウンドを管理するクラス
///
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // どこからでも呼び出せるようにする（シングルトン化）
    public static SoundManager instance;

    [Header("BGM用オーディオソース")]
    public AudioSource bgmAudioSource;

    [Header("SE用オーディオソース")]
    public AudioSource seAudioSource;

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

    // BGMを再生する関数
    public void PlayBgm(AudioClip clip)
    {
        // すでに同じ曲が流れているなら何もしない（リスタート防ぐため）
        if (bgmAudioSource.clip == clip) return;

        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    // BGMを止める関数
    public void StopBgm()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = null;
    }

    // SEを再生する関数（ついでに実装しておきます）
    public void PlaySe(AudioClip clip)
    {
        seAudioSource.PlayOneShot(clip);
    }
}