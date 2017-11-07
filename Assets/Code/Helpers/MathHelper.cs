using UnityEngine;

public static class MathHelper
{
    public static bool DiceRoll(int chance)
    {
        return Random.Range(0,100) <= chance;
    }
}