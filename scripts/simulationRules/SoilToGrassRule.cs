using Godot;
using System;

public class SoilToGrassRule : SimulationRule
{
    private const float grassGrowChance = 0.33f;
    private const int grassGrowScore = 4;

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

            // Пропускаем, если уже отравилась в предыдущем правиле
            if (grid.Next[idx].Type != CellType.Soil) continue;

            var n = grid.GetNeighbors(x, y);
            byte waters = 0;
            for (int i = 0; i < 4; i++)
                if (n[i].Type == CellType.Water) waters++;

            if (GD.Randf() <= grassGrowChance * waters)
            {
                grid.Next[idx] = self with { Type = CellType.Grass };
                if (self.Owner > 0) delta += grassGrowScore;
            }
        }
        return delta;
    }
}