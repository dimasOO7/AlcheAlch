using Godot;
using System;

public abstract partial class SimulationRule
{
    /// <summary>
    /// применяет правило к полю
    /// </summary>
    /// <param name="grid"></param>
    /// <returns>очки</returns>
    public abstract int Execute(GridData grid); 
}
