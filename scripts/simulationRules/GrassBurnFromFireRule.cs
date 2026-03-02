using Godot;
using System;

public class GrassBurnFromFireRule : SimulationRule
{
    private const int OnCaptureScore = 4;
    private const int OnPlayerFlameScore = -4;

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

            var n = grid.GetNeighbors(x, y);
            byte fires = 0;
            byte waters = 0;
            byte fireOwner = 0;

            for (int i = 0; i < 4; i++)
            {
                if (n[i].Type == CellType.Fire)
                {
                    fires++;
                    if (fireOwner == 0) fireOwner = n[i].Owner;
                }
                if (n[i].Type == CellType.Water) waters++;
            }

            if (fires > 0 && waters == 0)
            {
                CellData newState;
                int scoreThis = 0;

                if (self.Owner == 0)
                {
                    newState = self with { Type = CellType.Fire, Owner = fireOwner };
                    scoreThis = fireOwner > 0 ? OnCaptureScore : 0;
                }
                else
                {
                    newState = self with { Type = CellType.Fire };
                    scoreThis = self.Owner > 0 ? OnPlayerFlameScore : 0;
                }

                grid.Next[idx] = newState;
                delta += scoreThis;
            }
        }
        return delta;
    }
}