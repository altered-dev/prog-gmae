using System.Drawing;
using Raylib_cs;

public static class Extensions
{
	public static Size ToCoords(this Direction direction)
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

		return new(x, y);
	}

	public static Direction ToDirection(this Point pos)
	{
		var dir = Direction.None;

		if (pos.X < 0)
			dir |= Direction.Left;
		else if (pos.X > 0)
			dir |= Direction.Right;
		
		if (pos.Y < 0)
			dir |= Direction.Up;
		else if (pos.Y > 0)
			dir |= Direction.Down;

		return dir;
	}

	public static Direction Reverse(this Direction direction)
	{
		var dir = Direction.None;
		if ((direction & Direction.Left) != 0)
			dir |= Direction.Right;
		if ((direction & Direction.Right) != 0)
			dir |= Direction.Left;
		if ((direction & Direction.Up) != 0)
			dir |= Direction.Down;
		if ((direction & Direction.Down) != 0)
			dir |= Direction.Up;
		return dir;
	}

	private static readonly Random random = new();

	public static T PickRandom<T>(this List<T> list) => list[random.Next(list.Count)];

	public static void ProcessInput<T>(this Dictionary<KeyboardKey, T> mapping, Action<T> action)
	{
		foreach (var (key, value) in mapping)
			if (Raylib.IsKeyPressed(key))
				action(value);
	}

	public static bool IsInBounds(this Point point, int width, int height) =>
		0 <= point.X && point.X < width &&
		0 <= point.Y && point.Y < height;
}