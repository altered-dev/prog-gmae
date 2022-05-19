using System.Drawing;

public class Maze
{
	public int Width { get; }
	public int Height { get; }

	public Cell[,] Cells { get; }
	public Character Character { get; } = new();

	public Maze(int width, int height)
	{
		Width = width;
		Height = height;
		Cells = GenerateMaze(width, height);
	}

	public Cell this[Point pos] => Cells[pos.X, pos.Y];

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