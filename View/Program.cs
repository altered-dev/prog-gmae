using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

#region initialization

InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowName);
var camera = new Camera2D(Config.WindowCenter, Vector2.Zero, 0f, 1f);
var maze = new Maze(16, 16, 5, 2);
var directions = Config.WalkDirections.Values.ToList();

#endregion

#region main game loop

while (!WindowShouldClose())
{
	foreach (var (key, direction) in Config.WalkDirections)
		if (IsKeyPressed(key) && (maze[maze.Player.Position].Connections & direction) != 0)
			{
				maze.Player.Move(direction);
				maze.TryTeleport(maze.Player.Position);
			}
	
	foreach (var (key, deltaSize) in Config.ResetDirections)
		if (IsKeyPressed(key))
		{
			maze = new Maze(maze.Width + deltaSize.Width, maze.Height + deltaSize.Height, 5, 2);
			camera.zoom = 16f / Math.Max(maze.Width, maze.Height);
		}
	
	if (IsKeyDown(KeyboardKey.KEY_Q))
	{
		var direction = directions.PickRandom();
		if ((maze[maze.Player.Position].Connections & direction) != 0)
			maze.Player.Move(direction);
	}

	if (IsKeyPressed(KeyboardKey.KEY_SLASH))
		maze.Player.ChangeDirection();

	maze.TryCollect(maze.Player.Position);

	BeginDrawing();
	BeginMode2D(camera);
	ClearBackground(Color.DARKGRAY);
	maze.DrawMaze();
	foreach (var collectible in maze.Collectibles)
		maze.DrawCollectible(collectible);
	foreach (var teleport in maze.Teleports)
		maze.DrawTeleport(teleport);
	maze.DrawPlayer();
	EndMode2D();

	DrawText($"Score: {maze.Score}\nArrows - move\n/ - change snake direction\nR - reset\nWASD - change size", 32, 32, 20, Color.WHITE);

	EndDrawing();
}

#endregion

CloseWindow();