using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

#region initialization

InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowName);

var camera = new Camera2D(Config.WindowCenter, Vector2.Zero, 0f, 1f);
var maze = new Maze(Config.InitialWidth, Config.InitialHeight, 
	Config.CollectibleCount, Config.TeleportCount);
var player = new Player(maze.GetRandomFreePoint());
var directions = Config.WalkDirections.Values.ToList();

#endregion

void Input()
{
	Config.WalkDirections.ProcessInput(direction =>
	{
		if ((maze[player.Position].Connections & direction) == 0)
			return;
		player.Move(direction);
		maze.TryTeleportPlayer(player);
		maze.TryCollect(player);
	});

	Config.ResetDirections.ProcessInput(deltaSize =>
	{
		maze = new(maze.Width + deltaSize.Width, maze.Height + deltaSize.Height, 
			Config.CollectibleCount, Config.TeleportCount);
		player = new(maze.GetRandomFreePoint());
		camera.zoom = 16f / Math.Max(maze.Width, maze.Height);
	});

	if (IsKeyPressed(KeyboardKey.KEY_SLASH))
		player.ChangeDirection();
}

void Logic()
{

}

void Draw()
{
	BeginDrawing();
	BeginMode2D(camera);

	ClearBackground(Color.DARKGRAY);
	maze.DrawMaze();
	maze.DrawPlayer(player);

	EndMode2D();

	DrawText($"score: {maze.Score}", 32, 32, 40, Color.GREEN);
	DrawText($"arrows - move\n/ - change direction\nr - reset\nwasd - change size", 32, 72, 20, Color.LIGHTGRAY);

	EndDrawing();
}

while (!WindowShouldClose())
{
	Input();
	Logic();
	Draw();
}

CloseWindow();