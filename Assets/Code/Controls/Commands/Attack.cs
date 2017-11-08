using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class Attack : Command
{
    public override void Execute(GameObject obj, Command comm)
    {
        AttackWithWeapon(obj);
    }


    void AttackWithWeapon(GameObject obj)
    {
        Animator playerAnim = obj.GetComponent<Animator>();
        playerAnim.SetTrigger("attack");
    }

}