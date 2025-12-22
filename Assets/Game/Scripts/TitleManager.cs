///
/// タイトルを管理するスクリプト
///
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [Header("タイトルBGM"), SerializeField] private AudioClip titleBgm_;          // 戦闘BGM
    [Header("選択SE"), SerializeField] private AudioClip selectSe_;          // 戦闘BGM

    void Start()
    {
        // タイトルBGM再生
        SoundManager.instance.PlayBgm(titleBgm_);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnClickStart()
    {
        // SEを再生
        if (selectSe_ != null)
        {
            SoundManager.instance.PlaySe(selectSe_);
        }
    }
}
