using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class MoveLeft : Command
{
    public override void Execute(GameObject obj, Command comm)
    {
        MoveToLeft(obj);
    }

    void MoveToLeft(GameObject obj)
    {
        //Debug.Log("Moving left");
        var movSpeed = obj.GetComponent<MoveStats>().MoveSpeed;     //Voy a las move stats(script) del objeto y me traigo el valor del move speed
        obj.GetComponent<MoveStats>().FacingRight = false;           //Cambio el state de FacingRight a false porque estoy mirando hacia la izquierda
        obj.GetComponent<Animator>().SetBool("isWalking", true);    //Seteo la animacion de caminar   
        obj.GetComponentInChildren<SpriteRenderer>().flipX = true;  //Como va hacia la izq invierto el sprite
        obj.transform.Translate(Vector2.left * movSpeed * Time.deltaTime);  //Aplico movimiento al objeto
    }
}