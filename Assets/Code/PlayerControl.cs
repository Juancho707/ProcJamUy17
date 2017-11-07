using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    private bool facingRight = true;
    private Animator playerAnim;
    private SpriteRenderer bodySprite;

    public float Speed=5f;
    public float DashForce=10f;
    public float JumpForce=8f;    

    // Use this for initialization
    void Start()
    {
        playerAnim = gameObject.GetComponent<Animator>();
        bodySprite = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Move();

        if (Input.GetButtonDown("Attack"))
        {
            Attack();
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonDown("Roll"))
        {
            Roll();
        }


        if (facingRight)
        {
            bodySprite.flipX = false;
        }
        else
        {
            bodySprite.flipX = true;
        }

    }

    void Move()
    {
        var horizValue = Input.GetAxis("Horizontal");

        if (horizValue != 0)
        {
            playerAnim.SetBool("isWalking", true);
            if(horizValue > 0.5)   //Voy hacia la derecha
            {
                gameObject.transform.Translate( Vector2.right * Speed * Time.deltaTime);
                facingRight = true;               
            }
            else  //Voy hacia la izquierda
            {
                if(horizValue < -0.5)
                {
                    gameObject.transform.Translate(Vector2.left * Speed * Time.deltaTime);
                    facingRight = false;
                } 
            }
        }
        else
        {
            playerAnim.SetBool("isWalking", false);
        }
    }

    void Attack()
    {
        playerAnim.SetTrigger("attack");
    }

    void Jump()
    {
        playerAnim.SetTrigger("jump");
    }

    void Roll()
    {
        playerAnim.SetTrigger("roll");
    }
}