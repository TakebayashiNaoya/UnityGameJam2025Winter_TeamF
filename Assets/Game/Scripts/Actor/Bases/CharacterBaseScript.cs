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
    [Header("攻撃にかかる時間"),    SerializeField] private float m_attackingTime;
    [Header("間合い"),              SerializeField] private SphereCollider m_attackCollider;
    [Header("自身の当たり判定"),    SerializeField] private SphereCollider m_bodyCollider;
    [Header("キャラタイプ"),        SerializeField] private CharacterType m_characterType;


    private float m_attakIntervalTimer = 0.0f;  //待機時間計測用タイマー
    private float m_attackingTimer = 0.0f;      //攻撃時間計測用タイマー
    private bool m_canAttack = true;            //攻撃可能かどうか
    private bool m_isAttakking = false;         //攻撃中かどうか
    private bool m_isAttackInterval = false;    //攻撃がクールダウン中かどうか
    private bool m_isDie = false;               //死亡しているかどうか

    private Vector3 m_moveDirction = Vector3.zero;    //移動方向ベクトル

    private CharacterState m_currentState = CharacterState.None;
    private Rigidbody m_rb;

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

        switch (m_characterType)
        {
            case CharacterType.Player:
                m_moveDirction = Vector3.left;
                break;
            case CharacterType.Enemy:
                m_moveDirction = Vector3.right;
                break;
            default:
                break;
        }


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
                //WalkStateUpdate();
                //WalkChangeState();
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
                break;
        }
    }


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
    }

    //アイドルステートの更新処理
    private void IdleStateUpdate()
    {
        ManageAttackInterval();
    }


    //アイドルステートの終了処理
    private void IdleStateExit()
    {

    }


    //アイドルステートの状態遷移処理
    private void IdleChangeState()
    {
        if (m_canAttack)
        {
            IdleStateExit();
            m_currentState = CharacterState.Attack;
            AttackStateEnter();
        }
    }



    //歩きステートの初期化処理
    private void WalkStateEnter()
    {

    }

    //歩きステートの更新処理
    private void WalkStateUpdate()
    {
        //移動処理
        m_rb.MovePosition(m_rb.position + m_moveDirction * m_moveSpeed * Time.fixedDeltaTime);

        //transform.Translate(Vector3.right * m_moveSpeed * Time.deltaTime);

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

    }


    //攻撃ステートの初期化処理
    private void AttackStateEnter()
    {
        Debug.Log("AttackStateEnter");

        m_attackingTimer = 0.0f;
        m_canAttack = false;
        m_isAttakking = true;
    }


    //攻撃ステートの更新処理
    private void AttackStateUpdate()
    {
        m_attackingTimer += Time.deltaTime;
    }


    //攻撃ステートの終了処理
    private void AttackStateExit()
    {
        m_canAttack = false;
        m_isAttackInterval = true;
        m_isAttakking = false;
    }


    //攻撃ステートの状態遷移処理
    private void AttackChangeState()
    {
        //攻撃中であれば処理を抜ける
        if (m_attackingTimer <= m_attackingTime)
        {
            return;
        }

        //攻撃を終了したので、タイマーをリセット
        m_attackingTimer = 0.0f;

        AttackStateExit();
    }


    //死亡ステートの初期化処理
    private void DieStateEnter()
    {
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


    //当たり判定に敵が入ったときの処理
    private void OnTriggerEnter(Collider other)
    {
        // レイヤーによって通す
        if (other.gameObject.layer != LayerMask.NameToLayer("EnemyUnit") &&
           other.gameObject.layer != LayerMask.NameToLayer("PlayerUnit"))
        {
            return;
        }

        //自身の射程内に敵が入ったので、それぞれのステートへ遷移
        if (m_canAttack)
        {
            WalkStateExit();
            m_currentState = CharacterState.Attack;
            AttackStateEnter();
        }
        else
        {
            WalkStateExit();
            m_currentState = CharacterState.Idle;
            IdleStateEnter();
        }
    }


    //当たり判定に敵が滞在しているときの処理
    private void OnTriggerStay(Collider other)
    {
        // レイヤーによって通す
        if (other.gameObject.layer != LayerMask.NameToLayer("EnemyUnit") &&
           other.gameObject.layer != LayerMask.NameToLayer("PlayerUnit"))
        {
            return;
        }

        if (m_canAttack)
        {
            IdleStateExit();
            m_currentState = CharacterState.Attack;
            AttackStateEnter();
        }

        ManageAttackInterval();
    }


    //当たり判定から敵が出たときの処理
    private void OnTriggerExit(Collider other)
    {
        //攻撃中であれば処理を抜ける
        if (m_isAttakking)
        {
            return;
        }

        switch (m_currentState)
        {
            case CharacterState.Idle:
                IdleStateExit();
                break;
            case CharacterState.Attack:
                AttackStateExit();
                break;
            default:
                break;
        }

        m_currentState = CharacterState.Walk;
        WalkStateEnter();
    }



    //攻撃のクールダウンを管理する
    private void ManageAttackInterval()
    {
        //攻撃可能であれば処理を抜ける
        if (m_canAttack)
        {
            return;
        }

        //攻撃がクールダウン中でなければ処理を抜ける
        if (!m_isAttackInterval)
        {
            return;
        }

        m_isAttackInterval = false;
        m_attakIntervalTimer += Time.deltaTime;

        //攻撃のクールダウンが終了したら、攻撃可能にする
        if (m_attakIntervalTimer >= m_attackInterval)
        {
            m_canAttack = true;
            m_attakIntervalTimer = 0.0f;
        }
    }
}