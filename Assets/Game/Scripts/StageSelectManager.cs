using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] private AudioClip returnSe_;
    [SerializeField] private AudioClip selectSe_;
    [SerializeField] private AudioClip startSe_;


    [Header("ステージごとのシーン名リスト"), SerializeField]
    private string[] stageSceneNames_;

    [Header("選択中のステージ名"), SerializeField]
    private TextMeshProUGUI stageNameText_;

    [Header("スタートボタン"), SerializeField]
    private Button startButton_;

    [Header("ステージ選択ボタンのリスト"), SerializeField]
    private Button[] stageButtons_;

    [Header("選択時の色"), SerializeField]
    private Color selectedColor_ = Color.white;

    [Header("非選択時の色"), SerializeField]
    private Color unselectedColor_ = Color.gray;

    [Header("選択時の大きさ"), SerializeField]
    private float selectedScale_ = 1.1f;


    // 現在選んでいるステージの番号（初期値を -1（未選択）にする）
    private int currentStageIndex_ = -1;

    // シーン遷移用コンポーネント
    private SceneChanger sceneChanger_;


    void Start()
    {
        sceneChanger_ = FindObjectOfType<SceneChanger>();
        UpdateUI(); // 最初の表示更新
    }


    public void OnClickReturnSe()
    {
        // SEを再生
        if (returnSe_ != null)
        {
            SoundManager.instance.PlaySe(returnSe_);
        }
    }


    public void OnClickSelectSe()
    {
        // SEを再生
        if (selectSe_ != null)
        {
            SoundManager.instance.PlaySe(selectSe_);
        }
    }


    public void OnClickStartSe()
    {
        // SEを再生
        if (startSe_ != null)
        {
            SoundManager.instance.PlaySe(startSe_);
        }
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
        if (currentStageIndex_ < 0)
        {
            Debug.Log("ステージが選択されていません");
            return;
        }

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


    /// <summary>
    /// 画面の表示を更新する
    /// </summary>
    private void UpdateUI()
    {
        // テキストとスタートボタンの制御
        if (currentStageIndex_ < 0)
        {
            if (stageNameText_ != null)
            {
                stageNameText_.text = "SELECT STAGE";
            }
        }
        else
        {
            if (stageNameText_ != null)
            {
                stageNameText_.text = "STAGE " + (currentStageIndex_ + 1);
            }
        }

        // 画像の光り方（色とサイズ）を更新
        if (stageButtons_ == null)
        {
            return;
        }

        for (int i = 0; i < stageButtons_.Length; i++)
        {
            if (stageButtons_[i] == null)
            {
                continue;
            }

            // ボタンについている画像を取得して色を変える
            Image btnImage = stageButtons_[i].image;

            if (i == currentStageIndex_)
            {
                // 選択中
                if (btnImage != null)
                {
                    btnImage.color = selectedColor_;
                }
                stageButtons_[i].transform.localScale = Vector3.one * selectedScale_;
            }
            else
            {
                // 非選択
                if (btnImage != null)
                {
                    btnImage.color = unselectedColor_;
                }
                stageButtons_[i].transform.localScale = Vector3.one;
            }
        }
    }
}