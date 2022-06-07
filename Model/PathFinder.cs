namespace prog_gmae.Model;

public static class PathFinder
{
	private static IEnumerable<Size> GetDirections(this Maze maze, Point point) => 
		Config.PossibleDirections
			.Where(dir => (maze[point]?.Connections & new Point(dir).ToDirection()) != 0);

	public static List<Size>? FindPath(this Maze maze, Point from, Point to, 
		bool ignoreEnemies = false, bool cosmetic = false, params Player[] players)
	{
		var visited = new HashSet<Point>();
		var queue = new Queue<Link<Point>>();
		var root = new Link<Point>(from);
		foreach (var dir in maze.GetDirections(from))
			queue.Enqueue(new(from + dir, root));
		while (queue.Any())
		{
			var link = queue.Dequeue();
			var point = link.Value;
			var tp = maze.Teleports.Find(tp => tp.Position == point);
			if (tp != null)
			{
				if (cosmetic)
					link = new(point = tp.Link!.Position, link);
				else
					link.Value = point = tp.Link!.Position;
			}
			if (!point.IsInBounds(maze.Width, maze.Height) 
			    || visited.Contains(point)
			    || players.Any(p => p.Contains(point))
			    || (!ignoreEnemies && maze.Enemies.Any(e => e.Position == point)))
				continue;
			if (point == to)
			{
				var list = link.ToList();
				return list.Zip(list.Skip(1), (a, b) => new Size(b.X - a.X, b.Y - a.Y)).ToList();
			}
			visited.Add(point);
			foreach (var direction in maze.GetDirections(point))
				queue.Enqueue(new(point + direction, link));
		}
		return null;
	}

	private record Link<T>(T Value, Link<T>? Previous = null)
	{
		public T Value { get; set; } = Value;

		public List<T> ToList()
		{
			var list = new LinkedList<T>();
			var current = this;
			while (current != null)
			{
				list.AddFirst(current.Value);
				current = current.Previous;
			}
			return list.ToList();
		}
	}
}