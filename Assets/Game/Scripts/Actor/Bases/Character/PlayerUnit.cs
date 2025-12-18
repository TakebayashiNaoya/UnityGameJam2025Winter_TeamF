using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : CharacterBaseScript
{
    // Start is called before the first frame update
    void Start()
    {
        characterType_ = CharacterType.Player;
        myTag_ = CharacterType.Player.ToString();
        targetTag_ = CharacterType.Enemy.ToString();

        myLayer_ = LayerMask.NameToLayer("PlayerUnit");
        targetLayer_ = LayerMask.NameToLayer("EnemyUnit");

        moveDirction_ = Vector3.left;
        InitializeObject();

        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObject();
    }
}
