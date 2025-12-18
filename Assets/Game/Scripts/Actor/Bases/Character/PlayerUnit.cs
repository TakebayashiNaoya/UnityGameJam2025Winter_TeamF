using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : CharacterBaseScript
{
    // Start is called before the first frame update
    void Start()
    {
        m_moveDirction = Vector3.left;
        InitializeObject();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObject();
    }
}
