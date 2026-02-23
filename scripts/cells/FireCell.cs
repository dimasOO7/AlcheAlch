using Godot;
using System;

public sealed class FireCell : Cell
{

    public override (CellData, int) Execute(CellData self, ReadOnlySpan<CellData> n, Vector2I pos, GridData g)
    {   
        return (self with {Type = CellTypes.Soil}, 0);
    }
}
