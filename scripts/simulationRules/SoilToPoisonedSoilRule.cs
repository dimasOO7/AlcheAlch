using Godot;
using System;

public class SoilToPoisonedSoilRule : SimulationRule
{
    private const float acidConvertChance = 0.15f;
    private const int acidConvertScore = -8;

    public override int Execute(GridData grid)
    {
        int delta = 0;
        int width = grid.Width;

        for (int y = 0; y < grid.Height; y++)
        for (int x = 0; x < grid.Width; x++)
        {
            int idx = y * width + x;
            CellData self = grid.Current[idx];
            if (self.Type != CellType.Soil) continue;

            var n = grid.GetNeighbors(x, y);
            byte poisoning = 0;
            for (int i = 0; i < 4; i++)
            {
                if (n[i].Type == CellType.Acid) poisoning += 2;
                if (n[i].Type == CellType.PoisonedSoil) poisoning++;
            }

            if (GD.Randf() <= acidConvertChance * poisoning)
            {
                grid.Next[idx] = self with { Type = CellType.PoisonedSoil };
                if (self.Owner > 0) delta += acidConvertScore;
            }
        }
        return delta;
    }
}