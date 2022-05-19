using System.Numerics;
using Raylib_cs;
using System.Drawing;

public static class Config
{
	public static int WindowWidth = 1280;
	public static int WindowHeight = 720;
	public static string WindowName = "haha yes";
	public const int CellSize = 32;

	public static Vector2 WindowCenter => new(WindowWidth / 2, WindowHeight / 2);

	public static readonly List<Size> PossibleDirections = new()
		{ new(1, 0), new(0, -1), new(-1, 0), new(0, 1) };

	public static readonly Dictionary<KeyboardKey, Direction> WalkDirections = new()
	{
		[KeyboardKey.KEY_RIGHT] = Direction.Right,
		[KeyboardKey.KEY_UP] = Direction.Up,
		[KeyboardKey.KEY_LEFT] = Direction.Left,
		[KeyboardKey.KEY_DOWN] = Direction.Down
	};

	public static readonly Dictionary<KeyboardKey, Size> ResetDirections = new()
	{
		[KeyboardKey.KEY_R] = new(0, 0),
		[KeyboardKey.KEY_D] = new(1, 0),
		[KeyboardKey.KEY_W] = new(0, -1),
		[KeyboardKey.KEY_A] = new(-1, 0),
		[KeyboardKey.KEY_S] = new(0, 1)
	};
}