using Godot;
using System;

public abstract class Cell
{
    protected Vector2I _textureCord = new Vector2I(0,0);
    public abstract (CellData newState, int scoreDelta) Execute(CellData self,
        ReadOnlySpan<CellData> neighbors,
        Vector2I position,
        GridData grid
    );

    public virtual Vector2I GetTextureCord(CellData data)
    {
        return _textureCord;
    }
}