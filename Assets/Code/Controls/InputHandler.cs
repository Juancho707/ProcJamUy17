using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InputHandler : MonoBehaviour
{
    private Command buttonAttack = new Attack();
    private Command buttonMoveLeft = new MoveLeft();
    private Command buttonMoveRight = new MoveRight();
    private Command buttonJump = new Jump();
    private Command idleComm = new Idle();
 
    // Use this for initialization
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Attack"))
        {
            buttonAttack.Execute(gameObject, buttonAttack);
        }

        if (Input.GetButtonDown("Jump"))
        {
            buttonJump.Execute(gameObject, buttonJump);
        }

        if (Input.GetButtonDown("Roll"))
        {
            Roll();
        }

        Move();
    }

    void Move()
    {
        var horizValue = Input.GetAxis("Horizontal");

        if (horizValue != 0)
        {
            
            if(horizValue > 0.5)   //Voy hacia la derecha
            {
                buttonMoveRight.Execute(gameObject, buttonMoveRight);
            }
            else  //Voy hacia la izquierda
            {
                if(horizValue < -0.5)
                {
                    buttonMoveLeft.Execute(gameObject, buttonMoveLeft);
                } 
            }
        }
        else
        {
            idleComm.Execute(gameObject, idleComm);
        }
    }

    void Roll()
    {
        //playerAnim.SetTrigger("roll");
    }
}