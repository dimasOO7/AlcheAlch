using Godot;
using System;

public class cellTransform : CellAction
{
    public CellType TargetType { get; }

    public cellTransform(CellType targetType, int cost, Vector2I iconCoord, string description = "") : base(cost,iconCoord,description)
    {
        TargetType = targetType;
    }

    public override (CellData, int) Execute(CellData self, Vector2I pos, GridData grid)
    {
        if(CanExecute(self,pos,grid))
        {
            return (self with {Type = TargetType}, Cost);
        }
        return (self,0);
    }
}