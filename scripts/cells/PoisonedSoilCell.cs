using Godot;
using System;

public sealed class PoisonedSoilCell : Cell
{
    public PoisonedSoilCell()
    {
        _textureCord = new Vector2I(1,1);
        cellActions = new CellAction[]{new cellTransform(CellType.Soil,24,new Vector2I(1,0))};
    }
    public override (CellData, int) Execute(CellData self, ReadOnlySpan<CellData> n, Vector2I pos, GridData g)
    {   
        return (self, 0);
    }
}