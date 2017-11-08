using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Command
{
    public abstract void Execute(GameObject obj, Command comm);
}