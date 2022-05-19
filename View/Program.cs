using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

#region initialization

InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowName);
var camera = new Camera2D(Vector2.Zero, -Config.WindowCenter, 0f, 1f);

var maze = new Maze(10, 10);

var directions = Config.WalkDirections.Values.ToList();

#endregion

#region main game loop

while (!WindowShouldClose())
{
	foreach (var (key, direction) in Config.WalkDirections)
		if (maze.CharacterInBounds() && IsKeyPressed(key) 
			&& (maze[maze.Character.Position].Connections & direction) != 0)
			maze.Character.Move(direction);
	
	foreach (var (key, deltaSize) in Config.ResetDirections)
		if (IsKeyPressed(key))
			maze = new Maze(maze.Width + deltaSize.Width, maze.Height + deltaSize.Height);
	
	if (IsKeyDown(KeyboardKey.KEY_Q) && maze.CharacterInBounds())
	{
		var direction = directions.PickRandom();
		if ((maze[maze.Character.Position].Connections & direction) != 0)
			maze.Character.Move(direction);
	}

	BeginDrawing();
	BeginMode2D(camera);
	ClearBackground(Color.DARKGRAY);
	maze.DrawMaze();
	maze.DrawCharacter();
	EndMode2D();

	if (!maze.CharacterInBounds())
		DrawText("You win!", 32, 32, 20, Color.GREEN);
	else
		DrawText("Get to the exit\nArrows - move\nR - reset\nWASD - change size", 32, 32, 20, Color.WHITE);

	EndDrawing();
}

#endregion

CloseWindow();