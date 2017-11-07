using System;

[Serializable]
public class RoomData
{
    public RoomType RoomClass;
    public EdgeType RightEdge;
    public EdgeType LeftEdge;
    public EdgeType TopEdge;
    public EdgeType BottomEdge;
    public bool IsFlipped;

    public EdgeType[] SideEdges { get { return new[] {RightEdge, LeftEdge}; } }

    public EdgeType RealRightEdge { get { return IsFlipped ? LeftEdge : RightEdge; } }
    public EdgeType RealLeftEdge { get { return IsFlipped ? RightEdge : LeftEdge; } }

    public EdgeType RealTopEdge
    {
        get
        {
            if (!IsFlipped)
            {
                return TopEdge;
            }

            if (TopEdge == EdgeType.A)
            {
                return EdgeType.B;
            }

            return EdgeType.A;
        }
    }

    public EdgeType RealBottomEdge
    {
        get
        {
            if (!IsFlipped)
            {
                return BottomEdge;
            }

            if (BottomEdge == EdgeType.A)
            {
                return EdgeType.B;
            }

            return EdgeType.A;
        }
    }
}
