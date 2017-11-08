using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class Jump : Command
{
    public override void Execute(GameObject obj, Command comm)
    {
        Jumping(obj);
    }


    void Jumping(GameObject obj)
    {
        bool faceRight = obj.GetComponent<MoveStats>().FacingRight;                 //Guardo el state de FacingRight porque lo voy a usar mas abajo
        var jumpForce = obj.GetComponent<MoveStats>().JumpForce;
        obj.GetComponent<Animator>().SetTrigger("jump");                            //Seteo la animacion de salto
        obj.GetComponentInChildren<SpriteRenderer>().flipX = !faceRight;
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(obj.GetComponent<Rigidbody2D>().velocity.x, jumpForce);    //Aplico el salto al objeto
    }

}