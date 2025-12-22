using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // テキスト表示用（もしあれば）

public class StageSelectManager : MonoBehaviour
{
    [Header("ステージごとのシーン名リスト")]
    [SerializeField] private string[] stageSceneNames_; // 例: "Stage1", "Stage2", "Stage3"...

    [Header("現在のステージ名を表示するテキスト (任意)")]
    [SerializeField] private TextMeshProUGUI stageNameText_;

    // 現在選んでいるステージの番号（0が最初のステージ）
    private int currentStageIndex_ = 0;

    private SceneChanger sceneChanger_;

    void Start()
    {
        sceneChanger_ = FindObjectOfType<SceneChanger>();
        UpdateUI(); // 最初の表示更新
    }

    // =================================================
    //  ボタンから呼ぶ関数
    // =================================================

    /// <summary>
    /// 「→」ボタンを押したとき
    /// </summary>
    public void OnClickNext()
    {
        currentStageIndex_++;
        // 最後のステージを超えたら最初に戻す（ループ）
        if (currentStageIndex_ >= stageSceneNames_.Length)
        {
            currentStageIndex_ = 0;
        }
        UpdateUI();
    }

    /// <summary>
    /// 「←」ボタンを押したとき
    /// </summary>
    public void OnClickPrev()
    {
        currentStageIndex_--;
        // 最初より前に行ったら最後に回す（ループ）
        if (currentStageIndex_ < 0)
        {
            currentStageIndex_ = stageSceneNames_.Length - 1;
        }
        UpdateUI();
    }


    /// <summary>
    /// ステージの画像ボタンから呼ばれる。指定した番号を選択状態にする。
    /// </summary>
    /// <param name="stageIndex">ステージ番号（0が最初）</param>
    public void OnClickSelectStage(int stageIndex)
    {
        // 範囲外の数字が来ないようにチェック
        if (stageIndex >= 0 && stageIndex < stageSceneNames_.Length)
        {
            currentStageIndex_ = stageIndex;
            UpdateUI(); // 画面更新（テキストなどを切り替え）
        }
    }


    /// <summary>
    /// 「START」ボタンを押したとき
    /// </summary>
    public void OnClickStartButton()
    {
        // 現在の番号に対応するシーン名を取得
        string targetScene = stageSceneNames_[currentStageIndex_];

        // シーン遷移実行
        if (sceneChanger_ != null)
        {
            sceneChanger_.SlideAndLoadScene(targetScene);
        }
        else
        {
            SceneManager.LoadScene(targetScene);
        }
    }

    // =================================================
    //  内部処理
    // =================================================

    /// <summary>
    /// 画面の表示を更新する
    /// </summary>
    private void UpdateUI()
    {
        // 画面中央のテキストを書き換える（もし設定されていれば）
        if (stageNameText_ != null)
        {
            // "STAGE 1", "STAGE 2" のように表示
            stageNameText_.text = "STAGE " + (currentStageIndex_ + 1);
        }

        // ★ここに「中央の画像を切り替える処理」などを追加できます
        // 例: stageImage.sprite = stageSprites[currentStageIndex_];
    }
}