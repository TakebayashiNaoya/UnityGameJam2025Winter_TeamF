//
//アクタークラス
//
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [Header("最大体力"),         SerializeField] protected int maxHealth_;
    [Header("自身の当たり判定"), SerializeField] protected SphereCollider bodyCollider_;


    protected int health_ = 0;              //現在の体力

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    protected abstract void InitializeObject();


    protected abstract void UpdateObject();



    //ダメージを受け取る処理
    public void ReceiveDamage(int damage)
    {
        health_ -= damage;
        Debug.Log("Damage:" + damage);
    }
}
