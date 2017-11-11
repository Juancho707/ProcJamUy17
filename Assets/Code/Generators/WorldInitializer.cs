using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WorldInitializer : MonoBehaviour
{
    private UnityAction worldGenerationListener;
    private WorldMatrixConstructor worldMatrixConstructor;
    private GameObject[] worldTiles = { };
    public int ZoneAmount = 10;
    public int MaxTilesPerZone = 10;

    void Awake()
    {
        worldGenerationListener = DrawMapOnScreen;
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.worldGenerationEnd, worldGenerationListener);
    }

    void Start()
    {
        worldMatrixConstructor = GetComponent<WorldMatrixConstructor>();
        worldTiles = Resources.LoadAll<GameObject>("World");
        if (worldMatrixConstructor != null)
        {
            worldMatrixConstructor.InitializeGeneration(ZoneAmount, MaxTilesPerZone);
        }
        // If you store all your items that you want to load in the same folder (Assets/Resources/MyItemsToLoad).
        // TODO move to string constants static class.
    }

    private void DrawMapOnScreen()
    {
        // Assuming two-dimensional matrix.
        for (int i = 0; i < worldMatrixConstructor.WorldMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < worldMatrixConstructor.WorldMatrix.GetLength(1); j++)
            {
                DrawSingleWorldUnit(worldMatrixConstructor.WorldMatrix[i, j], i, j);
            }
        }
    }

    public void DrawSingleWorldUnit(WorldCellType worldCellType, int x, int y)
    {
        string worldCellTypeStr;
        switch (worldCellType)
        {
            case WorldCellType.None:
                worldCellTypeStr = "WorldNone";
                break;
            case WorldCellType.Path:
                worldCellTypeStr = "WorldPath";
                break;
            case WorldCellType.Zone:
                worldCellTypeStr = "WorldZone";
                break;
            case WorldCellType.PlayerInitialPosition:
                // TODO handle player case.
                worldCellTypeStr = "WorldPath";
                break;
            default:
                throw new ArgumentOutOfRangeException("worldCellType", worldCellType, null);
        }
        GameObject prefab = worldTiles.Where(tile => tile.name == worldCellTypeStr).SingleOrDefault();
        if (prefab == null) throw new ArgumentNullException("prefab");
        Vector3 pos = new Vector3(x, y, 0); // TODO review z-axis value.
        var newObject = Instantiate(prefab, pos, Quaternion.identity);
        // TODO add string to constants.
        newObject.transform.parent = GameObject.Find("Cells").transform;
    }
}