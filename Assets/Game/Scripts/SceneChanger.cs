using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    // 画像を保持する変数
    private Image fadeImage_;
    private RectTransform slideImageRect_; // スライドは位置を動かすのでRectTransformを使う

    [Header("設定")]
    [SerializeField] private float duration_ = 0.5f; // アニメーション時間

    private void Awake()
    {
        // フェード画像の取得
        Transform fadeObj = transform.Find("FadeImage");
        if (fadeObj != null)
        {
            fadeImage_ = fadeObj.GetComponent<Image>();
            if (fadeImage_)
            {
                fadeImage_.raycastTarget = false;
                Color c = fadeImage_.color;
                c.a = 0f;
                fadeImage_.color = c;
            }
            else
            {
                Debug.LogError("'FadeImage' に Image コンポーネントが見つかりません");
            }
        }
        else
        {
            Debug.LogError("SceneChangerの子に 'FadeImage' が見つかりません");
        }


        // スライド画像の取得
        Transform slideObj = transform.Find("SlideImage");
        if (slideObj != null)
        {
            slideImageRect_ = slideObj.GetComponent<RectTransform>();

            // 初期位置を画面外右に設定
            if (slideImageRect_)
            {
                float screenWidth = GetComponent<RectTransform>().rect.width;
                slideImageRect_.anchoredPosition = new Vector2(screenWidth, 0);
            }
            else
            {
                Debug.LogError("'SlideImage' に RectTransform コンポーネントが見つかりません");
            }
        }
        else
        {
            Debug.LogError("SceneChangerの子に 'SlideImage' が見つかりません");
        }
    }


    /// <summary> 
    /// フェードしながら遷移
    /// </summary>
    public void FadeAndLoadScene(string sceneName)
    {
        if (fadeImage_ != null) StartCoroutine(FadeSequence(sceneName));
        else LoadGameScene(sceneName);
    }


    /// <summary> 
    /// スライドしながら遷移
    /// </summary>
    public void SlideAndLoadScene(string sceneName)
    {
        if (slideImageRect_ != null) StartCoroutine(SlideSequence(sceneName));
        else LoadGameScene(sceneName);
    }


    /// <summary> 
    /// 通常ロード 
    /// </summary>
    public void LoadGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    /// <summary>
    /// フェードシーケンス
    /// </summary>
    private IEnumerator FadeSequence(string sceneName)
    {
        fadeImage_.raycastTarget = true; // クリックブロック
        float elapsed = 0;

        while (elapsed < duration_)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration_);

            // アルファ値を操作
            Color c = fadeImage_.color;
            c.a = t;
            fadeImage_.color = c;

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }


    /// <summary>
    /// スライドシーケンス
    /// </summary>
    private IEnumerator SlideSequence(string sceneName)
    {
        Image img = slideImageRect_.GetComponent<Image>();
        // クリックブロック
        if (img)
        {
            img.raycastTarget = true;
        }

        // 経過時間
        float elapsed = 0;

        float screenWidth = GetComponent<RectTransform>().rect.width;
        Vector2 startPos = new Vector2(screenWidth, 0);
        Vector2 endPos = Vector2.zero;
        slideImageRect_.anchoredPosition = startPos;

        // スライドイン
        while (elapsed < duration_)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration_);
            slideImageRect_.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        slideImageRect_.anchoredPosition = endPos;

        SceneManager.LoadScene(sceneName);
    }
}