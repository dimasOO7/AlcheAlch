using Godot;
using System;
using System.Threading.Tasks.Dataflow;

public partial class ShopPosition : Control
{
    public CellAction action;

    [Export] TileMapLayer icon;
    [Export] Button button;

    public void Initialize(CellAction action, Shop shop)
    {
        button.Text = $"{(action.Cost >= 0 ? "−" : "+")}{Mathf.Abs(action.Cost)}";
        button.Pressed += () => shop.OnButtonPressed(action);
        icon.SetCell(Vector2I.Zero,0,action.IconCoord);
    }
}
