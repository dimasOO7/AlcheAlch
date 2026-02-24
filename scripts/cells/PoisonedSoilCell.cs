using Godot;
using System;

public sealed class PoisonedSoilCell : Cell
{
    public PoisonedSoilCell()
    {
        _textureCord = new Vector2I(1,1);
    }
    public override (CellData, int) Execute(CellData self, ReadOnlySpan<CellData> n, Vector2I pos, GridData g)
    {   
        return (self, 0);
    }
}