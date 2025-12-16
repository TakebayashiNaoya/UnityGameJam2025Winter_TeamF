using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CharacterState
{
    None,
    Idle,
    Walk,
    Attack,
    Die
}



public class CharacterBaseScript : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float moveSpeed;
    [SerializeField] float attackPower;
    [SerializeField] float attackRange;
    [SerializeField] float attackInterval;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
