using Godot;
using System;

public sealed class WaterCell : Cell
{
    private const float acidConvertChance = 0.25f;
    private const int acidConvertScore = -8;

    public WaterCell()
    {
        _textureCord = new Vector2I(1,0);
    }

    public override (CellData, int) Execute(CellData self, ReadOnlySpan<CellData> n, Vector2I pos, GridData g)
    {
        for (int i = 0; i < 8;i++)
        {
            if(n[i].Type == CellTypes.Acid && GD.Randf() <= acidConvertChance)
            {
                return (self with {Type = CellTypes.Acid},acidConvertScore);
            }
        }
        return (self,0);
    }
}