using Godot;
using System;

public readonly record struct CellData
{
    public readonly CellType Type;
    public readonly byte Owner; 
    public readonly byte Age;  
}