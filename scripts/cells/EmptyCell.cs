using Godot;
using System;

public sealed class EmptyCell : Cell
{
    public static readonly EmptyCell Instance = new();

    public override (CellData, int) Execute(CellData self, ReadOnlySpan<CellData> n, Vector2I pos, GridData g)
    {
        return (self, 0);
    }
}