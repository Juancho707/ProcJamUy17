using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int Level;
    public LevelGenSettings Settings;

    private RoomBase[,] matrix;
    private GameObject[] roomRepo;
    
	void Start ()
	{
	    matrix = new RoomBase[Settings.LevelWidth, Settings.LevelHeight];
	}
}
