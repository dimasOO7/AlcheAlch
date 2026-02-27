using Godot;
using System;

/// <summary>
/// игровое поле
/// </summary>
public class GridData
{
    public readonly int Width, Height;
    public CellData[] Current; // дядя грок сказал так быстрее работать будет
    public CellData[] Next;

    public GridData(int width, int height)
    {
        Width = width;
        Height = height;
        Current = new CellData[width * height];
        Next = new CellData[width * height];
    }

    public CellData this[int x, int y] // дядя грок сказал так быстрее работать будет
    {
        get => Current[y * Width + x];
        set => Current[y * Width + x] = value;
    }

    /// <summary>
    /// получить соседей, первые 4 по сторонам, вторые 4 по диагонали
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>соседи, первые 4 по сторонам, вторые 4 по диагонали</returns>
    public CellData[] GetNeighbors(int x, int y)
    {
        int[] dx = {1,0,-1,0,1,-1,1,-1};
        int[] dy = {0,1,0,-1,1,1,-1,-1};


        CellData[] neighbors = new CellData[8];
        for(int i = 0;i<8;i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            if (nx < 0 || nx >= Width || ny < 0 || ny >= Height)
                neighbors[i] = new CellData { Type = CellType.Empty };
            else
                neighbors[i] = Current[ny * Width + nx];
        }

        return neighbors;
    }

    public CellData[] GetNeighbors(Vector2I pos)
    {
        return GetNeighbors(pos.X,pos.Y);
    }
}