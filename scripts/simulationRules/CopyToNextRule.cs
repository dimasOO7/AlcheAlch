using Godot;
using System;

public class CopyToNextRule : SimulationRule
{
    public override int Execute(GridData grid)
    {
        Array.Copy(grid.Current, grid.Next, grid.Current.Length);
        return 0;
    }
}