///
///キャラスポーンボタンクラス
///
using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonScript : MonoBehaviour
{
    // ゲームオブジェクトのプレイヤーを登録する
    [SerializeField] private GameObject PlayerObject;

    // PlayerObjectにアタッチされたスクリプトを保持する
    private CharacterBaseScript characterBaseScript;

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
        characterBaseScript = PlayerObject.GetComponent<CharacterBaseScript>();

        moneyScript = MoneyManager.GetComponent<MoneyScript>();

        spawnButton = GetComponent<Button>();

        // コストよりも現在のお金があるのなら
        if (moneyScript._currentMoney > characterBaseScript.GetNeedMoney())
        {
            moneyScript.CanSpawn = true;

            spawnButton.onClick.AddListener(() =>
            {
                moneyScript.Spawn(characterBaseScript.GetNeedMoney());

            });
        }

        else
        {
            moneyScript.CanSpawn = false;
        }
    }
}
