using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : CharacterBaseScript
{
    // Start is called before the first frame update
    void Start()
    {
        m_moveDirction = Vector3.right;
        InitializeObject();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObject();
    }
}
