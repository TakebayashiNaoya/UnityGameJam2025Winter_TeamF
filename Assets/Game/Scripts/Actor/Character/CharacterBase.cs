//
//キャラクターベースクラス
//
using UnityEngine;
using System.Collections.Generic;

public enum CharacterState
{
    None,
    Idle,
    Walk,
    Attack,
    Die,
    StateMax
}


public enum AttackType
{
    None,
    Single,
    Range
}


public enum CharacterType
{
    None,
    Player,
    Enemy
}


public class CharacterBase : Actor
{
    [Header("キャラコスト"),        SerializeField] private int charaCost_;    
    [Header("移動速度"),            SerializeField] private float moveSpeed_;
    [Header("攻撃力"),              SerializeField] private float attackPower_;
    [Header("射程"),                SerializeField] private float attackRange_;
    [Header("当たり判定"),          SerializeField] private float bodyRange_;
    [Header("再出撃までの時間"),    SerializeField] private float spawnInterval_;
    [Header("再攻撃までの時間"),    SerializeField] private float attackInterval_;
    [Header("攻撃にかかる時間"),    SerializeField] private float attackkingTime_;
    [Header("間合い"),              SerializeField] private SphereCollider attackCollider_;    
    [Header("攻撃タイプ"),          SerializeField] private AttackType attackType_;
    

    private List<Actor> foundList_ = new List<Actor>(); //感知した敵のリスト

    private float attackIntervalTimer_ = 0.0f; //待機時間計測用タイマー
    private float attackkingTimer_ = 0.0f;     //攻撃時間計測用タイマー
    private bool isFind_ = false;         //敵を感知しているかどうかのフラグ

    private CharacterState currentState_ = CharacterState.None;
    private Rigidbody rb_;

    protected string myTag_ = "";                //自分のタグ
    protected string targetTag_ = "";             //敵のタグ
    protected int myLayer_ = 0;                  //自分のレイヤーID
    protected int targetLayer_ = 0;               //敵のレイヤーID
    protected Vector3 moveDirction_ = Vector3.zero;    //移動方向ベクトル
    protected CharacterType characterType_ = CharacterType.None; //キャラクタータイプ


    //感知した敵リストをクリーンアップする
    private void CleanUpFindingTargets()
    {
        //null参照を削除
        foundList_.RemoveAll(target => target == null);

        //感知した敵がいなければフラグをfalseに設定
        if (foundList_.Count == 0)
        {
            isFind_ = false;
        }
    }

    //キャラクターの金額を取得する
    public int GetNeedMoney()
    {
       return charaCost_;
    }


    //再出撃までの時間を取得する
    public float GetSpawnInterval()
    {
        return spawnInterval_;
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
        attackCollider_.radius = attackRange_;
        bodyCollider_.radius = bodyRange_;
    }


    protected override void InitializeObject()
    {
        rb_ = GetComponent<Rigidbody>();
        //重力無効化
        rb_.useGravity = false;
        //慣性挙動無効化
        rb_.isKinematic = true;

        //当たり判定をトリガーに設定
        attackCollider_.isTrigger = true;
        bodyCollider_.isTrigger = true;

        //タイマーをリセット
        attackIntervalTimer_ = 0.0f;
        attackkingTimer_ = 0.0f;

        //体力を最大体力に設定
        health_ = maxHealth_;

        //初期ステートを歩きステートに設定
        currentState_ = CharacterState.Walk;
        WalkStateEnter();
    }


    protected override void UpdateObject()
    {
        CleanUpFindingTargets();
        CharacterStateMachine();
    }


    //キャラクターのステートマシン
    private void CharacterStateMachine()
    {
        switch (currentState_)
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
                //不正なステート
                Debug.LogError("不正なステートです");
                break;
        }
    }


    //物理演算更新処理
    private void FixedUpdate()
    {
        if (currentState_ == CharacterState.Walk)
        {
            WalkStateUpdate();
            WalkChangeState();
        }
    }


    //アイドルステートの初期化処理
    private void IdleStateEnter()
    {
        //Debug.Log("IdleStateEnter");

        //攻撃のインターバル管理用タイマーをリセット
        attackIntervalTimer_ = 0.0f;
    }


    //アイドルステートの更新処理
    private void IdleStateUpdate()
    {
        //攻撃のインターバル管理
        attackIntervalTimer_ += Time.deltaTime;
    }


    //アイドルステートの終了処理
    private void IdleStateExit()
    {
        attackIntervalTimer_ = 0.0f;
    }


    //アイドルステートの状態遷移処理
    private void IdleChangeState()
    {
        if (JudgeDie())
        {
            IdleStateExit();
            currentState_ = CharacterState.Die;
            DieStateEnter();
            return;
        }


        //敵を感知していなければ歩きステートへ
        if (!isFind_)
        {
            IdleStateExit();
            currentState_ = CharacterState.Walk;
            WalkStateEnter();
            return;
        }


        //攻撃インターバルが完了していれば攻撃ステートへ
        if (JudgeAttackIntervalComplete())
        {
            IdleStateExit();
            currentState_ = CharacterState.Attack;
            AttackStateEnter();
        }
        //攻撃インターバルが完了していなければ処理を抜ける
        else
        {
            return;
        }
    }

    //歩きステートの初期化処理
    private void WalkStateEnter()
    {
        //Debug.Log("WalkStateEnter");
    }


    //歩きステートの更新処理
    private void WalkStateUpdate()
    {
        //移動処理
        rb_.MovePosition(rb_.position + moveDirction_ * moveSpeed_ * Time.fixedDeltaTime);

        //攻撃のインターバル
        attackIntervalTimer_ += Time.deltaTime;
    }


    //歩きステートの終了処理
    private void WalkStateExit()
    {
        //Debug.Log("FoundEnemy");
    }


    //歩きステートの状態遷移処理
    private void WalkChangeState()
    {
        if (JudgeDie())
        {
            WalkStateExit();
            currentState_ = CharacterState.Die;
            DieStateEnter();
            return;
        }


        //敵を感知していなければ処理を抜ける
        if (!isFind_)
        {
            return;
        }

        //攻撃インターバルが完了していれば
        if (JudgeAttackIntervalComplete())
        {
            WalkStateExit();
            currentState_ = CharacterState.Attack;
            AttackStateEnter();
        }
        //攻撃インターバルが完了していなければ
        else
        {
            WalkStateExit();
            currentState_ = CharacterState.Idle;
            IdleStateEnter();
        }
    }


    //攻撃ステートの初期化処理
    private void AttackStateEnter()
    {
        //Debug.Log("AttackStateEnter");

        //攻撃発生タイマーをリセット
        attackkingTimer_ = 0.0f;
    }


    //攻撃ステートの更新処理
    private void AttackStateUpdate()
    {
        attackkingTimer_ += Time.deltaTime;
    }


    //攻撃ステートの終了処理
    private void AttackStateExit()
    {
        //攻撃発生タイマーをリセット
        attackkingTimer_ = 0.0f;
    }


    //攻撃ステートの状態遷移処理
    private void AttackChangeState()
    {
        if (JudgeDie())
        {
            AttackStateExit();
            currentState_ = CharacterState.Die;
            DieStateEnter();
            return;
        }

        //攻撃が完了していなければ処理を抜ける
        if (!JudgeAttackComplete())
        {
            return;
        }

        ///////////
        //攻撃完了
        ///////////

        Attack();
        attackkingTimer_ = 0.0f;

        //敵を見つけていればアイドルステートへ
        if (foundList_.Count != 0)
        {
            AttackStateExit();
            currentState_ = CharacterState.Idle;
            IdleStateEnter();
        }
        //敵を見つけていなければ歩きステートへ
        else
        {
            AttackStateExit();
            currentState_ = CharacterState.Walk;
            WalkStateEnter();
        }
    }


    //死亡ステートの初期化処理
    private void DieStateEnter()
    {
        //Debug.Log("DieStateEnter");
    }


    //死亡ステートの更新処理
    private void DieStateUpdate()
    {

    }


    //死亡ステートの終了処理
    private void DieStateExit()
    {
        //自身を削除
        Destroy(this.gameObject);
    }


    //死亡ステートの状態遷移処理
    private void DieChangeState()
    {
        DieStateExit();
    }



    //当たり判定に敵が入ったときの処理
    private void OnTriggerEnter(Collider other)
    {
        //ターゲットのCharacterBaseコンポーネントを取得
        Actor target = other.GetComponent<Actor>();

        //targetがnull参照なら処理を抜ける
        if (target == null)
        {
            return;
        }
        //targetが攻撃対象でなければ処理を抜ける
        if (target.gameObject.layer != targetLayer_)
        {
            return;
        }

        //未登録ならリストに追加
        if (!foundList_.Contains(target))
        {
            foundList_.Add(target);
        }

        isFind_ = true;
    }


    //当たり判定から敵が出たときの処理
    private void OnTriggerExit(Collider other)
    {
        //ターゲットのCharacterBaseコンポーネントを取得
        Actor target = other.GetComponent<Actor>();

        //targetがnull参照なら処理を抜ける
        if (target == null)
        {
            return;
        }

        //ターゲットが登録されていたらリストから削除
        if (foundList_.Contains(target))
        {
            foundList_.Remove(target);
        }       
    }


    //攻撃インターバルが完了したかどうかを判定
    private bool JudgeAttackIntervalComplete()
    {
        if (attackIntervalTimer_ >= attackInterval_)
        {
            return true;
        }
        return false;
    }


    //攻撃が完了したかどうかを判定
    private bool JudgeAttackComplete()
    {
        if (attackkingTimer_ >= attackkingTime_)
        {
            return true;
        }
        return false;
    }


    //死んでいるかどうかを判定
    private bool JudgeDie()
    {
        if (health_ <= 0)
        {
            return true;
        }
        return false;
    }


    //攻撃処理
    private void Attack()
    {
        //攻撃タイプによって処理を分岐
        switch (attackType_)
        {
            case AttackType.Single:
                SingleAttack();
                break;
            case AttackType.Range:
                RangeAttack();
                break;
            default:
                Debug.LogError("不正な攻撃タイプです");
                break;
        }
    }



    //範囲攻撃処理
    private void RangeAttack()
    {
        
        //攻撃処理
        Debug.Log("Attack!");

        //攻撃範囲内の攻撃対象を取得
        Collider[] targets = Physics.OverlapSphere(
            transform.position
            , attackRange_
            , LayerMask.GetMask(targetTag_)
        );


        //攻撃対象のタグを設定
        foreach (var target in targets)
        {
            if (target.CompareTag(targetTag_))
            {
                //攻撃対象にダメージを与える
                //Debug.Log("Attack Target:" + target.name);
                Actor targetCharacter = target.GetComponent<Actor>();
                targetCharacter.ReceiveDamage((int)attackPower_);
            }
        }
    }



    //単体攻撃処理
    private void SingleAttack()
    {
        //攻撃処理
        Debug.Log("Single Attack!");

        //最も近い攻撃対象を取得するための変数
        float lengthMin = float.MaxValue;

        //攻撃対象との距離を比較し、最も近い攻撃対象を取得
        foreach (var target in foundList_)
        {
            //攻撃対象の方向ベクトルを取得
            Vector3 dir = target.transform.position - transform.position;
            //攻撃対象までの距離を取得
            float length = dir.magnitude;
            //取得した距離が最小距離よりも小さければ攻撃対象を更新
            if (lengthMin > length)
            {
                lengthMin = length;
            }
        }


        foreach (var target in foundList_)
        {
            //攻撃対象の方向ベクトルを取得
            Vector3 dir = target.transform.position - transform.position;
            //攻撃対象までの距離を取得
            float length = dir.magnitude;


            //取得した距離が最小距離と同じであれば攻撃対象にダメージを与える
            if (lengthMin == length)
            {
                //Debug.Log("Attack Target:" + target.name);
                Actor targetCharacter = target.GetComponent<Actor>();
                targetCharacter.ReceiveDamage((int)attackPower_);
            }
        }
    }
}