using System.Drawing;
using Raylib_cs;
using Color = Raylib_cs.Color;

public class Player
{
	public Point Position { get; set; }
	public Color Color { get; }
	public Dictionary<KeyboardKey, Direction> WalkDirections { get; }
	public int TailLength { get; set; }
	public int Score { get; set; }

	public LinkedList<Point> Tail { get; private set; } = new();

	public Player(Point startPosition, Color color, Dictionary<KeyboardKey, Direction> walkDirections)
	{
		Position = startPosition;
		Color = color;
		WalkDirections = walkDirections;
	}

	public void Move(Direction direction)
	{ 
		var newPos = Position + direction.ToCoords();
		if (Contains(newPos))
			return;
		Tail.AddFirst(Position);
		while (Tail.Count > TailLength)
			Tail.RemoveLast();
		Position = newPos;
	}

	public bool Contains(Point point) => Position == point || Tail.Any(point.Equals);

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