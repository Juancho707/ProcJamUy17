
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionMethods
{
    public static T PickOne<T>(this IEnumerable<T> col)
    {
        return col.ToArray()[Random.Range(0, col.Count())];
    }
}