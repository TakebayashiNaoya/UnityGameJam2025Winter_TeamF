///
/// 戦闘を管理するスクリプト
///
using UnityEngine;
using System.Collections; // コルーチンを使うために必要
using TMPro;

public class BattleManagerScript : MonoBehaviour
{
    [Header("参照設定")]
    [SerializeField] private GameObject playerSpawner_;     // プレイヤースポナー
    [SerializeField] private GameObject enemySpawner_;      // エネミースポナー
    [SerializeField] private Camera mainCamera_;            // メインカメラ
    [SerializeField] private GameObject gamePlayUI_;        // ゲーム中に表示するUIの親
    [SerializeField] private GameObject winCanvasPrefab_;
    [SerializeField] private GameObject loseCanvasPrefab_;
    [SerializeField] private GameObject returnButtonPrefab_;

    [Header("演出設定")]
    [SerializeField] private float zoomSize_ = 2.0f;        // ズーム後のカメラサイズ
    [SerializeField] private float zoomDuration_ = 1.0f;    // ズームにかかる時間

    [Header("サウンド設定")]
    [SerializeField] private AudioClip battleBgm_;          // 戦闘BGM
    [SerializeField] private AudioClip winSe_;              // 勝利SE
    [SerializeField] private AudioClip loseSe_;             // 敗北SE


    private static readonly float MAP_LEFT = -9.0f;         // 左端の限界
    private static readonly float MAP_RIGHT = 9.0f;         // 右端の限界
    private static readonly float MAP_BOTTOM = -5.0f;       // 下端の限界
    private static readonly float MAP_TOP = 5.0f;           // 上端の限界
    private static readonly float Y_OFFSET = 1.5f;          // カメラのYオフセット


    private bool isGameOver_ = false;   // 二重実行防止用フラグ



    public bool IsGameOver()
    {
        return isGameOver_;
    }

    void Start()
    {
        SoundManager.instance.PlayBgm(battleBgm_);
    }

    void Update()
    {
        // ゲームオーバー後は処理しない
        if (isGameOver_)
        {
            return;
        }

        // 勝敗判定
        if (playerSpawner_.GetComponent<Spawner>().GetHealth() <= 0)
        {
            GameOver();

            if (returnButtonPrefab_ != null)
            {
                Instantiate(returnButtonPrefab_);
            }
        }
        else if (enemySpawner_.GetComponent<Spawner>().GetHealth() <= 0)
        {
            GameClear();

            if (returnButtonPrefab_ != null)
            {
                Instantiate(returnButtonPrefab_);
            }
        }
    }

    private void GameClear()
    {
        isGameOver_ = true;
        SoundManager.instance.StopBgm();

        if (winSe_ != null)
        {
            SoundManager.instance.PlaySe(winSe_);
        }


        // ゲーム中UIを非表示
        Canvas pCanvas = playerSpawner_.GetComponentInChildren<Canvas>();
        if (pCanvas != null)
        {
            pCanvas.gameObject.SetActive(false);
        }
        Canvas eCanvas = enemySpawner_.GetComponentInChildren<Canvas>();
        if (eCanvas != null)
        {
            eCanvas.gameObject.SetActive(false);
        }
        if (gamePlayUI_ != null)
        {
            gamePlayUI_.SetActive(false);
        }


        // 演出開始
        if (winCanvasPrefab_ != null)
        {
            Instantiate(winCanvasPrefab_);
        }


        // カメラを敵スポナーより少し上にズーム
        Vector3 targetPos = enemySpawner_.transform.position + new Vector3(0, Y_OFFSET, 0);
        StartCoroutine(ZoomCamera(targetPos));
    }

    private void GameOver()
    {
        isGameOver_ = true;
        SoundManager.instance.StopBgm();

        if (loseSe_ != null)
        {
            SoundManager.instance.PlaySe(loseSe_);
        }

        // ゲーム中UIを非表示
        Canvas pCanvas = playerSpawner_.GetComponentInChildren<Canvas>();
        if (pCanvas != null)
        {
            pCanvas.gameObject.SetActive(false);
        }
        Canvas eCanvas = enemySpawner_.GetComponentInChildren<Canvas>();
        if (eCanvas != null)
        {
            eCanvas.gameObject.SetActive(false);
        }
        if (gamePlayUI_ != null)
        {
            gamePlayUI_.SetActive(false);
        }

        // 演出開始
        if (loseCanvasPrefab_ != null)
        {
            Instantiate(loseCanvasPrefab_);
        }

        // カメラをプレイヤースポナーより少し上にズーム
        Vector3 targetPos = playerSpawner_.transform.position + new Vector3(0, Y_OFFSET, 0);
        StartCoroutine(ZoomCamera(targetPos));
    }


    /// <summary>
    /// カメラを滑らかにズームさせるコルーチン
    /// </summary>
    private IEnumerator ZoomCamera(Vector3 targetPos)
    {
        float elapsed = 0;
        float startSize = mainCamera_.orthographicSize;
        Vector3 startPos = mainCamera_.transform.position;

        while (elapsed < zoomDuration_)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomDuration_;
            t = Mathf.SmoothStep(0, 1, t);

            // 1. サイズを更新
            mainCamera_.orthographicSize = Mathf.Lerp(startSize, zoomSize_, t);

            // 2. 「今」のカメラサイズから限界位置を計算
            float camH = mainCamera_.orthographicSize;
            float camW = camH * mainCamera_.aspect;

            // 3. 現在の移動先(Lerp結果)を計算してから、それをステージ内に閉じ込める
            Vector3 currentTargetPos = Vector3.Lerp(startPos, targetPos, t);

            float clampedX = Mathf.Clamp(currentTargetPos.x, MAP_LEFT + camW, MAP_RIGHT - camW);
            float clampedY = Mathf.Clamp(currentTargetPos.y, MAP_BOTTOM + camH, MAP_TOP - camH);

            // 4. Z座標を維持して適用
            mainCamera_.transform.position = new Vector3(clampedX, clampedY, startPos.z);

            yield return null;
        }
    }


    /// <summary>
    /// インスペクターで設定したステージ範囲を赤い枠で表示
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3((MAP_LEFT + MAP_RIGHT) / 2, (MAP_TOP + MAP_BOTTOM) / 2, 0);
        Vector3 size = new Vector3(MAP_RIGHT - MAP_LEFT, MAP_TOP - MAP_BOTTOM, 1);
        Gizmos.DrawWireCube(center, size);
    }
}