///
/// シーン遷移を管理するスクリプト
///
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeGroup_;        // で作ったCanvas Group
    [SerializeField] private float fadeDuration_ = 0.5f;    // フェードにかける時


    /// <summary>
    /// 即時にシーンをロードする
    /// </summary>
    public void LoadGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    /// <summary>
    /// 現在のシーンをリロードする
    /// </summary>
    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }


    /// <summary>
    /// フェードアウトしてシーンをロードする
    /// </summary>
    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeSequence(sceneName));
    }
    /// <summary>
    /// フェードアウトシーケンス
    /// </summary>
    private IEnumerator FadeSequence(string sceneName)
    {
        float elapsed = 0;
        fadeGroup_.blocksRaycasts = true;

        while (elapsed < fadeDuration_)
        {
            elapsed += Time.deltaTime;
            fadeGroup_.alpha = Mathf.Clamp01(elapsed / fadeDuration_);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}