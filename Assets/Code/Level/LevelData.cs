using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int Level;
    public RoomBase[,] CellMatrix;
    public LevelGenSettings Settings;
    public Transform Cells;

    public LevelData(LevelGenSettings settings, Transform cellsContainer)
    {
        Settings = settings;
        Cells = cellsContainer;
        CellMatrix = new RoomBase[Settings.LevelWidth, Settings.LevelHeight];
    }
}
