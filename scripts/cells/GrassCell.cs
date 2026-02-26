using Godot;
using System;
using System.Collections.Generic;

public sealed class GrassCell : Cell
{
    private const float AcidFlameChance = 0.1f;
    private const int AcidFlameScore = -8;
    private const int OnPlayerFlameScore = -4;
    private const int OnCaptureScore = 4;
    private const int TickScore = 1;

    public GrassCell()
    {
        _textureCord = new Vector2I(2,0);
        cellActions = new CellAction[]{new cellTransform(CellType.Fire,6,new Vector2I(0,0)),
        new cellTransform(CellType.Soil,-4,new Vector2I(3,0),"собрать траву")
        };
    }

    public override (CellData, int) Execute(CellData self, ReadOnlySpan<CellData> n, Vector2I pos, GridData g)
    {
        byte acids = 0;
        byte waters = 0;
        byte fires = 0;
        byte fireOwner = 0;
        for (int i = 0; i < 4;i++)
        {
            if(n[i].Type == CellType.Fire)
            {
                fires++;
                if(fireOwner == 0)
                {
                    fireOwner = n[i].Owner;
                }
            }
            if(n[i].Type == CellType.Water)
            {
                waters++;
            }
            if(n[i].Type == CellType.Acid)
            {
                acids++;
            }
        }

        if (fires > 0 & waters == 0)
        {
            if(self.Owner == 0)
            {
                return (self with {Type = CellType.Fire, Owner = fireOwner}, fireOwner > 0 ? OnCaptureScore : 0);
            }
            else
            {
                return (self with {Type = CellType.Fire}, self.Owner > 0 ? OnPlayerFlameScore : 0);
            }
        }
        if(GD.Randf() <= AcidFlameChance * acids)
        {
            return (self with {Type = CellType.Fire}, self.Owner > 0 ? AcidFlameScore : 0);
        }
        return (self,self.Owner > 0 ? TickScore : 0);
    }
}