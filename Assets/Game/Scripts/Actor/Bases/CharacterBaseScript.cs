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



public class CharacterBaseScript : ActorScript
{
    [Header("出撃に必要なお金"), SerializeField] int m_needMoney;
    [Header("体力"),             SerializeField] float m_health; 
    [Header("移動速度"),         SerializeField] float m_moveSpeed; 
    [Header("攻撃力"),           SerializeField] float m_attackPower;
    [Header("射程"),             SerializeField] float m_range; 
    [Header("再出撃までの時間"), SerializeField] float m_spawnInterval;  
    [Header("再攻撃までの時間"), SerializeField] float m_attackInterval; 
    [Header("攻撃にかかる時間"), SerializeField] float m_attackingTime;  

    private float m_standingTimer = 0.0f;       //待機時間計測用タイマー
    private float m_attackingTimer = 0.0f;      //攻撃時間計測用タイマー
    private bool m_canAttack = true;            //攻撃可能かどうか
    private bool m_isDie = false;               //死亡しているかどうか

    CharacterState m_currentState = CharacterState.None;

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
    }


    protected override void UpdateObject()
    {
        CharacterStateMachine();
    }


    void CharacterStateMachine()
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
    void IdleStateUpdate()
    {
        m_standingTimer += Time.deltaTime;
    }


    //アイドルステートの状態遷移処理
    void IdleChangeState()
    {
        if (m_standingTimer >= m_attackInterval)
        {
            m_standingTimer = 0.0f;
            m_canAttack = true;
        }
    }



    //歩きステートの更新処理
    void WalkStateUpdate()
    {
        transform.Translate(Vector3.right * m_moveSpeed * Time.deltaTime);
    }


    //歩きステートの状態遷移処理
    void WalkChangeState()
    {

    }


    //攻撃ステートの更新処理
    void AttackStateUpdate()
    {
        m_attackingTimer += Time.deltaTime;
    }


    //攻撃ステートの状態遷移処理
    void AttackChangeState()
    {
        if (m_attackingTimer >= m_attackingTime)
        {
            m_currentState = CharacterState.Walk;
            m_attackingTimer = 0.0f;
        }
    }


    //死亡ステートの更新処理
    void DieStateUpdate()
    {

    }


    //死亡ステートの状態遷移処理
    void DieChangeState()
    {

    }
}
