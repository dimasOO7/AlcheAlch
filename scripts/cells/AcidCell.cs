using Godot;
using System;

public sealed class AcidCell : Cell
{
    public AcidCell()
    {
        _textureCord = new Vector2I(0,1);
    }
    public override (CellData, int) Execute(CellData self, ReadOnlySpan<CellData> n, Vector2I pos, GridData g)
    {   
        return (self, 0);
    }
}