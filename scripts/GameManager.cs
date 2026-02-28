using Godot;
using System;
/// <summary>
/// основной класс управляющий игрой
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
			
			// Пропускаем незахваченные клетки (Owner = 0)
			if (self.Owner == 0)
			{
				// Просто копируем текущее состояние в следующий кадр без изменений
				grid.Next[idx] = self;
				continue;
			}

			CellData[] neighbors = grid.GetNeighbors(x,y);

			Cell behavior = registry.Get(self.Type);
			var (newState, scoreDelta) = behavior.Execute(self, neighbors, new Vector2I(x, y), grid);
			grid.Next[idx] = newState;
			delta += scoreDelta;
		}
		
		// Меняем местами текущий и следующий буферы
		(grid.Current, grid.Next) = (grid.Next, grid.Current);

		Score += delta;
		RenderAll();
}

	private void InitializeRandomGrid()
	{
		// 1. Заполняем всё нейтральной почвой/огнем
		for (int y = 0; y < GridHeight; y++)
		for (int x = 0; x < GridWidth; x++)
		{
			grid[x, y] = new CellData { Type = (CellType)rng.RandiRange(3, 4), Owner = 0 };
		}

		// 2. Создаем много случайных точек для спавна ресурсов
		// Увеличил количество попыток до 100, чтобы точно забить карту
		var spots = new System.Collections.Generic.List<Vector2I>();
		for (int i = 0; i < 100; i++) 
		{
			spots.Add(new Vector2I(rng.RandiRange(2, GridWidth - 3), rng.RandiRange(2, GridHeight - 3)));
		}

		// 3. Генерируем много пятен КИСЛОТЫ (50 штук)
		// Кислота теперь будет повсюду средними группами
		for (int i = 0; i < 50; i++) 
		{
			GrowBlob(spots[i], CellType.Acid, rng.RandiRange(6, 12));
		}

		// 4. Генерируем много луж ВОДЫ (50 штук)
		// Вода будет в других точках списка, чтобы не перекрывать кислоту
		for (int i = 50; i < 100; i++) 
		{
			GrowBlob(spots[i], CellType.Water, rng.RandiRange(4, 10));
		}

		// 5. Возвращаем твою исходную зону игрока
		int startX = GridWidth / 2;
		int startY = GridHeight / 2;
		int half = playerStartZone / 2;

		for (int y = startY - half; y <= startY + half; y++)
		for (int x = startX - half; x <= startX + half; x++)
		{
			if (x >= 0 && x < GridWidth && y >= 0 && y < GridHeight)
			{
				grid[x, y] = new CellData { Type = (CellType)rng.RandiRange(3, 4), Owner = 1 };
			}
		}
	}

	/// <summary>
	/// Метод разращивания пятна (блоба)
	/// </summary>
	private void GrowBlob(Vector2I startPos, CellType type, int size)
	{
		// Проверка границ, чтобы не вылететь при старте
		if (startPos.X < 0 || startPos.X >= GridWidth || startPos.Y < 0 || startPos.Y >= GridHeight) return;

		var cells = new System.Collections.Generic.List<Vector2I> { startPos };
		grid[startPos.X, startPos.Y] = new CellData { Type = type, Owner = 0 };

		for (int i = 0; i < size; i++)
		{
			// Берем случайную уже созданную клетку пятна и растем от неё
			Vector2I baseCell = cells[rng.RandiRange(0, cells.Count - 1)];
			Vector2I next = baseCell + new Vector2I(rng.RandiRange(-1, 1), rng.RandiRange(-1, 1));

			// Проверяем границы карты и чтобы не залезть в зону игрока случайно
			if (next.X > 0 && next.X < GridWidth - 1 && next.Y > 0 && next.Y < GridHeight - 1)
			{
				// Если клетка еще не захвачена игроком — красим её
				if (grid[next.X, next.Y].Owner == 0)
				{
					grid[next.X, next.Y] = new CellData { Type = type, Owner = 0 };
					if (!cells.Contains(next)) cells.Add(next);
				}
			}
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
