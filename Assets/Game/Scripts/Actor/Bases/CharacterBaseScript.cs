//
//キャラクターベースクラス
//
using Unity.VisualScripting;
using UnityEngine;

public enum CharacterState
{
    None,
    Idle,
    Walk,
    Attack,
    Die
}


public enum CharacterType
{
    None,
    Player,
    Enemy
}


public class CharacterBaseScript : ActorScript
{
    [Header("出撃に必要なお金"),    SerializeField] private int m_needMoney;
    [Header("体力"),                SerializeField] private float m_health;
    [Header("移動速度"),            SerializeField] private float m_moveSpeed;
    [Header("攻撃力"),              SerializeField] private float m_attackPower;
    [Header("射程"),                SerializeField] private float m_attackRange;
    [Header("当たり判定"),          SerializeField] private float m_bodyRange;
    [Header("再出撃までの時間"),    SerializeField] private float m_spawnInterval;
    [Header("再攻撃までの時間"),    SerializeField] private float m_attackInterval;
    [Header("攻撃にかかる時間"),    SerializeField] private float m_attackkingTime;
    [Header("間合い"),              SerializeField] private SphereCollider m_attackCollider;
    [Header("自身の当たり判定"),    SerializeField] private SphereCollider m_bodyCollider;
    [Header("キャラタイプ"),        SerializeField] private CharacterType m_characterType;


    private float m_attakIntervalTimer = 0.0f;  //待機時間計測用タイマー
    private float m_attackkingTimer = 0.0f;     //攻撃時間計測用タイマー
    private bool m_isDetectingEnemy = false;    //敵を感知しているかどうか
    private bool m_canAttack = true;            //攻撃可能かどうか
    private bool m_isAttackking = false;        //攻撃中かどうか
    private bool m_isDie = false;               //死亡しているかどうか

    protected Vector3 m_moveDirction = Vector3.zero;    //移動方向ベクトル

    private CharacterState m_currentState = CharacterState.None;
    private Rigidbody m_rb;


    private int m_playerUnitID = 0;
    private int m_enemyUnitID = 0;

    //キャラ生産に必要なお金を取得
    public int GetNeedMoney()
    {
       return m_needMoney;
    }



// Start is called before the first frame update
void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// インスペクター上で値が変更されたときに呼ばれる
    /// </summary>
    private void OnValidate()
    {
        m_attackCollider.radius = m_attackRange;
        m_bodyCollider.radius = m_bodyRange;
    }


    protected override void InitializeObject()
    {
        m_attackCollider.radius = m_attackRange;
        m_bodyCollider.radius = m_bodyRange;

        m_rb = GetComponent<Rigidbody>();
        //重力無効化
        m_rb.useGravity = false;
        //慣性挙動無効化
        m_rb.isKinematic = true;

        //プレイヤーと敵のレイヤーのIDを取得
        m_playerUnitID = LayerMask.NameToLayer("PlayerUnit");
        m_enemyUnitID = LayerMask.NameToLayer("EnemyUnit");

        m_currentState = CharacterState.Walk;
        WalkStateEnter();
    }


    protected override void UpdateObject()
    {
        CharacterStateMachine();
    }


    private void CharacterStateMachine()
    {
        switch (m_currentState)
        {
            case CharacterState.Idle:
                IdleStateUpdate();
                IdleChangeState();
                break;
            case CharacterState.Walk:
                break;
            case CharacterState.Attack:
                AttackStateUpdate();
                AttackChangeState();
                break;
            case CharacterState.Die:
                DieStateUpdate();
                DieChangeState();
                break;
            default:
                Debug.LogError("不正なステートです");
                break;
        }
    }


    //物理演算更新処理
    private void FixedUpdate()
    {
        if (m_currentState == CharacterState.Walk)
        {
            WalkStateUpdate();
            WalkChangeState();
        }
    }


    //アイドルステートの初期化処理
    private void IdleStateEnter()
    {
        Debug.Log("IdleStateEnter");
    }


    //アイドルステートの更新処理
    private void IdleStateUpdate()
    {
        //攻撃のインターバル管理
        ManageAttackInterval();
    }


    //アイドルステートの終了処理
    private void IdleStateExit()
    {

    }


    //アイドルステートの状態遷移処理
    private void IdleChangeState()
    {

        //敵を感知していなければ歩きステートへ
        if (!m_isDetectingEnemy)
        {
            IdleStateExit();
            m_currentState = CharacterState.Walk;
            WalkStateEnter();
            return;
        }


        //攻撃可能なら攻撃ステートへ
        if (m_canAttack)
        {
            IdleStateExit();
            m_currentState = CharacterState.Attack;
            AttackStateEnter();
        }
        //攻撃不可なら処理を抜ける
        else
        {
            return;
        }
    }

    //歩きステートの初期化処理
    private void WalkStateEnter()
    {
        Debug.Log("WalkStateEnter");
    }


    //歩きステートの更新処理
    private void WalkStateUpdate()
    {
        //移動処理
        m_rb.MovePosition(m_rb.position + m_moveDirction * m_moveSpeed * Time.fixedDeltaTime);

        //攻撃のクールダウン管理
        ManageAttackInterval();
    }


    //歩きステートの終了処理
    private void WalkStateExit()
    {

    }


    //歩きステートの状態遷移処理
    private void WalkChangeState()
    {
        //敵を感知していなければ処理を抜ける
        if (!m_isDetectingEnemy)
        {
            return;
        }

        //攻撃可能なら攻撃ステートへ
        if (m_canAttack)
        {
            WalkStateExit();
            m_currentState = CharacterState.Attack;
            AttackStateEnter();
        }
        //攻撃不可ならアイドルステートへ
        else
        {
            WalkStateExit();
            m_currentState = CharacterState.Idle;
            IdleStateEnter();
        }
    }


    //攻撃ステートの初期化処理
    private void AttackStateEnter()
    {
        Debug.Log("AttackStateEnter");

        //攻撃中フラグを立てる
        m_isAttackking = true;
        m_canAttack = false;
        m_attackkingTimer = 0.0f;
        m_attakIntervalTimer = 0.0f;
    }


    //攻撃ステートの更新処理
    private void AttackStateUpdate()
    {
        ManageAttackkingTimer();
    }


    //攻撃ステートの終了処理
    private void AttackStateExit()
    {
        
    }


    //攻撃ステートの状態遷移処理
    private void AttackChangeState()
    {
        //まだ攻撃中であれば処理を抜ける
        if (m_isAttackking)
        {
            return;
        }

        //攻撃完了

        //敵を感知していればアイドルステートへ
        if (m_isDetectingEnemy)
        {
            AttackStateExit();
            m_currentState = CharacterState.Idle;
            IdleStateEnter();
        }
        //敵を感知していなければ歩きステートへ
        else
        {
            AttackStateExit();
            m_currentState = CharacterState.Walk;
            WalkStateEnter();
        }
    }


    //死亡ステートの初期化処理
    private void DieStateEnter()
    {
        Debug.Log("DieStateEnter");
    }


    //死亡ステートの更新処理
    private void DieStateUpdate()
    {

    }


    //死亡ステートの終了処理
    private void DieStateExit()
    {
    }


    //死亡ステートの状態遷移処理
    private void DieChangeState()
    {

    }



    //当たり判定に敵が滞在しているときの処理
    private void OnTriggerStay(Collider other)
    {
        //感知したオブジェクトがプレイヤーか敵でなければ処理を抜ける
        if (other.gameObject.layer != m_playerUnitID && 
            other.gameObject.layer != m_enemyUnitID)
        {
            return;
        }

        m_isDetectingEnemy = true;
    }


    //当たり判定から敵が出たときの処理
    private void OnTriggerExit(Collider other)
    {
        //感知したオブジェクトがプレイヤーか敵でなければ処理を抜ける
        if (other.gameObject.layer != m_playerUnitID &&
            other.gameObject.layer != m_enemyUnitID)
        {
            return;
        }

        m_isDetectingEnemy = false;
    }



    //攻撃のクールダウンを管理する
    private void ManageAttackInterval()
    {
        //攻撃中であれば処理を抜ける
        if (m_isAttackking)
        {
            m_canAttack = false;
            m_attakIntervalTimer = 0.0f;
            return;
        }

        m_attakIntervalTimer += Time.deltaTime;

        //攻撃インターバル時間を超えたら攻撃可能にする
        if (m_attakIntervalTimer >= m_attackInterval)
        {
            //フラグとタイマーをリセット
            m_canAttack = true;
            m_attakIntervalTimer = 0.0f;
        }
    }


    //攻撃実行中の時間を管理する
    private void ManageAttackkingTimer()
    {
        //現在攻撃可能であれば処理を抜ける
        if (m_canAttack)
        {
            m_isAttackking = false;
            m_attackkingTimer = 0.0f;
            return;
        }


        m_attackkingTimer += Time.deltaTime;

        //攻撃時間を超えたら攻撃完了
        if (m_attackkingTimer >= m_attackkingTime)
        {
            //フラグとタイマーをリセット
            m_isAttackking = false;
            m_attackkingTimer = 0.0f;
        }
    }
}