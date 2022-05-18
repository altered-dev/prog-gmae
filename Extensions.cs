public static class Extensions
{
	public static (int x, int y) ToCoords(this Direction direction)
	{
		var x = 0;
		var y = 0;

		if ((direction & Direction.Left) == Direction.Left)
			x -= 1;
		if ((direction & Direction.Right) == Direction.Right)
			x += 1;
		if ((direction & Direction.Up) == Direction.Up)
			y -= 1;
		if ((direction & Direction.Down) == Direction.Down)
			y += 1;

		return (x, y);
	}

	public static Direction ToDirection(this (int x, int y) pos)
	{
		var dir = default(Direction);

		if (pos.x < 0)
			dir |= Direction.Left;
		else if (pos.x > 0)
			dir |= Direction.Right;
		
		if (pos.y < 0)
			dir |= Direction.Up;
		else if (pos.y > 0)
			dir |= Direction.Down;

		return dir;
	}

	public static T PickRandom<T>(this List<T> list) => list[new Random().Next(list.Count)];
}