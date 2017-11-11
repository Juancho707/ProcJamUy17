using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WorldInitializer : MonoBehaviour
{
    private UnityAction worldGenerationListener;
    private WorldGenerator worldGenerator;
    private GameObject[] worldTiles = { };
    public int ZoneAmount = 10;
    public int MaxTilesPerZone = 10;

    void Awake()
    {
        worldGenerationListener = new UnityAction(DrawMapOnScreen);
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.worldGenerationEnd, worldGenerationListener);
    }

    void Start()
    {
        worldGenerator = GetComponent<WorldGenerator>();
        worldTiles = Resources.LoadAll<GameObject>("World");
        if (worldGenerator != null)
        {
            worldGenerator.InitializeGeneration(ZoneAmount, MaxTilesPerZone);
        }
        // If you store all your items that you want to load in the same folder (Assets/Resources/MyItemsToLoad).
        // TODO move to string constants static class.
    }

    private void DrawMapOnScreen()
    {
        // TODO trigger event since generation ended.
        List<string> worldStringRepresentation =
            WorldGeneratorHelper.GetWorldStringRepresentation(worldGenerator.worldMatrix);
        foreach (string row in worldStringRepresentation)
        {
            Debug.Log(row);
        }
        Debug.Log(string.Format("Length of worldTiles is {0}", worldTiles.Length));
        foreach (var tile in worldTiles)
        {
            Debug.Log(tile.name);
        }
        // Assuming two-dimensional matrix.
        for (int i = 0; i < worldGenerator.worldMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < worldGenerator.worldMatrix.GetLength(1); j++)
            {
                DrawSingleWorldUnit(worldGenerator.worldMatrix[i, j], i, j);
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
        Instantiate(prefab, pos, Quaternion.identity);
    }
}