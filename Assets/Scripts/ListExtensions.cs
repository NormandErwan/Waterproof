using System.Collections.Generic;

public static class ListExtensions
{
    public static int GetNextIndex<T>(this List<T> list, int index)
    {
        return (index + 1) % list.Count;
    }
}
