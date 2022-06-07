namespace prog_gmae.ExperimentalModel;

public static class Config
{
    public static int WindowWidth = 1280, WindowHeight = 720;
    public static string WindowTitle = "snek";

    public static Vector2 WindowCenter => new(WindowWidth / 2f, WindowHeight / 2f);

    public static int CellSize = 32;
    public static double FoodBalance = 0.5;
    public static double MazeDensity = 0.6;
    public static bool RandomMaze = true;
    public static bool SquareMaze = true;

    private static readonly Random random = new();
    
    public static readonly Size[] DirectionSizes =
        { new(1, 0), new(0, -1), new(-1, 0), new(0, 1) };
    public static readonly Direction[] Directions = 
        {Direction.Right, Direction.Up, Direction.Left, Direction.Down};
    public static readonly Color[] PlayerColors =
        {Color.GREEN, Color.BLUE};
    public static readonly Controls[] PlayerControls =
    {
        new(new()
        {
            [KeyboardKey.KEY_D] = Direction.Right,
            [KeyboardKey.KEY_W] = Direction.Up,
            [KeyboardKey.KEY_A] = Direction.Left,
            [KeyboardKey.KEY_S] = Direction.Down,
        }, KeyboardKey.KEY_Z),
        new(new()
        {
            [KeyboardKey.KEY_RIGHT] = Direction.Right,
            [KeyboardKey.KEY_UP]    = Direction.Up,
            [KeyboardKey.KEY_LEFT]  = Direction.Left,
            [KeyboardKey.KEY_DOWN]  = Direction.Down,
        }, KeyboardKey.KEY_SLASH),
    };

    public static readonly Color[] TeleportColors =
        { Color.BEIGE, Color.DARKBLUE, Color.GOLD, Color.MAGENTA, Color.LIME };

    public static bool GetChance(double mark) => random.NextDouble() < mark;
}