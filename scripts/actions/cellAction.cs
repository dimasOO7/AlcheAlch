using Godot;
using System;

public abstract class CellAction
{
    public int Cost { get; protected init; }
    public Vector2I IconCoord { get; protected init; }
    public string Description { get; protected init; } = "";

    protected CellAction(int cost, Vector2I iconCoord, string description = "")
    {
        Cost = cost;
        IconCoord = iconCoord;
        Description = description;
    }

    public virtual bool CanExecute(CellData self, Vector2I pos, GridData grid) => true;

    public abstract (CellData newState, int cost) Execute(CellData self, Vector2I pos, GridData grid);
}