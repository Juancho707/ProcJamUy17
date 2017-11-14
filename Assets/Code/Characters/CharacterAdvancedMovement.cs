using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAdvancedMovement : CharacterMovementBase {
    private Animator advAnimator;
    private SpriteRenderer advBodySprite;
    private bool wallJumped=false;


    public float rollForce = 50f;
    public float rollTime = 0f;
    public float MaxRollTime = 1f;

    public bool IsRolling = false;


    // Use this for initialization
    void Start()
    {
        advAnimator = gameObject.GetComponent<Animator>();
        animator = advAnimator;
        advBodySprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        bodySprite = advBodySprite;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(IsRolling)
        {
            float move;
            if(facingRight)
            {
                move = 1;
            }
            else
            {
                move = -1;
            }

            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(move * rollForce,0);
            rollTime += Time.fixedDeltaTime;
            if (rollTime >= MaxRollTime)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                IsRolling = false;
                advAnimator.SetBool("isRolling", false);
                rollTime = 0;
            }
        }
    }

    public void Roll()
    {
        if(rollTime==0)
        {
            IsRolling = true;
            advAnimator.SetBool("isRolling", true);
        }
    }

    public void WallJump()
    {
        if (!wallJumped)
        {
            jumpElapsed = Stats.JumpDuration;
            advAnimator.SetTrigger("wallJump");
            wallJumped = true;
            gameObject.transform.Translate(Vector2.up * Stats.JumpAcceleration);
        }
        else
        {
            if (jumpElapsed > 0)
            {
                gameObject.transform.Translate(Vector2.up * Stats.JumpAcceleration);
                jumpElapsed -= Time.fixedDeltaTime;
            }
            else
            {
                wallJumped = false;
            }
        }
    }
}
