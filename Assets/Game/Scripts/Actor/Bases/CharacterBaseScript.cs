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
    [SerializeField] int m_needMoney;
    [SerializeField] float m_health;
    [SerializeField] float m_moveSpeed;
    [SerializeField] float m_spawnInterval;

    [SerializeField] float m_attackPower;
    [SerializeField] float m_range;
    [SerializeField] float m_attackInterval;
    [SerializeField] float m_attackingTime;

    private float m_standingTimer = 0.0f;
    private float m_attackingTimer = 0.0f;
    private bool m_canAttack = true;
    private bool m_isDie = false;

    CharacterState currentState = CharacterState.None;

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
        currentState = CharacterState.Walk;
    }


    protected override void UpdateObject()
    {
        CharacterStateMachine();
    }


    void CharacterStateMachine()
    {
        switch (currentState)
        {
            case CharacterState.Idle:
                IdleStateUpdate();
                IdleChangeState();
                break;
            case CharacterState.Walk:
                WalkStateUpdate();
                break;
            case CharacterState.Attack:
                break;
            case CharacterState.Die:
                break;
            default:
                break;
        }
    }


    void IdleStateUpdate()
    {
        m_standingTimer += Time.deltaTime;
        
    }

    void IdleChangeState()
    {
        if (m_standingTimer >= m_attackInterval)
        {
            m_standingTimer = 0.0f;
            m_canAttack = true;
        }
    }

    void WalkStateUpdate()
    {
        transform.Translate(Vector3.right * m_moveSpeed * Time.deltaTime);
    }

    void WalkChangeState()
    {

    }


    void AttackStateUpdate()
    {
        m_attackingTimer += Time.deltaTime;
    }


    void AttackChangeState()
    {
        if (m_attackingTimer >= m_attackingTime)
        {

        }
    }
}
