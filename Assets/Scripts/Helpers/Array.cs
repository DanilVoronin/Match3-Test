using System;
using System.Collections.Generic;
using UnityEngine;

public struct Index2D
{
    public int x;
    public int y;

    public Index2D(int x, int y) 
    { 
        this.x = x;
        this.y = y; 
    }
}

public static class Array
{
    public static bool ContainsElement<T>(this T[,] array, T target)
    {
        if (array is null) throw new ArgumentNullException(nameof(array));
        if (target is null) throw new ArgumentNullException(nameof(array));

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[i, j].Equals(target))
                    return true;
            }
        }
        return false;
    }
    public static Index2D? GetPositionElement<T>(this T[,] array, T target, out Index2D? index)
    {
        if (array is null) throw new ArgumentNullException(nameof(array));
        if (target is null) throw new ArgumentNullException(nameof(array));

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[i, j].Equals(target))
                {
                    index = new Index2D(i, j);
                    return index;
                }
            }
        }

        index = null;
        return index;
    }

    public static bool AreNeighbors<T>(this T[,] array, T item1, T item2)
    {
        array.GetPositionElement(item1, out var index1);
        array.GetPositionElement(item2, out var index2);

        int dx = Mathf.Abs(index1.Value.x - index2.Value.x);
        int dy = Mathf.Abs(index1.Value.y - index2.Value.y);
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }
}