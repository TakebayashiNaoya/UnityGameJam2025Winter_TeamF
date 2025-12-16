using System.Collections;
using System.Collections.Generic;
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
    [Header("出撃に必要なお金"), SerializeField] private int m_needMoney;
    [Header("体力"),             SerializeField] private float m_health; 
    [Header("移動速度"),         SerializeField] private float m_moveSpeed; 
    [Header("攻撃力"),           SerializeField] private float m_attackPower;
    [Header("射程"),             SerializeField] private float m_range; 
    [Header("再出撃までの時間"), SerializeField] private float m_spawnInterval;  
    [Header("再攻撃までの時間"), SerializeField] private float m_attackInterval; 
    [Header("攻撃にかかる時間"), SerializeField] private float m_attackingTime;
    [Header("間合い"),           SerializeField] private SphereCollider m_attackCollider;
    [Header("自身の当たり判定"), SerializeField] private SphereCollider m_bodyCollider;
    [Header("キャラタイプ"),     SerializeField] private CharacterType m_characterType;


    private float m_standingTimer = 0.0f;       //待機時間計測用タイマー
    private float m_attackingTimer = 0.0f;      //攻撃時間計測用タイマー
    private bool m_canAttack = true;            //攻撃可能かどうか
    private bool m_isDie = false;               //死亡しているかどうか

    private CharacterState m_currentState = CharacterState.None;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    protected override void InitializeObject()
    {
        m_currentState = CharacterState.Walk;


        m_attackCollider.name = "AttackCollider";
        m_attackCollider.radius = m_range / 2;
        m_bodyCollider.name = "BodyCollider";
        m_bodyCollider.radius = 0.5f;
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
                WalkStateUpdate();
                WalkChangeState();
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

    //アイドルステートの更新処理
    private void IdleStateUpdate()
    {
        m_standingTimer += Time.deltaTime;
    }


    //アイドルステートの状態遷移処理
    private void IdleChangeState()
    {
        if (m_standingTimer >= m_attackInterval)
        {
            m_standingTimer = 0.0f;
            m_canAttack = true;
        }
    }



    //歩きステートの更新処理
    private void WalkStateUpdate()
    {
        transform.Translate(Vector3.right * m_moveSpeed * Time.deltaTime);
    }


    //歩きステートの状態遷移処理
    private void WalkChangeState()
    {

    }


    //攻撃ステートの更新処理
    private void AttackStateUpdate()
    {
        m_attackingTimer += Time.deltaTime;
    }


    //攻撃ステートの状態遷移処理
    private void AttackChangeState()
    {
        if (m_attackingTimer >= m_attackingTime)
        {
            m_currentState = CharacterState.Walk;
            m_attackingTimer = 0.0f;
        }
    }


    //死亡ステートの更新処理
    private void DieStateUpdate()
    {

    }


    //死亡ステートの状態遷移処理
    private void DieChangeState()
    {

    }
}
