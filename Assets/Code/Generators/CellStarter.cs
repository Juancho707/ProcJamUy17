using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellStarter : MonoBehaviour
{
    public GameObject RightBorder;
    public GameObject LeftBorder;

    private CellStartData nextCellData;

    public void SpawnCell(CellStartData data)
    {
        nextCellData = new CellStartData
        {
            Settings = data.Settings,
            CellPrefab = data.CellPrefab,
            Repo = data.Repo,
            X = data.X,
            Y = data.Y,
            IsMainPath = data.IsMainPath
        };

        CalculateAndPlaceRoom(data);
        BuildBorder(data);
        CreateNewCell();
    }

    void CalculateAndPlaceRoom(CellStartData data)
    {
        if (!PlaceEndRoom(data))
        {
            if (data.FromDir == Direction.Left || data.FromDir == Direction.Right)
            {
                data.NextDir = CalculateNextDir(data);

                if (data.NextDir == Direction.Left || data.NextDir == Direction.Right)
                {
                    var rmPf = data.Repo.Where(c => c.GetComponent<RoomBase>().Data.SideEdges.Contains(data.FromEdge) &&
                                                    c.GetComponent<RoomBase>().Data.RoomClass == RoomType.Horizontal)
                        .PickOne();
                    var room = PlaceRoom(rmPf, data.FromDir, data.FromEdge);

                    if (data.NextDir == Direction.Left)
                    {
                        nextCellData.X = data.X - 1;
                        nextCellData.FromEdge = room.Data.RealLeftEdge;
                    }

                    if (data.NextDir == Direction.Right)
                    {
                        nextCellData.X = data.X + 1;
                        nextCellData.FromEdge = room.Data.RealRightEdge;
                    }

                    nextCellData.FromDir = data.NextDir;
                }
                else if (data.NextDir == Direction.Up)
                {
                    var rmPf = data.Repo.Where(c => c.GetComponent<RoomBase>().Data.SideEdges.Contains(data.FromEdge) &&
                                                    c.GetComponent<RoomBase>().Data.TopEdge != EdgeType.Z).PickOne();
                    var room = PlaceRoom(rmPf, data.FromDir, data.FromEdge);

                    nextCellData.Y = data.Y + 1;
                    nextCellData.FromDir = data.NextDir;
                    nextCellData.FromEdge = room.Data.RealTopEdge;
                }
                else if (data.NextDir == Direction.Down)
                {
                    var rmPf = data.Repo.Where(c => c.GetComponent<RoomBase>().Data.SideEdges.Contains(data.FromEdge) &&
                                                    c.GetComponent<RoomBase>().Data.BottomEdge != EdgeType.Z).PickOne();
                    var room = PlaceRoom(rmPf, data.FromDir, data.FromEdge);

                    nextCellData.Y = data.Y - 1;
                    nextCellData.FromDir = data.NextDir;
                    nextCellData.FromEdge = room.Data.RealBottomEdge;
                }
            }
            else if (data.FromDir == Direction.Up)
            {
                data.NextDir = data.Y % 2 == 0 ? Direction.Right : Direction.Left;
                var rmPf = data.Repo.Where(c => c.GetComponent<RoomBase>().Data.BottomEdge != EdgeType.Z).PickOne();
                var room = PlaceRoom(rmPf, data.FromDir, data.FromEdge);

                if (data.NextDir == Direction.Left)
                {
                    nextCellData.X = data.X - 1;
                    nextCellData.FromEdge = room.Data.RealLeftEdge;
                }

                if (data.NextDir == Direction.Right)
                {
                    nextCellData.X = data.X + 1;
                    nextCellData.FromEdge = room.Data.RealRightEdge;
                }

                nextCellData.FromDir = data.NextDir;
            }
            else if (data.FromDir == Direction.Down)
            {
                data.NextDir = data.Y % 2 == 0 ? Direction.Right : Direction.Left;
                var rmPf = data.Repo.Where(c => c.GetComponent<RoomBase>().Data.TopEdge != EdgeType.Z).PickOne();
                var room = PlaceRoom(rmPf, data.FromDir, data.FromEdge);

                if (data.NextDir == Direction.Left)
                {
                    nextCellData.X = data.X - 1;
                    nextCellData.FromEdge = room.Data.RealLeftEdge;
                }

                if (data.NextDir == Direction.Right)
                {
                    nextCellData.X = data.X + 1;
                    nextCellData.FromEdge = room.Data.RealRightEdge;
                }

                nextCellData.FromDir = data.NextDir;
            }
        }
    }

    RoomBase PlaceRoom(GameObject cell, Direction movement, EdgeType edge)
    {
        var room = Instantiate(cell, this.transform).GetComponent<RoomBase>();

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

    Direction CalculateNextDir(CellStartData data)
    {
        var defaultDir = data.Y % 2 == 0 ? Direction.Right : Direction.Left;

        if (data.Y == data.Settings.LevelHeight - 1)
        {
            return defaultDir;
        }

        if (defaultDir == Direction.Left && data.X == 0)
        {
            return Direction.Up;
        }

        if (defaultDir == Direction.Right && data.X == data.Settings.LevelWidth - 1)
        {
            return Direction.Up;
        }

        if (MathHelper.DiceRoll(data.Settings.VerticalChance))
        {
            return Direction.Up;
        }

        return defaultDir;
    }

    void CreateNewCell()
    {
        if (nextCellData != null)
        {
            if (nextCellData.X >= 0 && nextCellData.Y >= 0 && nextCellData.X < nextCellData.Settings.LevelWidth &&
                nextCellData.Y < nextCellData.Settings.LevelHeight)
            {
                var pos = new Vector2(nextCellData.X * nextCellData.Settings.RoomWidth,
                    nextCellData.Y * nextCellData.Settings.RoomHeight);
                var cell = Instantiate(nextCellData.CellPrefab, pos, Quaternion.identity).GetComponent<CellStarter>();
                cell.SpawnCell(nextCellData);
            }
        }
    }

    bool PlaceEndRoom(CellStartData data)
    {
        if (data.Y == data.Settings.LevelHeight - 1)
        {
            if ((data.FromDir == Direction.Left && data.X == 0) || (data.FromDir == Direction.Right && data.X == data.Settings.LevelWidth - 1))
            {
                if (CellStartData.Level == 3)
                {
                    var rmPf = data.Repo.Where(c => c.GetComponent<RoomBase>().Data.SideEdges.Contains(data.FromEdge) &&
                                                    c.GetComponent<RoomBase>().Data.RoomClass == RoomType.BossChamber)
                        .PickOne();
                    PlaceRoom(rmPf, data.FromDir, data.FromEdge);
                    nextCellData = null;
                    return true;
                }
            }
        }

        return false;
    }

    void BuildBorder(CellStartData data)
    {
        if (data.X == 0)
        {
            Instantiate(LeftBorder, this.transform);
        }

        if (data.X == data.Settings.LevelWidth - 1)
        {
            Instantiate(RightBorder, this.transform);
        }
    }
}
