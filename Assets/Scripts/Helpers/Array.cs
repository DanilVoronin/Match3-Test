using System;

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
        if(target is null) throw new ArgumentNullException(nameof(array));

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
}