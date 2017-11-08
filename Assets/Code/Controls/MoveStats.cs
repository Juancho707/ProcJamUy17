using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este script contiene los valores de jump, move speed, roll speed, etc de la entidad, ya sea player, enemy1, enemy2, etc
//Leo estos valores en el Command respectivo

public class MoveStats : MonoBehaviour {

    public float MoveSpeed = 2f;
    public float JumpForce = 10f;
    public float RollSpeed = 4f;
    public bool FacingRight = true;
}
