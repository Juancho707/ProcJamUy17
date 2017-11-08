using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    private CharacterMovementBase movement;
    
    // Use this for initialization
    void Start()
    {
        movement = this.GetComponent<CharacterMovementBase>();
    }

    void FixedUpdate()
    {
        movement.Move(Input.GetAxis("Horizontal"));

        if (Input.GetButtonDown("Attack"))
        {
            movement.Attack();
        }

        if (Input.GetButton("Jump"))
        {
            movement.Jump();
        }

        if (Input.GetButtonDown("Roll"))
        {
            movement.Roll();
        }
    }
}