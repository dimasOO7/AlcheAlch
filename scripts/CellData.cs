using Godot;
using System;

/// <summary>
/// данные клетки
/// </summary>
/// <param name="Type">тип</param>
/// <param name="Owner">владелец (0 никто, > 0 игрок (на случай нескольких игроков, пока задавать 1))</param>
/// <param name="state">прост на будущее переменная для доп условий</param>
public record struct CellData(CellType Type, byte Owner, byte state);