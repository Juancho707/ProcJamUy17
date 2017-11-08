using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class MoveRight : Command
{
    public override void Execute(GameObject obj, Command comm)
    {
        MoveToRight(obj);
    }


    void MoveToRight(GameObject obj)
    {
        //Debug.Log("Moving right");
        var movSpeed = obj.GetComponent<MoveStats>().MoveSpeed;     //Voy a las move stats(script) del objeto y me traigo el valor del move speed
        obj.GetComponent<MoveStats>().FacingRight = true;           //Cambio el state de FacingRight a true porque estoy mirando hacia la derecha
        obj.GetComponent<Animator>().SetBool("isWalking", true);    //Seteo la animacion de caminar
        obj.GetComponentInChildren<SpriteRenderer>().flipX = false;
        obj.transform.Translate(Vector2.right * movSpeed * Time.deltaTime);  //Aplico movimiento al objeto
    }

}