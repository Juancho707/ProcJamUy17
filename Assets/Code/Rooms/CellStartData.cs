using System;
using UnityEngine;

[Serializable]
public class CellStartData
{
    public static int Level;

    public int X;
    public int Y;
    public Direction FromDir;
    public Direction NextDir;
    public EdgeType FromEdge;
    public GameObject CellPrefab;
    public GameObject[] Repo;
    public LevelGenSettings Settings;
    public bool IsMainPath;
}
