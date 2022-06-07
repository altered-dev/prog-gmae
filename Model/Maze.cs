namespace prog_gmae.Model;

public class Maze
{
	public readonly int Width, Height;
	public readonly List<Teleport> Teleports = new();
	public readonly List<Collectible> Collectibles;
	public readonly List<Enemy> Enemies;
	public readonly List<Player> Players;
	private readonly Cell?[,] cells;

	private static readonly Random random = new();

	public Maze(int width, int height, int playerCount, int collectibleCount, int teleportCount, int enemyCount)
	{
		Width = width < 1 ? 1 : width;
		Height = height < 1 ? 1 : height;
		cells = Generator.GenerateMaze(Width, Height, Config.RandomMaze);
		for (var i = 0; i < teleportCount; i++)
			AddTeleports(i);
		Players = Config.PlayerProperties
			.Take(playerCount)
			.Select(properties => new Player(GetRandomFreePoint(), properties.color,
				properties.walkDirections, properties.changeDirectionKey))
			.ToList();
		Collectibles = collectibleCount
			.Repeat(_ => Generator.CreateCollectible(GetRandomFreePoint()))
			.ToList();
		Enemies = enemyCount
			.Repeat(_ => new Enemy(GetRandomFreePoint(), Color.RED))
			.ToList();
	}

	public Cell? this[int x, int y]
	{
		get => cells[x, y];
		set => cells[x, y] = value;
	}

	public Cell? this[Point pos]
	{
		get => cells[pos.X, pos.Y];
		set => cells[pos.X, pos.Y] = value;
	}

	public bool IsCellOccupied(Point position) => 
		this[position] == null ||
		Players.Any(player => player.Contains(position)) ||
		Collectibles.Any(c => c.Position == position) ||
		Teleports.Any(t => t.Position == position) ||
		Enemies.Any(e => e.Position == position);

	public Point GetRandomFreePoint()
	{
		Point result;
		do result = new(random.Next(Width), random.Next(Height));
		while (IsCellOccupied(result));
		return result;
	}

	public void AddCollectible() => 
		Collectibles.Add(Generator.CreateCollectible(GetRandomFreePoint()));

	private void AddTeleports(int i)
	{

	}

	public void TryCollect(params Player[] players)
	{
		foreach (var player in players)
		{
			var collectible = Collectibles.Find(c => c.Position == player.Position);
			if (collectible == null)
				continue;
			Collectibles.Remove(collectible);
			AddCollectible();
			player.Score += 1;
			player.TailLength += collectible.TailLengthDelta;
		}
	}

	public void AddWall(Point position, Direction direction)
	{
		var newPos = position + direction.ToCoords();
		if (!newPos.IsInBounds(Width, Height))
			return;
		this[position]!.Connections &= ~direction;
		this[newPos]!.Connections &= ~direction.Reverse();
	}

	public void RemoveWall(Point position, Direction direction)
	{
		var newPos = position + direction.ToCoords();
		if (!newPos.IsInBounds(Width, Height))
			return;
		this[position]!.Connections |= direction;
		this[newPos] ??= new(newPos);
		this[newPos]!.Connections |= direction.Reverse();
	}

	public void TryTeleportPlayer(Player player) => Teleports
		.Find(t => t.Position == player.Position)?
		.MovePlayer(player);
}