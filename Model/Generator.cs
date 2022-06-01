using System.Drawing;

public static class Generator
{
	public static Cell[,] GenerateMaze(int width, int height, bool isRandom = true)
	{
		var cells = new Cell[width, height];
		var stack = new Stack<Cell>();
		var random = new Random();
		var x = random.Next(width);
		var y = random.Next(height);
		var current = cells[x, y] = new(x, y);
		var visited = new HashSet<Point>();
		stack.Push(current);
		var directions = Config.PossibleDirections.OrderBy(_ => random.Next()).ToList();

		for (var visitedCount = 0; visitedCount < width * height;)
		{
			var emptyNeighbours = directions
				.Select(direction => current.Position + direction)
				.Where(coord => coord.IsInBounds(width, height) && (cells[coord.X, coord.Y] == null || !visited.Contains(coord)))
				.ToList();
			if (!emptyNeighbours.Any())
			{
				if (!stack.Any())
					break;
				current = stack.Pop();
				continue;
			}
			var nextPosition = isRandom ? emptyNeighbours.PickRandom() : emptyNeighbours.First();
			var next = cells[nextPosition.X, nextPosition.Y] ?? new Cell(nextPosition);
			var dx = nextPosition.X - current.Position.X;
			var dy = nextPosition.Y - current.Position.Y;

			current.Connections |= new Point(dx, dy).ToDirection();
			next.Connections |= new Point(-dx, -dy).ToDirection();

			cells[nextPosition.X, nextPosition.Y] = next;
			stack.Push(next);
			if (random.Next(10) < Config.MazeDensity && !visited.Contains(nextPosition))
			{
				visited.Add(nextPosition);
				if (Config.PerfectSquareMaze)
					visitedCount++;
			}
			if (!Config.PerfectSquareMaze)
				visitedCount++;
			current = next;
		}

		return cells;
	}

	public static Collectible CreateCollectible(Point position)
	{
		var random = new Random();
		if (random.Next(10) < 6)
			return new GoodCollectible(position);
		return new BadCollectible(position);
	}
}