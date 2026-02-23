using Godot;
using System;

public enum CellType : int
{
    Empty = 0,
    Fire = 1,
    Water = 2,
    Grass = 3,
    Soil = 4,
    Acid = 5
}

public class CellRegistry
{
    public readonly Cell[] _behaviors = new Cell[256];

    public CellRegistry()
    {
        _behaviors[(int)CellType.Empty] = new EmptyCell();
    }

    public Cell Get(CellType type) => _behaviors[(int)type] ?? EmptyCell.Instance;
}