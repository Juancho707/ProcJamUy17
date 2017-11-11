using System;
using System.Collections.Generic;

public static class WorldGeneratorHelper
{
    public static List<string> GetWorldStringRepresentation(WorldCellType[,] worldMatrix)
    {
        List<string> worldStringRepresentation = new List<string>();
        int worldWidthAndHeight = worldMatrix.GetLength(0);
        for (int i = 0; i < worldWidthAndHeight; i++)
        {
            var rowString = "";
            for (int j = 0; j < worldWidthAndHeight; j++)
            {
                rowString += string.Format("{0} ", PrintReadableWorldCell(worldMatrix[i, j]));
            }
            worldStringRepresentation.Add(rowString);
        }
        return worldStringRepresentation;
    }

    public static string PrintReadableWorldCell(WorldCellType worldCellType)
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

    /**
    * Find the nearest location to a location passed by parameter, from within a list of locations.
    * Returns a tuple, or null if it doesn't find any near location.
    */
    public static Tuple<int, int> FindNearestLocationInLocations(Tuple<int, int> location,
        List<Tuple<int, int>> locations)
    {
        Tuple<int, int> nearestLocation = null;
        float previousMinimumDistance = int.MaxValue;

        foreach (var currentLocation in locations)
        {
            if (nearestLocation == null) nearestLocation = currentLocation;
            else
            {
                float distanceBetweenLocations =
                    TupleHelper.GetEuclideanDistanceBetweenTuples(location, currentLocation);
                if (distanceBetweenLocations < previousMinimumDistance)
                {
                    nearestLocation = currentLocation;
                    previousMinimumDistance = distanceBetweenLocations;
                }
            }
        }
        return nearestLocation;
    }

    public static bool IsPositionAboveAnother(Tuple<int, int> first, Tuple<int, int> second)
    {
        return first.Item1 < second.Item1;
    }

    public static bool IsPositionOnRightSideOfAnother(Tuple<int, int> first, Tuple<int, int> second)
    {
        return first.Item2 > second.Item2;
    }

    public static int PathNeighborCellsForPosition(Tuple<int, int> newPosition, WorldCellType[,] worldMatrix)
    {
        int pathNeighborCellsForPosition = 0;
        for (int i = -1; i <= 1; i += 2)
        {
            if (IsInSquareMatrixRange(newPosition.Item1 + i, worldMatrix) &&
                worldMatrix[newPosition.Item1 + i, newPosition.Item2] == WorldCellType.Path)
                pathNeighborCellsForPosition++;
            if (IsInSquareMatrixRange(newPosition.Item2 + i, worldMatrix) &&
                worldMatrix[newPosition.Item1, newPosition.Item2 + i] == WorldCellType.Path)
                pathNeighborCellsForPosition++;
        }
        return pathNeighborCellsForPosition;
    }

    public static bool IsInSquareMatrixRange(int index, WorldCellType[,] matrix)
    {
        return index >= 0 && index < matrix.GetLength(0);
    }

    public static bool AllZonesAreReachedByPaths(WorldCellType[,] localWorldMatrix)
    {
    // TODO implement this.
        throw new NotImplementedException();
    }
}