using System;

public class Tuple<T1, T2>
{
    public Tuple(T1 item1, T2 item2)
    {
        this.Item1 = item1;
        this.Item2 = item2;
    }

    public T1 Item1 { get; set; }

    public T2 Item2 { get; set; }
    
    public override bool Equals(Object obj) 
    {
        // Check for null values and compare run-time types.
        if (obj == null || GetType() != obj.GetType()) 
            return false;

        Tuple<T1, T2> tuple = (Tuple<T1, T2>)obj;
        return Item1.Equals(tuple.Item1) && Item2.Equals(tuple.Item2);
    }
}

public class Tuple<T1, T2, T3> : Tuple<T1, T2>
{
    public Tuple(T1 item1, T2 item2, T3 item3) : base(item1, item2)
    {
        this.Item3 = item3;
    }

    public T3 Item3 { get; set; }
}

public static class Tuple
{
    public static Tuple<T1, T2> Create<T1, T2>(
        T1 item1, T2 item2)
    {
        return new Tuple<T1, T2>(item1, item2);
    }

    public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
    {
        return new Tuple<T1, T2, T3>(item1, item2, item3);
    }
    
    // TODO maybe override for triple tuple.
}