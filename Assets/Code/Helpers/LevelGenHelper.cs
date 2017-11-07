using System.Linq;

public static class LevelGenHelper
{
    public static EdgeType[] GetCompatibleEdges(EdgeType edge)
    {
        switch (edge)
        {
            case EdgeType.A:
                return new[] {EdgeType.A, EdgeType.B};
            case EdgeType.B:
                return new[] { EdgeType.A, EdgeType.B, EdgeType.C };
            case EdgeType.C:
                return new[] { EdgeType.C, EdgeType.B, EdgeType.D };
            case EdgeType.D:
                return new[] { EdgeType.C, EdgeType.D };
        }

        return new[] {EdgeType.A};
    }

    public static bool AreEdgesCompatible(EdgeType edge1, EdgeType edge2)
    {
        return GetCompatibleEdges(edge1).Contains(edge2);
    }
}