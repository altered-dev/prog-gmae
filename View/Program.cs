using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

#region initialization

InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowName);
var camera = new Camera2D(Vector2.Zero, -Config.WindowCenter, 0f, 1f);

var maze = new Maze(10, 10);
var mazeWidth = 10;
var mazeHeight = 10;

#endregion

#region main game loop

while (!WindowShouldClose())
{
	foreach (var (key, direction) in Config.WalkDirections)
		if (IsKeyPressed(key) && (maze[maze.Character.Position].Connections & direction) != 0)
			maze.Character.Move(direction);
	
	foreach (var (key, deltaSize) in Config.ResetDirections)
		if (IsKeyPressed(key))
			maze = new Maze(mazeWidth += deltaSize.Width, mazeHeight += deltaSize.Height);

	BeginDrawing();
	BeginMode2D(camera);
	ClearBackground(Color.WHITE);
	maze.DrawMaze();
	maze.DrawCharacter();
	EndMode2D();

	if (maze.Character.Position.X == mazeWidth - 1 && maze.Character.Position.Y == mazeHeight - 1)
		DrawText("You win!", 32, 32, 20, Color.GREEN);
	else
		DrawText("Get to the bottom right\nArrows - move\nR - reset\nWASD - change size", 32, 32, 20, Color.DARKGRAY);

	EndDrawing();
}

#endregion

CloseWindow();