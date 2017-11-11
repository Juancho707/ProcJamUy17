using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovementBase : MonoBehaviour
{
    public CharacterStats Stats;
    public float CharacterHeight;

    private float currentSpeed;
    private float jumpElapsed;

    private bool facingRight = true;
    private Animator animator;
    private SpriteRenderer bodySprite;

    private bool IsGrounded
    {
        get
        {
            Physics2D.queriesStartInColliders = false;              //Esto está para que al tirar el Raycast no le dé bola al propio collider del objeto
            return Physics2D.Raycast(this.transform.position, Vector2.down, CharacterHeight);
        }
    }

    private bool AgainstWall
    {
        get
        {
            Physics2D.queriesStartInColliders = false;
            return (Physics2D.Raycast(this.transform.position, Vector2.left, CharacterHeight) || Physics2D.Raycast(this.transform.position, Vector2.right, CharacterHeight));
        }
    }


    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        bodySprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void Move(float hAxis)
    {
        if (hAxis != 0)
        {
            currentSpeed += hAxis * Stats.Acceleration;
            if (Mathf.Abs(currentSpeed) > Stats.MaxSpeed)
            {
                currentSpeed = currentSpeed < 0 ? -Stats.MaxSpeed : Stats.MaxSpeed;
            }

            if(IsGrounded)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("onWall", false);
            }
            else
            {
                if(AgainstWall)
                {
                    //Debug.Log("Against wall");
                    animator.SetBool("onWall", true);
                }
            }
            
            if (hAxis > 0)
            {

                facingRight = true;
            }
            else
            {
                facingRight = false;
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("onWall", false);
            if (currentSpeed > 0)
            {
                currentSpeed -= Stats.Acceleration;
                if (currentSpeed < 0.2f)
                {
                    currentSpeed = 0;
                }
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += Stats.Acceleration;
                if (currentSpeed > -0.2f)
                {
                    currentSpeed = 0;
                }
            }
        }

        gameObject.transform.Translate(Vector2.right * currentSpeed);

        if (facingRight)
        {
            bodySprite.flipX = false;
        }
        else
        {
            bodySprite.flipX = true;
        }
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
    }

    public void Jump()
    {
        if (IsGrounded)
        {
            jumpElapsed = Stats.JumpDuration;
            animator.SetTrigger("jump");
            gameObject.transform.Translate(Vector2.up * Stats.JumpAcceleration);
        }
        else
        {
            if (jumpElapsed > 0)
            {
                gameObject.transform.Translate(Vector2.up * Stats.JumpAcceleration);
                jumpElapsed -= Time.fixedDeltaTime;
            }
        }
    }

    public void Roll()
    {
        animator.SetTrigger("roll");
    }

    public void WallJump()
    {
        
    }

}