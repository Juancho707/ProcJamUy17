using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    private CharacterAdvancedMovement movement;
    
    // Use this for initialization
    void Start()
    {
        movement = this.GetComponent<CharacterAdvancedMovement>();
    }

    void FixedUpdate()
    {
        movement.Move(Input.GetAxis("Horizontal"));

        if (Input.GetButtonDown("Attack"))
        {
            movement.Attack();
        }

        if(Input.GetButton("Jump") && !movement.AgainstWall)
        {
            movement.Jump();
        }
        else
        {
            if(Input.GetButton("Jump"))
                movement.WallJump();
        }

        if (Input.GetButtonDown("Roll"))
        {
            movement.Roll();
        }

    }
}