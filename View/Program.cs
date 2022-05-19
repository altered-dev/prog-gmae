using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Drawing;
using Color = Raylib_cs.Color;

#region initialization

InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowName);
var camera = new Camera2D(Vector2.Zero, -Config.WindowCenter, 0f, 1f);

var maze = new Maze(10, 10);
var character = new Character();
var mazeWidth = 10;
var mazeHeight = 10;

#endregion

#region main game loop

while (!WindowShouldClose())
{
	if (IsKeyPressed(KeyboardKey.KEY_RIGHT) && (maze[character.Position].Connections & Direction.Right) != 0)
		character.Position = character.Position with { X = character.Position.X + 1 };
	if (IsKeyPressed(KeyboardKey.KEY_UP) && (maze[character.Position].Connections & Direction.Up) != 0)
		character.Position = character.Position with { Y = character.Position.Y - 1 };
	if (IsKeyPressed(KeyboardKey.KEY_LEFT) && (maze[character.Position].Connections & Direction.Left) != 0)
		character.Position = character.Position with { X = character.Position.X - 1 };
	if (IsKeyPressed(KeyboardKey.KEY_DOWN) && (maze[character.Position].Connections & Direction.Down) != 0)
		character.Position = character.Position with { Y = character.Position.Y + 1 };
	if (IsKeyPressed(KeyboardKey.KEY_R))
	{
		maze = new Maze(mazeWidth, mazeHeight);
		character.Position = Point.Empty;
	}
	if (IsKeyPressed(KeyboardKey.KEY_D))
	{
		maze = new Maze(++mazeWidth, mazeHeight);
		character.Position = Point.Empty;
	}
	if (IsKeyPressed(KeyboardKey.KEY_A))
	{
		maze = new Maze(--mazeWidth, mazeHeight);
		character.Position = Point.Empty;
	}
	if (IsKeyPressed(KeyboardKey.KEY_W))
	{
		maze = new Maze(mazeWidth, --mazeHeight);
		character.Position = Point.Empty;
	}
	if (IsKeyPressed(KeyboardKey.KEY_S))
	{
		maze = new Maze(mazeWidth, ++mazeHeight);
		character.Position = Point.Empty;
	}

	BeginDrawing();
	BeginMode2D(camera);
	ClearBackground(Color.WHITE);
	foreach (var cell in maze.Cells)
		if (cell != null)
			maze.DrawCell(cell);
	maze.DrawCharacter(character);
	EndMode2D();

	if (character.Position.X == mazeWidth - 1 && character.Position.Y == mazeHeight - 1)
		DrawText("You win!", 32, 32, 20, Color.GREEN);
	else
		DrawText("Get to the bottom right", 32, 32, 20, Color.DARKGRAY);

	EndDrawing();
}

#endregion

CloseWindow();