using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : CharacterBaseScript
{
    // Start is called before the first frame update
    void Start()
    {
        characterType_ = CharacterType.Enemy;
        myTag_ = CharacterType.Enemy.ToString();
        targetTag_ = CharacterType.Player.ToString();

        myLayer_ = LayerMask.NameToLayer("EnemyUnit");
        targetLayer_ = LayerMask.NameToLayer("PlayerUnit");

        moveDirction_ = Vector3.right;
        InitializeObject();

        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObject();
    }
}
