namespace prog_gmae.Model;

public static class Config
{
	public static int WindowWidth = 1280;
	public static int WindowHeight = 720;
	public static string WindowName = "haha yes";

	public const int CellSize = 32;
	public const int InitialWidth = 16;
	public const int InitialHeight = 16;
	public const int MazeDensity = 5; // 1 to 10
	public const int EnemySpeed = 5; // 1 to 10
	public const bool StartWithMultiplayer = false;
	public const bool RandomMaze = true;
	public const bool PerfectSquareMaze = false;
	public const bool DiggerMode = false;

	public static Vector2 WindowCenter => new(WindowWidth / 2, WindowHeight / 2);

	public static readonly List<Size> PossibleDirections = new()
		{ new(1, 0), new(0, -1), new(-1, 0), new(0, 1) };

	public static readonly (Dictionary<KeyboardKey, Direction> walkDirections, 
		KeyboardKey changeDirectionKey, Color color)[] PlayerProperties =
	{
		(new()
		{
			[KeyboardKey.KEY_D] = Direction.Right,
			[KeyboardKey.KEY_W] = Direction.Up,
			[KeyboardKey.KEY_A] = Direction.Left,
			[KeyboardKey.KEY_S] = Direction.Down,
		}, KeyboardKey.KEY_Z, Color.GREEN),
		(new()
		{
			[KeyboardKey.KEY_RIGHT] = Direction.Right,
			[KeyboardKey.KEY_UP] = Direction.Up,
			[KeyboardKey.KEY_LEFT] = Direction.Left,
			[KeyboardKey.KEY_DOWN] = Direction.Down,
		}, KeyboardKey.KEY_SLASH, Color.BLUE),
	};

	public static readonly Dictionary<KeyboardKey, Direction> Player1WalkDirections = new()
	{
		[KeyboardKey.KEY_D] = Direction.Right,
		[KeyboardKey.KEY_W] = Direction.Up,
		[KeyboardKey.KEY_A] = Direction.Left,
		[KeyboardKey.KEY_S] = Direction.Down,
	};

	public static readonly Dictionary<KeyboardKey, Direction> Player2WalkDirections = new()
	{
		[KeyboardKey.KEY_RIGHT] = Direction.Right,
		[KeyboardKey.KEY_UP] = Direction.Up,
		[KeyboardKey.KEY_LEFT] = Direction.Left,
		[KeyboardKey.KEY_DOWN] = Direction.Down,
	};

	public static readonly Dictionary<KeyboardKey, Size> ResetDirections = new()
	{
		[KeyboardKey.KEY_R] = new(0, 0),
		[KeyboardKey.KEY_MINUS] = new(-1, -1),
		[KeyboardKey.KEY_EQUAL] = new(1, 1),
	};
}