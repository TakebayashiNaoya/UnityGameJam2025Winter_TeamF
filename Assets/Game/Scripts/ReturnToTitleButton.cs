using UnityEngine;
using UnityEngine.UI;

public class ReturnToTitleButton : MonoBehaviour
{
    [SerializeField] private string titleSceneName_ = "Title"; // タイトルのシーン名

    void Start()
    {
        // ボタンコンポーネントを取得し、クリック時の処理を登録
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnClickReturn);
        }
    }

    void OnClickReturn()
    {
        // シーンにある SceneTransition (SceneChanger) を探す
        SceneTransition transition = FindObjectOfType<SceneTransition>();

        if (transition != null)
        {
            // 時間停止を解除してからフェード遷移
            Time.timeScale = 1.0f;
            transition.FadeAndLoadScene(titleSceneName_);
        }
        else
        {
            Debug.LogError("SceneTransition (SceneChanger) が見つかりません！");
            // 保険：見つからなくても強制移動
            Time.timeScale = 1.0f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(titleSceneName_);
        }
    }
}