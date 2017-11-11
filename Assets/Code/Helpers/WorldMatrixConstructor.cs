using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldMatrixConstructor : MonoBehaviour
{
    public WorldCellType[,] WorldMatrix;
    private List<Tuple<int, int>> zoneLocations;
    private Tuple<int, int> playerInitialPosition;
    private readonly object zoneLocationsLock = new object();
    private int zonesReachedByPaths;

    public void InitializeGeneration(int zoneAmount, int maxTilesPerZone) //10, 10
    {
        // Find out how many tiles there are according to how many tiles there should be per zone
        // (the zone itself and border tiles to separate it from other zones). Then, find the square root of 
        // that (always thinking about generating a square matrix/map) and approximate that number to the nearest
        // upper integer. Then, construct a square matrix with that number.
        // This is to not use a zoneAmount ** 2 matrix, but optimize it a bit.
        int worldWidthAndHeight = (int) Math.Ceiling(Math.Sqrt(zoneAmount * maxTilesPerZone));
        WorldMatrix = new WorldCellType[worldWidthAndHeight, worldWidthAndHeight];
        // Initialize world.
        // Generate available positions
        List<Tuple<int, int>> availablePositions = new List<Tuple<int, int>>();
        for (int i = 0; i < worldWidthAndHeight; i++)
        {
            for (int j = 0; j < worldWidthAndHeight; j++)
            {
                WorldMatrix[i, j] = 0;
                availablePositions.Add(Tuple.Create(i, j));
            }
        }
        // Assign initial position for player (chooses randomly between four corners of the map).
        List<Tuple<int, int>> possibleInitialPositionsForPlayer = availablePositions.FindAll(t =>
            (t.Item1 == 0 || t.Item1 == worldWidthAndHeight - 1) &&
            (t.Item2 == 0 || t.Item2 == worldWidthAndHeight - 1));
        playerInitialPosition = possibleInitialPositionsForPlayer.PickOne();
        availablePositions.Remove(playerInitialPosition);
        WorldMatrix[playerInitialPosition.Item1, playerInitialPosition.Item2] = WorldCellType.PlayerInitialPosition;
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
                    // Attempt to find it among availablePositions, since one of them could be out of the matrix' bounds.
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
            WorldMatrix[zoneLocation.Item1, zoneLocation.Item2] = WorldCellType.Zone;
        }
        zonesReachedByPaths = 0;
        // Generate initial path from player to its nearest location.
        Tuple<int, int> nearestLocationToPlayer =
            WorldGeneratorHelper.FindNearestLocationInLocations(playerInitialPosition, zoneLocations);
        GeneratePathBetweenLocations(playerInitialPosition, nearestLocationToPlayer);
        zonesReachedByPaths++;
        // From this nearest location onwards, auto-generate paths.
        zoneLocations.Remove(nearestLocationToPlayer);
        var currentZone = nearestLocationToPlayer; // alias
        var zoneAmountToReach = 1;
        StartCoroutine(GeneratePathsRecursively(currentZone, zoneLocations, zoneAmountToReach, zoneAmount));
    }

    IEnumerator GeneratePathsRecursively(Tuple<int, int> currentZone, List<Tuple<int, int>> availableZones,
        int zoneAmountToReach, int zoneAmount)
    {
        if (availableZones.Count > 0)
        {
            List<Tuple<int, int>> nearestZones = new List<Tuple<int, int>>();
            for (int i = 0; i < zoneAmountToReach; i++)
            {
                Tuple<int, int> nearestZone;
                lock (zoneLocationsLock)
                {
                    nearestZone = WorldGeneratorHelper.FindNearestLocationInLocations(currentZone, availableZones);
                    if (nearestZone != null) availableZones.Remove(nearestZone);
                }
                if (nearestZone == null) break;
                nearestZones.Add(nearestZone);
            }
            if (nearestZones.Count > 0)
            {
                foreach (var nearestZone in nearestZones)
                {
                    GeneratePathBetweenLocations(currentZone, nearestZone);
                    zonesReachedByPaths++;
                    if (zonesReachedByPaths == zoneAmount)
                    {
                        EventManager.TriggerEvent(Events.worldGenerationEnd);
                    }
                    else
                    {
                        yield return StartCoroutine(
                            GeneratePathsRecursively(currentZone, availableZones, zoneAmountToReach, zoneAmount));
                    }
                }
            }
        }
    }

    private void GeneratePathBetweenLocations(Tuple<int, int> start, Tuple<int, int> end)
    {
        Tuple<int, int> startCopy = Tuple.Create(start.Item1, start.Item2);
        while (!startCopy.Equals(end))
        {
            var m = startCopy.Item1;
            var n = startCopy.Item2;
            if (WorldMatrix[m, n] == WorldCellType.None) WorldMatrix[m, n] = WorldCellType.Path;
            MoveLocationTowardsDestination(startCopy, end);
        }
    }

    private void MoveLocationTowardsDestination(Tuple<int, int> origin, Tuple<int, int> destination)
    {
        // TODO maybe determine if a horizontal/vertical movement is attempted according to some random variable set at the start of the script.
        if (origin.Item1 != destination.Item1)
        {
            if (origin.Item1 > destination.Item1) origin.Item1--;
            else origin.Item1++;
        }
        else if (origin.Item2 != destination.Item2)
        {
            if (origin.Item2 > destination.Item2) origin.Item2--;
            else origin.Item2++;
        }
    }
}