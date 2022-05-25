using System.Drawing;

public class Player
{
	public Point Position { get; set; }
	public int TailLength { get; set; }

	public LinkedList<Point> Tail { get; private set; } = new();

	private static readonly Dictionary<Direction, Size> MoveDirections = new()
	{
		[Direction.Right] = new(1, 0),
		[Direction.Up] = new(0, -1),
		[Direction.Left] = new(-1, 0),
		[Direction.Down] = new(0, 1)
	};

	public void Move(Direction direction)
	{ 
		var newPos = Position + MoveDirections[direction];
		if (Contains(newPos))
			return;
		Tail.AddFirst(Position);
		if (Tail.Count > TailLength)
			Tail.RemoveLast();
		Position = newPos;
	}

	public bool Contains(Point point) => Position == point || Tail.Any(p => p == point);

	public void ChangeDirection()
	{
		Tail = new(Tail.Reverse());
		if (Tail.First != null)
		{
			Tail.AddLast(Position);
			Position = Tail.First.Value;
			Tail.RemoveFirst();
		}
		Console.WriteLine();
	}
}