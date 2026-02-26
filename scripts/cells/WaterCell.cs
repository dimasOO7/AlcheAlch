using Godot;
using System;

public sealed class WaterCell : Cell
{
    private const float acidConvertChance = 0.25f;
    private const int acidConvertScore = -8;

    public WaterCell()
    {
        _textureCord = new Vector2I(1,0);
        cellActions = new CellAction[]{new cellTransform(CellType.Soil,4,new Vector2I(3,0))};
    }

    public override (CellData, int) Execute(CellData self, ReadOnlySpan<CellData> n, Vector2I pos, GridData g)
    {
        byte poisoning = 0;
        for (int i = 0; i < 4;i++)
        {
            if(n[i].Type == CellType.Acid)
            {
                poisoning++;
            }
        }
        if(GD.Randf() <= acidConvertChance * poisoning)
        {
            return (self with {Type = CellType.Acid},self.Owner > 0 ? acidConvertScore : 0);
        }
        return (self,0);
    }
}