using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理に必要

public class SceneTransition : MonoBehaviour
{
    // ボタンから呼び出すためのパブリック関数
    public void LoadGameScene(string sceneName)
    {
        // 指定した名前のシーンを読み込む
        SceneManager.LoadScene(sceneName);
    }

    // 現在のシーンをリロード（やり直し）する場合
    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}