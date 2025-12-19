using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : Actor
{
    [Header("体力表示のTMP"), SerializeField]
    private TextMeshProUGUI hpText_;


    protected override void InitializeObject()
    {
        //体力を最大体力に設定
        health_ = maxHealth_;
    }

    protected override void UpdateObject()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (hpText_ != null)
        {
            // HP表示
            hpText_.text = health_.ToString() + " / " + maxHealth_.ToString();
        }
    }


    /// <summary>
    /// 現在の体力を取得する関数
    /// </summary>
    public int GetHealth()
    {
        return health_;
    }
}
