using System.Drawing;

public static class Generator
{
	public static Cell[,] GenerateMaze(int width, int height)
	{
		var cells = new Cell[width, height];
		var stack = new Stack<Cell>();
		var current = cells[0, 0] = new(0, 0);
		stack.Push(current);

		for (var visitedCount = 0; visitedCount < width * height;)
		{
			var emptyNeighbours = Config.PossibleDirections
				.Select(direction => current.Position + direction)
				.Where(coord => coord.IsInBounds(width, height) && cells[coord.X, coord.Y] == null)
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

	public static Collectible CreateCollectible(Point position)
	{
		var random = new Random();
		if (random.Next(10) < 6)
			return new GoodCollectible(position);
		return new BadCollectible(position);
	}
}