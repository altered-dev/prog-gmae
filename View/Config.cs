using System.Numerics;

public static class Config
{
	public static int WindowWidth = 1280;
	public static int WindowHeight = 720;
	public static string WindowName = "haha yes";
	public const int CellSize = 32;

	public static Vector2 WindowCenter => new(WindowWidth / 2, WindowHeight / 2);

	public static readonly (int x, int y)[] PossibleDirections = 
		{ (1, 0), (0, -1), (-1, 0), (0, 1) };
}