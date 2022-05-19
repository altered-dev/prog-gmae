using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Drawing;
using Color = Raylib_cs.Color;

public static class Drawer
{
	public static void DrawMaze(this Maze maze)
	{
		for (var x = 0; x < maze.Width; x++)
		for (var y = 0; y < maze.Height; y++)
			maze.DrawCell(x, y);
	}

	public static void DrawCell(this Maze maze, int x, int y)
	{
		var cell = maze.Cells[x, y];
		if (cell == null)
			return;
		var pos = maze.MazeToScreen(cell.Position);
		var right = pos.X + Config.CellSize / 2;
		var up = pos.Y - Config.CellSize / 2;
		var left = pos.X - Config.CellSize / 2;
		var down = pos.Y + Config.CellSize / 2;
		if ((cell.Connections & Direction.Right) == 0)
			DrawLine(right, up, right, down, Color.BLACK);
		if ((cell.Connections & Direction.Up) == 0)
			DrawLine(left, up, right, up, Color.BLACK);
		if ((cell.Connections & Direction.Left) == 0)
			DrawLine(left, up - 1, left, down, Color.BLACK);
		if ((cell.Connections & Direction.Down) == 0)
			DrawLine(left, down, right, down, Color.BLACK);
	}

	public static void DrawCharacter(this Maze maze)
	{
		var pos = maze.MazeToScreen(maze.Character.Position);
		DrawCircle(pos.X, pos.Y, Config.CellSize / 2 - 4, Color.GREEN);
	}

	public static Point MazeToScreen(this Maze maze, Point position) => new(
		(int)((position.X - (maze.Width / 2.0f) + 0.5f) * Config.CellSize), 
		(int)((position.Y - (maze.Height / 2.0f) + 0.5f) * Config.CellSize));
}