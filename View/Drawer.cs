using Raylib_cs;
using static Raylib_cs.Raylib;

public static class Drawer
{
	public static void DrawCell(this Maze maze, Cell cell)
	{
		var (x, y) = maze.MazeToScreen(cell);
		var right = x + Config.CellSize / 2;
		var up = y - Config.CellSize / 2;
		var left = x - Config.CellSize / 2;
		var down = y + Config.CellSize / 2;
		if ((cell.Connections & Direction.Right) == 0)
			DrawLine(right, up, right, down, Color.BLACK);
		if ((cell.Connections & Direction.Up) == 0)
			DrawLine(left, up, right, up, Color.BLACK);
		if ((cell.Connections & Direction.Left) == 0)
			DrawLine(left, up - 1, left, down, Color.BLACK);
		if ((cell.Connections & Direction.Down) == 0)
			DrawLine(left, down, right, down, Color.BLACK);
	}

	public static void DrawCharacter(this Maze maze, Character character)
	{
		var (x, y) = maze.MazeToScreen(character);
		DrawCircle(x, y, Config.CellSize / 2 - 4, Color.GREEN);
	}

	public static (int x, int y) MazeToScreen(this Maze maze, int x, int y)
	{
		var dx = (int) ((x - (maze.Width / 2.0f) + 0.5f) * Config.CellSize);
		var dy = (int) ((y - (maze.Height / 2.0f) + 0.5f) * Config.CellSize);
		return (dx, dy);
	}

	public static (int x, int y) MazeToScreen(this Maze maze, Cell cell) => 
		maze.MazeToScreen(cell.Position.x, cell.Position.y);

	public static (int x, int y) MazeToScreen(this Maze maze, Character character) =>
		maze.MazeToScreen(character.Position.x, character.Position.y);
}