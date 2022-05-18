using System.Linq;

public class Maze
{
	public int Width { get; }
	public int Height { get; }

	public Cell[,] Cells { get; }

	public Maze(int width, int height)
	{
		Width = width;
		Height = height;
		Cells = GenerateMaze(width, height);
	}

	public Cell this[(int x, int y) pos] => Cells[pos.x, pos.y];

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
				.Select(d => (x: current.Position.x + d.x, y: current.Position.y + d.y))
				.Where(coord => 0 <= coord.x && coord.x < width && 0 <= coord.y && coord.y < height)
				.Where(coord => cells[coord.x, coord.y] == null)
				.ToList();
			if (!emptyNeighbours.Any())
			{
				if (!stack.Any())
					break;
				
				current = stack.Pop();
				continue;
			}
			var (x, y) = emptyNeighbours.PickRandom<(int, int)>();
			var next = new Cell(x, y);
			var dx = x - current.Position.x;
			var dy = y - current.Position.y;

			current.Connections |= (dx, dy).ToDirection();
			next.Connections |= (-dx, -dy).ToDirection();

			cells[x, y] = next;
			stack.Push(next);
			visitedCount++;
			current = next;
		}

		return cells;
	}
}