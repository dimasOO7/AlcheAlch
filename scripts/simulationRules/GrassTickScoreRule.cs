using Godot;
using System;

public class GrassTickScoreRule : SimulationRule
{
    private const int TickScore = 1;

    public override int Execute(GridData grid)
    {
        int delta = 0;
        int width = grid.Width;

        for (int y = 0; y < grid.Height; y++)
        for (int x = 0; x < grid.Width; x++)
        {
            int idx = y * width + x;
            // Начисляем только если трава осталась травой после всех превращений
            if (grid.Next[idx].Type == CellType.Grass && grid.Current[idx].Owner > 0)
            {
                delta += TickScore;
            }
        }
        return delta;
    }
}