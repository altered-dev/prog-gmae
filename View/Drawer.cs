using prog_gmae.Model;
using static Raylib_cs.Raylib;

namespace prog_gmae.View;

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
		var pos = maze.MazeToScreenV(cell.Position) + Vector2.One * 0.5f;
		var right = pos.X + Config.CellSize / 2f;
		var up    = pos.Y - Config.CellSize / 2f;
		var left  = pos.X - Config.CellSize / 2f;
		var down  = pos.Y + Config.CellSize / 2f;

		DrawRectangleRec(new(left, up, right - left, down - up), cell.Color);

		if ((cell.Connections & Direction.Right) == 0)
			DrawLineV(new(right, up), new(right, down), Color.LIGHTGRAY);
		if ((cell.Connections & Direction.Up) == 0)
			DrawLineV(new(left, up), new(right, up), Color.LIGHTGRAY);
		if ((cell.Connections & Direction.Left) == 0)
			DrawLineV(new(left, up), new(left, down), Color.LIGHTGRAY);
		if ((cell.Connections & Direction.Down) == 0)
			DrawLineV(new(left, down), new(right, down), Color.LIGHTGRAY);
	}

	public static void DrawPlayer(this Maze maze, Player player)
	{
		var i = 0;
		var size = Config.CellSize / 3;
		foreach (var point in player.Tail.Select(maze.MazeToScreenV))
			DrawCircleV(point, size - i++ * 5 / player.Tail.Count, player.Color);
		
		var pos = maze.MazeToScreenV(player.Position);
		DrawCircleV(pos, Config.CellSize / 2 - 4, player.Color);
	}

	public static void DrawCollectible(this Maze maze, Collectible collectible)
	{
		var pos = maze.MazeToScreen(collectible.Position);
		DrawCircle(pos.X, pos.Y, Config.CellSize / 4, Color.ORANGE);
		DrawCircle(pos.X, pos.Y, Config.CellSize / 4 - 4, collectible.SecondaryColor);
	}

	public static void DrawTeleport(this Maze maze, Teleport teleport)
	{
		var pos = maze.MazeToScreenV(teleport.Position);
		var start = pos - Vector2.One * (Config.CellSize / 2.0f - 4);
		var end   = pos + Vector2.One * (Config.CellSize / 2.0f - 4);
		DrawRectangleV(start, Vector2.One * (Config.CellSize - 8), teleport.Color);
		DrawRectangleV(start + Vector2.One * 4.0f, Vector2.One * (Config.CellSize - 16), Color.BLACK with {a = 125});

		DrawRectangleLinesEx(new(start.X, start.Y, Config.CellSize - 8, Config.CellSize - 8), 1.0f, Color.RAYWHITE);
	}

	public static void DrawPath(this Maze maze, Point from, Point to, bool ignoreEnemies = false, params Player[] players)
	{
		var path = maze.FindPath(from, to, ignoreEnemies, true, players);
		path?.Aggregate(from, (point, dir) => 
		{
			var next = point + dir;
			var start = maze.MazeToScreenV(point);
			var end = maze.MazeToScreenV(next);
			if (Math.Abs(dir.Width) <= 1 && Math.Abs(dir.Height)<= 1)
				DrawLineV(start, end, Color.GRAY);
			return next;
		});
	}

	public static void DrawEnemy(this Maze maze, Enemy enemy)
	{
		var pos = maze.MazeToScreenV(enemy.Position);
		DrawCircleV(pos, Config.CellSize / 2 - 4, enemy.Color);
	}

	public static Point MazeToScreen(this Maze maze, Point position) => new(
		(int)((position.X - maze.Width / 2.0f + 0.5f) * Config.CellSize), 
		(int)((position.Y - maze.Height / 2.0f + 0.5f) * Config.CellSize));

	public static Vector2 MazeToScreenV(this Maze maze, Point position) => new(
		(position.X - maze.Width / 2.0f + 0.5f) * Config.CellSize, 
		(position.Y - maze.Height / 2.0f + 0.5f) * Config.CellSize);
	
	public static Point ScreenToMaze(this Maze maze, Vector2 point, Camera2D camera) => new(
		(int) Math.Round((point.X - camera.offset.X) / camera.zoom / Config.CellSize - 0.5f + maze.Width / 2.0f),
		(int) Math.Round((point.Y - camera.offset.Y) / camera.zoom / Config.CellSize - 0.5f + maze.Height / 2.0f)
	);
	
	public static void DrawTextRight(string text, int posX, int posY, int fontSize, Color color) =>
		DrawText(text, Config.WindowWidth - MeasureText(text, fontSize) - posX, posY, fontSize, color);
	
	public static void DrawTextCenter(string text, int posX, int posY, int fontSize, Color color) =>
		DrawText(text, (Config.WindowWidth - MeasureText(text, fontSize) + posX) / 2, posY, fontSize, color);
}