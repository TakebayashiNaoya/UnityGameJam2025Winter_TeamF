///
///キャラスポーンボタンクラス
///
using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonScript : MonoBehaviour
{

    // ゲームオブジェクトのMoneyManagerを登録する
    [SerializeField] private GameObject MoneyManager;

    // MoneyManagerにアタッチされたスクリプトを保持する
    private MoneyScript moneyScript;

    // ボタン
    private Button spawnButton;

    // キャラのスポーンにかかるコスト
    [SerializeField] private int SpawnCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    void Update()
    {
        moneyScript = MoneyManager.GetComponent<MoneyScript>();

        spawnButton = GetComponent<Button>();

        // コストよりも現在のお金があるのなら
        if(moneyScript._currentMoney > SpawnCost)
        {
            spawnButton.onClick.AddListener(() =>
            {
                moneyScript.Spawn(SpawnCost);
            });
        }
    }
}
