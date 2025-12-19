using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Actor
{
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

    }
}
