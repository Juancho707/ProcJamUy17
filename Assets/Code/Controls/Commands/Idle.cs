using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class Idle : Command
{
    public override void Execute(GameObject obj, Command comm)
    {
        GoIdle(obj);
    }


    void GoIdle(GameObject obj)
    {
        bool faceRight = obj.GetComponent<MoveStats>().FacingRight;
        obj.GetComponent<Animator>().SetBool("isWalking", false);                //Desactivo la animacion de caminar
        obj.GetComponentInChildren<SpriteRenderer>().flipX = !faceRight;        
    }

}