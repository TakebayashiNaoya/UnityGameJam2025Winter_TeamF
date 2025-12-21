///
/// タイトルを管理するスクリプト
///
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [Header("タイトルBGM"), SerializeField] private AudioClip titleBgm_;          // 戦闘BGM

    void Start()
    {
        // タイトルBGM再生
        SoundManager.instance.PlayBgm(titleBgm_);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
