using System.Drawing;

public class Maze
{
	public readonly int Width, Height;
	public int Score { get; private set; }

	private readonly Cell[,] cells;
	public readonly Player Player = new();
	public readonly List<Teleport> Teleports = new();
	public readonly List<Collectible> Collectibles = new();

	private static readonly Random random = new();

	public Maze(int width, int height, int collectibleCount, int teleportCount)
	{
		Width = width < 1 ? 1 : width;
		Height = height < 1 ? 1 : height;
		cells = GenerateMaze(Width, Height);
		Player.Position = new Point(random.Next(Width), random.Next(Height));
		for (var i = 0; i < collectibleCount; i++)
			AddCollectible();
		for (var i = 0; i < teleportCount; i++)
			AddTeleports();
	}

	public Cell this[int x, int y] => cells[x, y];

	public Cell this[Point pos] => cells[pos.X, pos.Y];

	private void AddCollectible()
	{
		Point position;
		do position = new Point(random.Next(Width), random.Next(Height));
		while (Player.Contains(position) 
			|| Collectibles.Any(c => c.Position == position) 
			|| Teleports.Any(t => t.Position == position));
		Collectibles.Add(new(position));
	}

	private void AddTeleports()
	{
		// omg clutter
		Point position;
		do position = new Point(random.Next(Width), random.Next(Height));
		while (Player.Contains(position)
			|| Collectibles.Any(c => c.Position == position)
			|| Teleports.Any(t => t.Position == position));
		var teleportA = new Teleport(position);
		Teleports.Add(teleportA);
		do position = new Point(random.Next(Width), random.Next(Height));
		while (Player.Contains(position)
			|| Collectibles.Any(c => c.Position == position)
			|| Teleports.Any(t => t.Position == position));
		var teleportB = new Teleport(position);
		Teleports.Add(teleportB);
		teleportA.LinkTo(teleportB);
	}

	public void TryCollect(Point position)
	{
		var collectible = Collectibles.Find(c => c.Position == position);
		if (collectible == null)
			return;

		Collectibles.Remove(collectible);
		AddCollectible();
		Score++;
		Player.TailLength++;
	}

	public void TryTeleport(Point position)
	{
		var teleport = Teleports.Find(t => t.Position == position);
		if (teleport == null || teleport.Link == null || Player.Contains(teleport.Link.Position))
			return;
		Player.Position = teleport.Link.Position;
	}

	private static Cell[,] GenerateMaze(int width, int height)
	{
		var cells = new Cell[width, height];
		var visitedCount = 0;
		var stack = new Stack<Cell>();
		cells[0, 0] = new Cell(0, 0);
		stack.Push(cells[0, 0]);

		var current = cells[0, 0];

		while (visitedCount < width * height)
		{
			var emptyNeighbours = Config.PossibleDirections
				.Select(d => new Point(current.Position.X + d.Width, current.Position.Y + d.Height))
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