using Godot;
using System;

public class FireDecayRule : SimulationRule
{
    public override int Execute(GridData grid)
    {
        int delta = 0;
        for (int i = 0; i < grid.Current.Length; i++)
        {
            if (grid.Current[i].Type == CellType.Fire)
            {
                grid.Next[i] = grid.Current[i] with { Type = CellType.Soil };
            }
        }
        return delta;
    }
}