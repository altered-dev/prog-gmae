namespace prog_gmae.Model;

public static class Generator
{
	public static Cell?[,] GenerateMaze(int width, int height, bool isRandom)
	{
		var cells = new Cell?[width, height];
		var stack = new Stack<Cell>();
		var random = new Random();
		var current = new Cell(random.Next(width), random.Next(height));
		cells[current.Position.X, current.Position.Y] = current;
		var visited = new HashSet<Point>();
		stack.Push(current);
		var directions = Config.PossibleDirections
			.OrderBy(_ => random.Next())
			.ToList();

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

	public static Collectible CreateCollectible(in Point position) => 
		new Random().Next(10) < 6 
			? new(position, -1, new(0, 0, 0, 125)) 
			: new(position, 1, new(255, 255, 255, 125));
}