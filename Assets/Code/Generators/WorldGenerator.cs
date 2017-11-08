using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class WorldGenerator : MonoBehaviour
{
    private int zoneAmount;
    private int maxTilesPerZone;
    private WorldCellType[,] worldMatrix;
    private List<Tuple<int, int>> zoneLocations;
    private int worldWidthAndHeight;


    void Start()
    {
        Debug.Log("test");
        zoneAmount = 100;
        maxTilesPerZone = 10;
        // Find out how many tiles there are according to how many tiles there should be per zone
        // (the zone itself and border tiles to separate it from other zones). Then, find the square root of 
        // that (always thinking about generating a square matrix/map) and approximate that number to the nearest
        // upper integer. Then, construct a square matrix with that number.
        // This is to not use a zoneAmount ** 2 matrix, but optimize it a bit.
        worldWidthAndHeight = (int) Math.Ceiling(Math.Sqrt(zoneAmount * maxTilesPerZone));
        Debug.Log(string.Format("Dimension n was found to be {0}", worldWidthAndHeight));

        worldMatrix = new WorldCellType[worldWidthAndHeight, worldWidthAndHeight];
        // Initialize world.
        // Generate available positions
        List<Tuple<int, int>> availablePositions = new List<Tuple<int, int>>();
        for (int i = 0; i < worldWidthAndHeight; i++)
        {
            for (int j = 0; j < worldWidthAndHeight; j++)
            {
                worldMatrix[i, j] = 0;
                availablePositions.Add(Tuple.Create(i, j));
            }
        }
        // Assign initial position for player (chooses randomly between four corners of the map).
        List<Tuple<int, int>> possibleInitialPositionsForPlayer = availablePositions.FindAll(t =>
            (t.Item1 == 0 || t.Item1 == worldWidthAndHeight - 1) &&
            (t.Item2 == 0 || t.Item2 == worldWidthAndHeight - 1));
        Tuple<int, int> playerInitialPosition = possibleInitialPositionsForPlayer.PickOne();
        availablePositions.Remove(playerInitialPosition);
        worldMatrix[playerInitialPosition.Item1, playerInitialPosition.Item2] = WorldCellType.PlayerInitialPosition;
        // Store zone locations in list.
        zoneLocations = new List<Tuple<int, int>>();
        // Keep adding zone locations until all have been created.
        while (zoneLocations.Count < zoneAmount)
        {
            Tuple<int, int> chosenPosition = availablePositions.PickOne();
            zoneLocations.Add(chosenPosition);
            // Remove adjacent positions (and itself) since not available for any other zones
            for (int i = chosenPosition.Item1 - 1; i <= chosenPosition.Item1 + 1; i++)
            {
                for (int j = chosenPosition.Item2 - 1; j <= chosenPosition.Item2 + 1; j++)
                {
                    Tuple<int, int> adjacentPosition =
                        availablePositions.FirstOrDefault(t => t.Item1 == i && t.Item2 == j);
                    if (adjacentPosition != null)
                    {
                        availablePositions.Remove(adjacentPosition);
                    }
                }
            }
        }
        // Go through zone locations and update matrix.
        foreach (var zoneLocation in zoneLocations)
        {
            worldMatrix[zoneLocation.Item1, zoneLocation.Item2] = WorldCellType.Zone;
        }
        PrintWorldToConsole();
        // TODO backtracking to generate path that connects all zones.
        
    }

    private void PrintWorldToConsole()
    {
        Debug.Log("bla");
        for (int i = 0; i < worldWidthAndHeight; i++)
        {
            var rowString = "";
            for (int j = 0; j < worldWidthAndHeight; j++)
            {
                rowString += string.Format("{0} ", PrintReadableWorldCell(worldMatrix[i, j]));
            }
            Debug.Log(rowString);
        }
    }

    private string PrintReadableWorldCell(WorldCellType worldCellType)
    {
        switch (worldCellType)
        {
            case WorldCellType.None:
                return "X";
            case WorldCellType.Path:
                return "-";
            case WorldCellType.Zone:
                return "O";
            case WorldCellType.PlayerInitialPosition:
                return "P";
            default:
                throw new ArgumentOutOfRangeException("worldCellType", worldCellType, null);
        }
    }
}