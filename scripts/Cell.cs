using Godot;
using System;

public abstract class Cell
{
    private Vector2I _textureCord = Vector2I.Zero;


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