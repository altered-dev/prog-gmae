using System.Drawing;

public class Player
{
	public Point Position { get; set; }
	public int TailLength { get; set; }

	public LinkedList<Point> Tail { get; private set; } = new();

	public Player(Point startPosition) => Position = startPosition;

	public void Move(Direction direction)
	{ 
		var newPos = Position + direction.ToCoords();
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
		if (Tail.First == null)
			return;
		Tail.AddLast(Position);
		Position = Tail.First.Value;
		Tail.RemoveFirst();
	}
}