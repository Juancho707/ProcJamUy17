using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int Level;
    public LevelGenSettings Settings;
    public Transform CellArray;

    private RoomBase[,] matrix;
    private GameObject[] roomRepo;

    void Start()
    {
        matrix = new RoomBase[Settings.LevelWidth, Settings.LevelHeight];
        roomRepo = Resources.LoadAll<GameObject>("Rooms");
    }

    void GenerateSolutionPath()
    {
        var fromDir = Direction.Right;
        var nextDir = Direction.Right;
        int x = 0;
        int y = 0;
        var room = PlaceCell(roomRepo.PickOne());
        var edge = room.Data.RightEdge;
        GameObject cell;

        x++;

        while (y < Settings.LevelHeight)
        {
            if (fromDir == Direction.Left || fromDir == Direction.Right)
            {
                nextDir = CalculateNextDir(x, y);

                if (nextDir == Direction.Left || nextDir == Direction.Right)
                {
                    cell = roomRepo.Where(c => c.GetComponent<RoomBase>().Data.SideEdges.Contains(edge) &&
                                               c.GetComponent<RoomBase>().Data.RoomClass == RoomType.Horizontal)
                        .PickOne();
                    room = PlaceCell(cell, new Vector2(x * Settings.RoomWidth, y * Settings.RoomHeight), nextDir, edge);
                    matrix[x, y] = room;

                    if (nextDir == Direction.Left)
                    {
                        edge = room.Data.LeftEdge;
                        x--;
                    }
                    else
                    {
                        edge = room.Data.RightEdge;
                        x++;
                    }
                }
                else if (nextDir == Direction.Up)
                {
                    cell = roomRepo.Where(c => c.GetComponent<RoomBase>().Data.SideEdges.Contains(edge) &&
                                               c.GetComponent<RoomBase>().Data.TopEdge != EdgeType.Z).PickOne();
                    room = PlaceCell(cell, new Vector2(x * Settings.RoomWidth, y * Settings.RoomHeight), nextDir, edge);
                    matrix[x, y] = room;

                    edge = room.Data.TopEdge;
                    y++;
                }
                else if (nextDir == Direction.Down)
                {
                    cell = roomRepo.Where(c => c.GetComponent<RoomBase>().Data.SideEdges.Contains(edge) &&
                                               c.GetComponent<RoomBase>().Data.BottomEdge != EdgeType.Z).PickOne();
                    room = PlaceCell(cell, new Vector2(x * Settings.RoomWidth, y * Settings.RoomHeight), nextDir, edge);
                    matrix[x, y] = room;

                    edge = room.Data.BottomEdge;
                    y--;
                }
            }
            else if (fromDir == Direction.Up)
            {
                cell = roomRepo.Where(c => c.GetComponent<RoomBase>().Data.BottomEdge != EdgeType.Z).PickOne();
                room = PlaceCell(cell, new Vector2(x * Settings.RoomWidth, y * Settings.RoomHeight), nextDir, edge);
                matrix[x, y] = room;

                nextDir = y % 2 == 0 ? Direction.Right : Direction.Left;
                if (nextDir == Direction.Left)
                {
                    edge = room.Data.LeftEdge;
                    x--;
                }
                else
                {
                    edge = room.Data.RightEdge;
                    x++;
                }
            }
            else if (fromDir == Direction.Down)
            {
                cell = roomRepo.Where(c => c.GetComponent<RoomBase>().Data.TopEdge != EdgeType.Z).PickOne();
                room = PlaceCell(cell, new Vector2(x * Settings.RoomWidth, y * Settings.RoomHeight), nextDir, edge);
                matrix[x, y] = room;

                nextDir = y % 2 == 0 ? Direction.Right : Direction.Left;
                if (nextDir == Direction.Left)
                {
                    edge = room.Data.LeftEdge;
                    x--;
                }
                else
                {
                    edge = room.Data.RightEdge;
                    x++;
                }
            }

            fromDir = nextDir;
        }
    }

    RoomBase PlaceCell(GameObject cell)
    {
        var room = Instantiate(cell, Vector3.zero, Quaternion.identity).GetComponent<RoomBase>();
        room.transform.SetParent(CellArray);

        return room;
    }

    RoomBase PlaceCell(GameObject cell, Vector2 position, Direction movement, EdgeType edge)
    {
        var room = Instantiate(cell, position, Quaternion.identity).GetComponent<RoomBase>();
        room.transform.SetParent(CellArray);

        switch (movement)
        {
            case Direction.Left:
                if (!LevelGenHelper.AreEdgesCompatible(edge, room.Data.RightEdge))
                {
                    room.Flip();
                }
                break;
            case Direction.Right:
                if (!LevelGenHelper.AreEdgesCompatible(edge, room.Data.LeftEdge))
                {
                    room.Flip();
                }
                break;
            case Direction.Up:
                if (edge != room.Data.BottomEdge)
                {
                    room.Flip();
                }
                break;
            case Direction.Down:
                if (edge != room.Data.TopEdge)
                {
                    room.Flip();
                }
                break;
        }

        return room;
    }

    Direction CalculateNextDir(int x, int y)
    {
        var defaultDir = y % 2 == 0 ? Direction.Right : Direction.Left;

        if (y == Settings.LevelHeight - 1)
        {
            return defaultDir;
        }

        if (defaultDir == Direction.Left && x == 0)
        {
            return Direction.Up;
        }

        if (defaultDir == Direction.Right && x == Settings.LevelWidth - 1)
        {
            return Direction.Up;
        }

        if (MathHelper.DiceRoll(Settings.VerticalChance))
        {
            return Direction.Up;
        }

        return defaultDir;
    }
}
