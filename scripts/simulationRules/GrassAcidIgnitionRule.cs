using Godot;
using System;

public class GrassAcidIgnitionRule : SimulationRule
{
    private const float AcidFlameChance = 0.1f;
    private const int AcidFlameScore = -8;

    public override int Execute(GridData grid)
    {
        int delta = 0;
        int width = grid.Width;

        for (int y = 0; y < grid.Height; y++)
        for (int x = 0; x < grid.Width; x++)
        {
            int idx = y * width + x;
            CellData self = grid.Current[idx];
            if (self.Type != CellType.Grass) continue;

            // Пропускаем, если уже сгорела от огня
            if (grid.Next[idx].Type != CellType.Grass) continue;

            var n = grid.GetNeighbors(x, y);
            byte acids = 0;
            for (int i = 0; i < 4; i++)
                if (n[i].Type == CellType.Acid) acids++;

            if (GD.Randf() <= AcidFlameChance * acids)
            {
                grid.Next[idx] = self with { Type = CellType.Fire };
                if (self.Owner > 0) delta += AcidFlameScore;
            }
        }
        return delta;
    }
}