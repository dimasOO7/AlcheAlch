using Godot;
using System;

/// <summary>
/// магазин
/// </summary>
public partial class Shop : Control
{
    public GameManager gameManager {get;set;}
    public TileMapLayer map {get;set;}

    Vector2I gridPos;

    [Export] public HBoxContainer container;

    [Export] public PackedScene shopPosition;

    public override void _Ready()
    {
        gameManager = GetTree().CurrentScene as GameManager;
        map = gameManager.tileMap;
        Visible = false;
    }
    
    /// <summary>
    /// обновить магаз
    /// </summary>
    /// <param name="pos">для какой клетки магаз</param>
    public void UpdateShop(Vector2I pos)
    {
        foreach (Node child in container.GetChildren())
            child.QueueFree(); //очистка магаза
        
        CellData self = gameManager.grid[pos.X,pos.Y];
        Cell  behavior = gameManager.registry.Get(self.Type);
        container.GlobalPosition = GetLocalMousePosition();
        foreach(CellAction action in behavior.GetAvailableActions(self,pos,gameManager.grid))
        {
            ShopPosition shopPos = shopPosition.Instantiate<ShopPosition>();
            container.AddChild(shopPos);

            shopPos.Initialize(action,this,gameManager.CanBuy(action));
        }
    }

    public void OnButtonPressed(CellAction action)
    {
        if (action.Cost <= gameManager.Score)
        {
        CloseShop();
        gameManager.UseAction(action,gridPos);
        }
    }

    /// <summary>
    /// скрытие и очистка магаза
    /// </summary>
    private void CloseShop()
    {
        Visible = false;
        foreach (Node child in container.GetChildren())
            child.QueueFree();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left &&
            mouseEvent.Pressed == false)
        {
            Vector2I position = map.LocalToMap(map.GetLocalMousePosition());
            if (position.X >= 0 && position.Y >= 0 && position.X < gameManager.GridWidth && position.Y < gameManager.GridHeight)
            {
                if(gameManager.grid[position.X,position.Y].Owner > 0 && Visible == false)
                {
                gridPos = position;
                UpdateShop(position);
                Visible = true;
                }
                else
                {
                    CloseShop();
                }
            }
        }
    }
}
