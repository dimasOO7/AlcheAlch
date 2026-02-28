using Godot;
using System;
/// <summary>
/// основной класс управляющий игной
/// </summary>
public partial class GameManager : Node2D 
{
    /// <summary>
    /// размер поля по Х
    /// </summary>
    [Export] public int GridWidth = 80;
    /// <summary>
    /// размер поля по Y
    /// </summary>
    [Export] public int GridHeight = 50;
    /// <summary>
    /// размер 1 клетки в пикслелях, не трогать
    /// </summary>
    [Export] public int CellSize = 32;

    /// <summary>
    /// импорты всяких нужных нод
    /// </summary>
    [Export] public TileMapLayer tileMap;
    [Export] public TileMapLayer ownerMap;
    [Export] public Camera2D camera;

    /// <summary>
    /// начальное кол-во очков
    /// </summary>
    [Export] public int startScore = 10;
    
    [Signal]
    public delegate void ScoreChangedSignalEventHandler(int score, int delta);

    private int score;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            EmitSignal(SignalName.ScoreChangedSignal,value,value - score);
            score = value;
        }
    }

    [Export] public int playerStartZone = 3;

    public GridData grid;
    public CellRegistry registry;

    private RandomNumberGenerator rng;

    public override void _Ready()
    {
        base._Ready();

        Score = startScore;

        grid = new GridData(GridWidth,GridHeight);
        registry = new CellRegistry();

        rng = new RandomNumberGenerator();
        InitializeRandomGrid();

        camera.GlobalPosition = new Vector2(GridWidth * CellSize / 2,GridHeight * CellSize / 2);
        camera.LimitLeft = 0;
        camera.LimitRight = CellSize * GridWidth;
        camera.LimitTop = 0;
        camera.LimitBottom = CellSize * GridHeight;

        RenderAll();
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Space)
        {
            Tick();
        }
    }

    /// <summary>
    /// получить Cell для обработки в других местах
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Cel</returns>
    public Cell GetBehavior(Vector2I pos)
    {
        return registry.Get(grid[pos.X,pos.Y].Type);
    }
    /// <summary>
    /// делает одну итерацию игры (один раз обрабатывает все клетки и реакции)
    /// </summary>
    private void Tick()
    {
        int delta = 0;
        for (int y = 0; y < GridHeight; y++)
        for (int x = 0; x < GridWidth; x++)
        {
            int idx = y * GridWidth + x;
            CellData self = grid.Current[idx];

            CellData[] neighbors = grid.GetNeighbors(x,y);

            Cell  behavior = registry.Get(self.Type);
            var (newState, scoreDelta) = behavior.Execute(self, neighbors, new Vector2I(x, y), grid);
            grid.Next[idx] = newState;
            delta += scoreDelta;
        }
        (grid.Current, grid.Next) = (grid.Next, grid.Current);

        Score += delta;
        RenderAll();
    }

    /// <summary>
    /// начальная генерация карты
    /// </summary>
    private void InitializeRandomGrid()
    {
        for (int y = 0; y < GridHeight; y++)
        for (int x = 0; x < GridWidth; x++)
        {
            CellType type = (CellType) rng.RandiRange(1, 5);
            if (type == CellType.Fire && rng.Randf() > 0.3f) type = CellType.Soil;

            grid[x, y] = new CellData { Type = type, Owner = 0 };
        }
        for (int y = GridHeight/2 - playerStartZone / 2 - playerStartZone % 2;y < GridHeight/2 + playerStartZone % 2; y++)
        for (int x = GridWidth/2 - playerStartZone / 2 - playerStartZone % 2;x < GridWidth/2 + playerStartZone % 2; x++)
            {
                grid[x, y] = new CellData { Type = (CellType) rng.RandiRange(3, 4), Owner = 1 };
            }
    }
    
    /// <summary>
    /// отрисовать 1 клетку
    /// </summary>
    /// <param name="pos"></param>
    private void RenderTile(Vector2I pos)
    {
        CellData cell = grid[pos.X,pos.Y];
        Cell behavior = registry.Get(cell.Type);
        tileMap.SetCell(pos, 0, behavior.GetTextureCord(cell));
        ownerMap.SetCell(pos, 0, new Vector2I(cell.Owner,0));
    }
    
    /// <summary>
    /// применить действие (заменить клетку)
    /// </summary>
    /// <param name="action"></param>
    /// <param name="pos"></param>
    public void UseAction(CellAction action, Vector2I pos)
    {
        var (newState, scoreDelta) = action.Execute(grid[pos.X,pos.Y],pos,grid);
        GD.Print(newState);
        grid[pos.X,pos.Y] = newState;
        Score -= scoreDelta;
        RenderTile(pos);
    }

    public bool CanBuy(CellAction action)
    {
        return action.Cost <= score || action.Cost < 0;
    }

    /// <summary>
    /// перерисовать всё поле (пиздец не оптимизированная хрень, которую невозможно норм оптимизировать, но для пошага на поле менее 100х100 норм)
    /// </summary>
    private void RenderAll()
    {
        for (int y = 0; y < GridHeight; y++)
        for (int x = 0; x < GridWidth; x++)
        {
            CellData cell = grid[x, y];
            Cell behavior = registry.Get(cell.Type);
            tileMap.SetCell(new Vector2I(x, y), 0, behavior.GetTextureCord(cell));
            ownerMap.SetCell(new Vector2I(x, y), 0, new Vector2I(cell.Owner,0));
        }
    }
}
