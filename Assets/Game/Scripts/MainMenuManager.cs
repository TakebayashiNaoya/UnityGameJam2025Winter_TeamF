using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("音の設定")]
    [SerializeField] private AudioClip menuBgm_;
    [SerializeField] private AudioClip doorOpenSe_;  // ★ここにドアのSEを入れる
    [SerializeField] private AudioClip returnSe_;


    void Start()
    {
        // BGM再生
        if (menuBgm_ != null)
        {
            SoundManager.instance.PlayBgm(menuBgm_);
        }
    }

    // ★ボタンにはこの関数【だけ】を登録します★
    public void OnClickDoor()
    {
        // 1. コードでシングルトンのSoundManagerを呼ぶ（これでMissing回避！）
        if (doorOpenSe_ != null)
        {
            SoundManager.instance.PlaySe(doorOpenSe_);
        }
    }


    public void OnClickReturn()
    {
        // SEを再生
        if (returnSe_ != null)
        {
            SoundManager.instance.PlaySe(returnSe_);
        }
    }
}