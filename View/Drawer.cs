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
		foreach (var collectible in maze.Collectibles)
			maze.DrawCollectible(collectible);
		foreach (var teleport in maze.Teleports)
			maze.DrawTeleport(teleport);
	}

	public static void DrawCell(this Maze maze, int x, int y)
	{
		var cell = maze[x, y];
		if (cell == null)
			return;
		var pos = maze.MazeToScreen(cell.Position);
		var right = pos.X + Config.CellSize / 2;
		var up = pos.Y - Config.CellSize / 2;
		var left = pos.X - Config.CellSize / 2;
		var down = pos.Y + Config.CellSize / 2;

		DrawRectangle(left, up, right - left, down - up, cell.Color);

		if ((cell.Connections & Direction.Right) == 0)
			DrawLine(right, up, right, down, Color.LIGHTGRAY);
		if ((cell.Connections & Direction.Up) == 0)
			DrawLine(left, up, right, up, Color.LIGHTGRAY);
		if ((cell.Connections & Direction.Left) == 0)
			DrawLine(left, up - 1, left, down, Color.LIGHTGRAY);
		if ((cell.Connections & Direction.Down) == 0)
			DrawLine(left, down, right, down, Color.LIGHTGRAY);
	}

	public static void DrawPlayer(this Maze maze, Player player)
	{
		var previous = maze.MazeToScreen(player.Position);
		var random = new Random();
		var i = 0;
		var size = Config.CellSize / 3;
		foreach (var point in player.Tail.Select(p => maze.MazeToScreen(p)))
			DrawCircle(point.X, point.Y, size - (i++ * 5) / player.Tail.Count, player.Color);

		var pos = maze.MazeToScreen(player.Position);
		DrawCircle(pos.X, pos.Y, Config.CellSize / 2 - 4, player.Color);
		DrawCircleLines(pos.X, pos.Y, Config.CellSize / 2 - 8, new Color(0, 0, 0, 125));
	}

	public static void DrawCollectible(this Maze maze, Collectible collectible)
	{
		var pos = maze.MazeToScreen(collectible.Position);
		DrawCircle(pos.X, pos.Y, Config.CellSize / 4, Color.ORANGE);
		DrawCircle(pos.X, pos.Y, Config.CellSize / 4 - 4, collectible.SecondaryColor);
	}

	public static void DrawTeleport(this Maze maze, Teleport teleport)
	{
		var pos = maze.MazeToScreen(teleport.Position);
		DrawCircle(pos.X, pos.Y, Config.CellSize / 2 - 4, teleport.Color);
		DrawCircle(pos.X, pos.Y, Config.CellSize / 2 - 10, new Color(0, 0, 0, 125));
		DrawCircleLines(pos.X, pos.Y, Config.CellSize / 2 - 4, Color.RAYWHITE);
	}

	public static Point MazeToScreen(this Maze maze, Point position) => new(
		(int)((position.X - (maze.Width / 2.0f) + 0.5f) * Config.CellSize), 
		(int)((position.Y - (maze.Height / 2.0f) + 0.5f) * Config.CellSize));
}