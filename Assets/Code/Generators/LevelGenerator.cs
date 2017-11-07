using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int Level;
    public LevelGenSettings Settings;
    public Transform CellArray;
    public GameObject CellPrefab;
    private GameObject[] roomRepo;

    void Start()
    {
        roomRepo = Resources.LoadAll<GameObject>("Rooms");
        var cell = Instantiate(CellPrefab, Vector3.zero, Quaternion.identity).GetComponent<CellStarter>();

        var cellData = new CellStartData
        {
            FromDir = Direction.Right,
            FromEdge = new[] { EdgeType.A, EdgeType.B, EdgeType.C, EdgeType.D }.PickOne(),
            X = 0,
            Y = 0,
            Repo = roomRepo,
            Settings = this.Settings,
            CellPrefab = this.CellPrefab,
            IsMainPath = true
        };

        CellStartData.Level = this.Level;
        cell.SpawnCell(cellData);
    }
}
