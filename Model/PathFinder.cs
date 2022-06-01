using System.Drawing;

public static class PathFinder
{
	private static readonly Size[] directions = 
		{ new(-1, 0), new(0, -1), new(1, 0), new(0, 1) };

	public static List<Size>? FindPath(this Maze maze, Point from, Point to, bool ignoreEnemies = false, bool cosmetic = false)
	{
		var visited = new HashSet<Point>();
		var queue = new Queue<Link<Point>>();
		queue.Enqueue(new(from));
		Link<Point>? result = null;
		while (queue.Any())
		{
			var link = queue.Dequeue();
			var point = link.Value;
			var tp = maze.Teleports.Find(tp => tp.Position == point);
			if (tp != null)
				if (cosmetic)
					link = new(point = tp.Link!.Position, link);
				else
					link.Value = point = tp.Link!.Position;
			if (!point.IsInBounds(maze.Width, maze.Height) || visited.Contains(point)
				|| (!ignoreEnemies && maze.Enemies.Any(e => e.Position == point)))
				continue;
			if (point == to)
			{
				result = link;
				break;
			}
			visited.Add(point);
			foreach (var direction in directions.Where(dir => (maze[point].Connections & new Point(dir).ToDirection()) != 0))
				queue.Enqueue(new(point + direction, link));
		}
		if (result == null)
			return null;
		var list = result.ToList();
		return list.Zip(list.Skip(1), (a, b) => new Size(b.X - a.X, b.Y - a.Y)).ToList();
	}

	private class Link<T>
	{
		public T Value { get; set; }
		public Link<T>? Previous { get; }

		public Link(T value, Link<T>? previous = null)
		{
			Value = value;
			Previous = previous;
		}

		public List<T> ToList()
		{
			var list = new LinkedList<T>();
			Link<T>? current = this;
			while (current != null)
			{
				list.AddFirst(current.Value);
				current = current.Previous;
			}
			return list.ToList();
		}
	}
}
