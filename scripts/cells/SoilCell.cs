using Godot;
using System;
public sealed class SoilCell : Cell
{
    private const float acidConvertChance = 0.15f;
    private const float grassGrowChance = 0.33f;
    private const int acidConvertScore = -8;
    private const int grassGrowScore = 4;


    public SoilCell()
    {
        _textureCord = new Vector2I(3,0);
    }

    public override (CellData, int) Execute(CellData self, ReadOnlySpan<CellData> n, Vector2I pos, GridData g)
    {
        byte poisoning = 0;
        byte waters = 0;
        for (int i = 0; i < 4;i++)
        {
            if(n[i].Type == CellType.Acid)
            {
                poisoning+=2;
            }
            if(n[i].Type == CellType.PoisonedSoil)
            {
                poisoning++;
            }
            if(n[i].Type == CellType.Water)
            {
                waters++;
            }
        }
        if(GD.Randf() <= acidConvertChance * poisoning)
        {
            return (self with {Type = CellType.PoisonedSoil}, self.Owner > 0 ? acidConvertScore : 0);
        }
        if(GD.Randf() <= grassGrowChance * waters)
        {
            return (self with {Type = CellType.Grass}, self.Owner > 0 ? grassGrowScore : 0);
        }
        return (self,0);
    }
}