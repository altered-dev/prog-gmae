using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

#region initialization

InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowName);
var camera = new Camera2D(Vector2.Zero, -Config.WindowCenter, 0f, 1f);
var maze = new Maze(16, 16, 5);
var directions = Config.WalkDirections.Values.ToList();

#endregion

#region main game loop

while (!WindowShouldClose())
{
	foreach (var (key, direction) in Config.WalkDirections)
		if (IsKeyPressed(key) && (maze[maze.Player.Position].Connections & direction) != 0)
			maze.Player.Move(direction);
	
	foreach (var (key, deltaSize) in Config.ResetDirections)
		if (IsKeyPressed(key))
			maze = new Maze(maze.Width + deltaSize.Width, maze.Height + deltaSize.Height, 10);
	
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
	maze.DrawPlayer();
	foreach (var collectible in maze.Collectibles)
		maze.DrawCollectible(collectible);
	EndMode2D();

	DrawText($"Score: {maze.Score}\nArrows - move\nR - reset\nWASD - change size", 32, 32, 20, Color.WHITE);

	EndDrawing();
}

#endregion

CloseWindow();