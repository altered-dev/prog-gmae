using System.Drawing;

public class Character
{
	public Point Position { get; set; }

	public readonly LinkedList<Point> Trail = new();

	private static readonly Dictionary<Direction, Size> MoveDirections = new()
	{
		[Direction.Right] = new(1, 0),
		[Direction.Up] = new(0, -1),
		[Direction.Left] = new(-1, 0),
		[Direction.Down] = new(0, 1)
	};

	public void Move(Direction direction)
	{ 
		Trail.AddFirst(Position);
		if (Trail.Count > 100)
			Trail.RemoveLast();
		Position += MoveDirections[direction];
	}
}