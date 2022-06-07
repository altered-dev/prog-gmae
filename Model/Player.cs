namespace prog_gmae.Model;

public record Player(
	Point Position, Color Color, 
	Dictionary<KeyboardKey, Direction> WalkDirections,
	KeyboardKey ChangeDirectionKey) : IMovable
{
	private Point position = Position;
	public Point Position
	{
		get => position;
		set
		{
			if (Contains(value))
				return;
			Tail.AddFirst(position);
			while (Tail.Count > TailLength)
				Tail.RemoveLast();
			position = value;
		}
	}

	public int TailLength { get; set; }
	public int Score { get; set; }
	public LinkedList<Point> Tail { get; private set; } = new();

	public bool Contains(Point point) => Position == point || Tail.Any(point.Equals);

	public void ChangeDirection()
	{
		Tail = new(Tail.Reverse());
		if (Tail.First == null)
			return;
		Tail.AddLast(position);
		position = Tail.First.Value;
		Tail.RemoveFirst();
	}
}