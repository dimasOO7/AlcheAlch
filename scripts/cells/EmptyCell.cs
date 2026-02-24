using Godot;
using System;

public sealed class EmptyCell : Cell
{
    public static readonly EmptyCell Instance = new();

    public EmptyCell()
    {
        _textureCord = new Vector2I(2,1);
    }
    public override (CellData, int) Execute(CellData self, ReadOnlySpan<CellData> n, Vector2I pos, GridData g)
    {
        return (self, 0);
    }
}