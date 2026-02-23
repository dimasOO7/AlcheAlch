using Godot;
using System;

public class GridData
{
    public readonly int Width, Height;
    public CellData[] Current;
    public CellData[] Next;

    public GridData(int width, int height)
    {
        Width = width;
        Height = height;
        Current = new CellData[width * height];
        Next = new CellData[width * height];
    }

    public CellData this[int x, int y]
    {
        get => Current[y * Width + x];
        set => Current[y * Width + x] = value;
    }
}