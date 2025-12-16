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



public class CharacterBaseScript : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float moveSpeed;

    CharacterState currentState = CharacterState.None;

    // Start is called before the first frame update
    void Start()
    {
        currentState = CharacterState.Walk;
    }

    // Update is called once per frame
    void Update()
    {
        if ()
    }
}
