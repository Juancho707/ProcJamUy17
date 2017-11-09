using System;

public static class TupleHelper
{
    
    public static float GetEuclideanDistanceBetweenTuples(Tuple<int, int> location, Tuple<int, int> currentLocation)
    {
        return (float) Math.Sqrt(Math.Pow(location.Item1 - currentLocation.Item1, 2) +
                                 Math.Pow(location.Item2 - currentLocation.Item2, 2));
    }
    
     public static string GetStringRepresentationOfTuple(Tuple<int, int> tuple, string name)
    {
        return string.Format("{0} tuple is [{1}, {2}]", name, tuple.Item1, tuple.Item2);
    }
    
}