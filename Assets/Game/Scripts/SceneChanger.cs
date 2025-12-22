using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    // 画像を保持する変数
    private Image fadeImage_;
    private RectTransform slideImageRect_; // スライドは位置を動かすのでRectTransformを使う

    [Header("演出時間の設定")]
    [SerializeField] private float fadeDuration_ = 0.5f;  // フェードの時間（ゆっくり）
    [SerializeField] private float slideDuration_ = 0.2f; // スライドの時間（シュッと速く）

    // シーンをまたいで「スライド状態」を共有するフラグ
    private static bool isSliding_ = false;


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


    private void Start()
    {
        // シーン開始時に「スライドの続き」かどうかをチェック
        if (isSliding_)
        {
            // スライド中なら「退場（Out）」アニメーションを開始
            StartCoroutine(SlideOutSequence());
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
        if (slideImageRect_ != null) StartCoroutine(SlideInSequence(sceneName));
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

        while (elapsed < fadeDuration_)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration_);

            // アルファ値を操作
            Color c = fadeImage_.color;
            c.a = t;
            fadeImage_.color = c;

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }


    /// <summary>
    /// スライドイン
    /// </summary>
    private IEnumerator SlideInSequence(string sceneName)
    {
        Image img = slideImageRect_.GetComponent<Image>();
        if (img) img.raycastTarget = true;

        float elapsed = 0;
        float screenWidth = GetComponent<RectTransform>().rect.width;

        Vector2 startPos = new Vector2(screenWidth, 0); // 右
        Vector2 endPos = Vector2.zero;                  // 中央

        slideImageRect_.anchoredPosition = startPos;

        while (elapsed < slideDuration_)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / slideDuration_);
            slideImageRect_.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        slideImageRect_.anchoredPosition = endPos;

        // ★次のシーンへ「スライド中だよ」と伝える
        isSliding_ = true;

        SceneManager.LoadScene(sceneName);
    }


    /// <summary>
    /// スライドアウト
    /// </summary>
    private IEnumerator SlideOutSequence()
    {
        // 1. まず強制的に画面中央（真っ暗な状態）にセット
        slideImageRect_.anchoredPosition = Vector2.zero;

        Image img = slideImageRect_.GetComponent<Image>();
        if (img) img.raycastTarget = true; // 操作ブロック継続

        float elapsed = 0;
        float screenWidth = GetComponent<RectTransform>().rect.width;

        Vector2 startPos = Vector2.zero;                 // 中央
        Vector2 endPos = new Vector2(-screenWidth, 0);   // 左

        // 2. 左へ抜けていくアニメーション
        while (elapsed < slideDuration_)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / slideDuration_);
            slideImageRect_.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        slideImageRect_.anchoredPosition = endPos;

        if (img) img.raycastTarget = false; // 操作ブロック解除

        // ★フラグをリセット（終わったよ）
        isSliding_ = false;
    }
}