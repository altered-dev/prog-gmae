using System.Drawing;

public class Maze
{
	public readonly int Width, Height;
	public int Score { get; private set; }

	private readonly Cell[,] cells;
	public readonly Player Player = new(Point.Empty);
	public readonly List<Teleport> Teleports = new();
	public readonly List<Collectible> Collectibles = new();

	private static readonly Random random = new();

	public Maze(int width, int height, int collectibleCount, int teleportCount)
	{
		Width = width < 1 ? 1 : width;
		Height = height < 1 ? 1 : height;
		cells = GenerateMaze(Width, Height);
		for (var i = 0; i < collectibleCount; i++)
			AddCollectible();
		for (var i = 0; i < teleportCount; i++)
			AddTeleports();
	}

	public Cell this[int x, int y] => cells[x, y];

	public Cell this[Point pos] => cells[pos.X, pos.Y];

	public bool IsInBounds(Point position) =>
		0 <= position.X && position.X < Width &&
		0 <= position.Y && position.Y < Height;

	public bool IsCellOccupied(Point position, Player? player = null) => 
		player?.Contains(position) ?? false ||
		Collectibles.Any(c => c.Position == position) ||
		Teleports.Any(t => t.Position == position);

	public Point GetRandomFreePoint(Player? player = null)
	{
		Point result;
		do result = new Point(random.Next(Width), random.Next(Height));
		while (IsCellOccupied(result, player));
		return result;
	}

	private void AddCollectible(Player? player = null) => Collectibles.Add(new(GetRandomFreePoint(player)));

	private void AddTeleports()
	{
		var teleportA = new Teleport(GetRandomFreePoint());
		var teleportB = new Teleport(GetRandomFreePoint());
		Teleports.Add(teleportA);
		Teleports.Add(teleportB);
		teleportA.LinkTo(teleportB);
	}

	public void TryCollect(Player player)
	{
		var collectible = Collectibles.Find(c => c.Position == player.Position);
		if (collectible == null)
			return;

		Collectibles.Remove(collectible);
		AddCollectible(player);
		Score++;
		player.TailLength++;
	}

	public void TryTeleportPlayer(Player player) => Teleports
		.Find(t => t.Position == player.Position)?
		.MovePlayer(player);

	private static Cell[,] GenerateMaze(int width, int height)
	{
		var cells = new Cell[width, height];
		var stack = new Stack<Cell>();
		var current = cells[0, 0] = new Cell(0, 0);
		stack.Push(current);

		for (var visitedCount = 0; visitedCount < width * height;)
		{
			var emptyNeighbours = Config.PossibleDirections
				.Select(d => current.Position + d)
				.Where(coord => 0 <= coord.X && coord.X < width && 0 <= coord.Y && coord.Y < height)
				.Where(coord => cells[coord.X, coord.Y] == null)
				.ToList();
			if (!emptyNeighbours.Any())
			{
				if (!stack.Any())
					break;
				
				current = stack.Pop();
				continue;
			}
			var nextPosition = emptyNeighbours.PickRandom();
			var next = new Cell(nextPosition);
			var dx = nextPosition.X - current.Position.X;
			var dy = nextPosition.Y - current.Position.Y;

			current.Connections |= new Point(dx, dy).ToDirection();
			next.Connections |= new Point(-dx, -dy).ToDirection();

			cells[nextPosition.X, nextPosition.Y] = next;
			stack.Push(next);
			visitedCount++;
			current = next;
		}

		return cells;
	}
}