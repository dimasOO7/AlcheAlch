using Godot;
using System;

public record struct CellData(CellType Type, byte Owner, byte state);