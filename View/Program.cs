using System.Numerics;
using Raylib_cs;
using System.Drawing;
using static Raylib_cs.Raylib;
using static Drawer;
using static Control;
using Color = Raylib_cs.Color;

#region initialization

SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_WINDOW_RESIZABLE);
InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowName);

var camera = new Camera2D(Config.WindowCenter, Vector2.Zero, 0f, 1f);
var directions = Config.Player1WalkDirections.Values.ToList();
var inputLocked = false;
Reset(new(Config.InitialWidth, Config.InitialHeight), Config.StartWithMultiplayer, ref camera, ref inputLocked, 
	out var maze, out var player1, out var player2, out var isMultiplayer);

#endregion

void Input()
{
	Config.ResetDirections.ProcessInput(deltaSize => Reset(deltaSize + new Size(maze.Width, maze.Height), isMultiplayer,
		ref camera, ref inputLocked, out maze, out player1, out player2, out isMultiplayer));

	if (IsKeyPressed(KeyboardKey.KEY_M))
		Reset(new(maze.Width, maze.Height), !isMultiplayer, ref camera, ref inputLocked,
			out maze, out player1, out player2, out isMultiplayer);
	
	if (inputLocked)
		return;

	player1.WalkDirections.ProcessInput(direction => maze.MovePlayer(player1, direction, isMultiplayer ? player2 : null));

	if (isMultiplayer) 
		player2.WalkDirections.ProcessInput(direction => maze.MovePlayer(player2, direction, player1));

	if (IsKeyPressed(KeyboardKey.KEY_Z))
		player1.ChangeDirection();
	if (IsKeyPressed(KeyboardKey.KEY_SLASH))
		player2.ChangeDirection();
}

void Logic()
{
	if (player1.TailLength < 0 || player2.TailLength < 0)
		inputLocked = true;

	if (IsWindowResized())
	{
		Config.WindowWidth = GetScreenWidth();
		Config.WindowHeight = GetScreenHeight();
		camera.offset = Config.WindowCenter;
	}
}

void Draw()
{
	BeginDrawing();
	BeginMode2D(camera);

	ClearBackground(Color.DARKGRAY);
	maze.DrawMaze();
	maze.DrawPlayer(player1);
	if (isMultiplayer)
		maze.DrawPlayer(player2);

	EndMode2D();

	DrawText($"score: {player1.Score}", 32, 32, 40, Color.GREEN);
	DrawText("wasd - move\nz - change direction\nr - reset\n-+ - change size", 32, 84, 20, Color.LIGHTGRAY);
	if (isMultiplayer)
	{
		DrawTextRight($"score: {player2.Score}", 32, 32, 40, Color.BLUE);
		DrawTextRight("arrows - move", 32, 84, 20, Color.LIGHTGRAY);
		DrawTextRight("/ - change direction", 32, 114, 20, Color.LIGHTGRAY);

		if (player1.TailLength < 0)
			DrawTextCenter("blue wins", 0, Config.WindowHeight - 72, 40, Color.BLUE);
		else if (player2.TailLength < 0)
			DrawTextCenter("green wins", 0, Config.WindowHeight - 72, 40, Color.GREEN);
	}
	else if (player1.TailLength < 0)
		DrawTextCenter("you lose", 0, Config.WindowHeight - 72, 40, Color.LIGHTGRAY);

	EndDrawing();
}

while (!WindowShouldClose())
{
	Input();
	Logic();
	Draw();
}

CloseWindow();