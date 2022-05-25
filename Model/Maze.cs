using System.Drawing;

public class Maze
{
	public int Width { get; }
	public int Height { get; }
	public Rectangle Bounds { get; }

	public Cell[,] Cells { get; }
	public Player Player { get; } = new();
	public List<Collectible> Collectibles { get; } = new();

	public int Score { get; private set; }

	public Maze(int width, int height, int collectibleCount)
	{
		if (width < 1)
			width = 1;
		if (height < 1)
			height = 1;
		Width = width;
		Height = height;
		Bounds = new(0, 0, width, height);
		Cells = GenerateMaze(width, height);
		var random = new Random();
		Player.Position = new Point(random.Next(width), random.Next(height));
		for (var i = 0; i < collectibleCount; i++)
			AddCollectible();
	}

	public Cell this[Point pos] => Cells[pos.X, pos.Y];

	public bool CharacterInBounds() => Bounds.Contains(Player.Position);

	public Collectible? GetCollectible(Point position) => Collectibles.Find(c => c.Position == position);

	private void AddCollectible()
	{
		var random = new Random();
		Point position;
		do position = new Point(random.Next(Width), random.Next(Height));
		while (Player.Contains(position) || Collectibles.Any(c => c.Position == position));
		Collectibles.Add(new(position));
	}

	public void TryCollect(Point position)
	{
		var collectible = GetCollectible(position);
		if (collectible == null)
			return;

		var random = new Random();
		Collectibles.Remove(collectible);
		AddCollectible();
		Score++;
		Player.TailLength++;
	}

	private static Cell[,] GenerateMaze(int width, int height)
	{
		var cells = new Cell[width, height];
		var visitedCount = 0;
		var random = new Random();
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
			var nextPosition = emptyNeighbours.PickRandom<Point>();
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