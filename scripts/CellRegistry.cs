using Godot;
using System;

public enum CellType : int //новые клетки сюда добавлять
{
    Empty = 0,
    Fire = 1,
    Water = 2,
    Grass = 3,
    Soil = 4,
    Acid = 5,
    PoisonedSoil = 6
}

public class CellRegistry
{
    public readonly Cell[] _behaviors = new Cell[256];

    public CellRegistry()
    {
        _behaviors[(int)CellType.Empty] = new EmptyCell();
        _behaviors[(int)CellType.Fire] = new FireCell();
        _behaviors[(int)CellType.Water] = new WaterCell();
        _behaviors[(int)CellType.Grass] = new GrassCell();
        _behaviors[(int)CellType.Soil] = new SoilCell();
        _behaviors[(int)CellType.Acid] = new AcidCell();
        _behaviors[(int)CellType.PoisonedSoil] = new PoisonedSoilCell(); //новые клетки сюда добавлять
    }

    public Cell Get(CellType type) => _behaviors[(int)type] ?? EmptyCell.Instance;
}