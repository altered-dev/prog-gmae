using System.Numerics;
using Raylib_cs;
using System.Drawing;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

#region initialization

InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowName);

var camera = new Camera2D(Config.WindowCenter, Vector2.Zero, 0f, 1f);
var directions = Config.Player1WalkDirections.Values.ToList();
var inputLocked = false;
Random random;
Maze maze;
Player player1, player2;
bool isMultiplayer;
Reset(new(Config.InitialWidth, Config.InitialHeight), true);

#endregion

void MovePlayer(Player player, Direction direction, Player? otherPlayer = null)
{
	if ((maze[player.Position].Connections & direction) == 0)
		return;
	// == true because the result of the expression is "bool?"
	if (otherPlayer?.Contains(player.Position + direction.ToCoords()) == true)
		return;
	player.Move(direction);
	maze.TryTeleportPlayer(player);
	if (otherPlayer != null)
		maze.TryCollect(player, otherPlayer);
	else
		maze.TryCollect(player);
}

void Reset(Size size, bool multiplayer)
{
	inputLocked = false;
	random = new();
	isMultiplayer = multiplayer;
	maze = new(size.Width, size.Height, random.Next(3, 11), random.Next(2, 5));
	player1 = new(maze.GetRandomFreePoint(), Color.GREEN, Config.Player1WalkDirections);
	player2 = new(maze.GetRandomFreePoint(player1), Color.BLUE, Config.Player2WalkDirections);
	camera.zoom = 16f / Math.Max(maze.Width, maze.Height);
}

void Input()
{
	Config.ResetDirections.ProcessInput(deltaSize => Reset(deltaSize + new Size(maze.Width, maze.Height), isMultiplayer));

	if (IsKeyPressed(KeyboardKey.KEY_M))
		Reset(new(maze.Width, maze.Height), !isMultiplayer);
	
	if (inputLocked)
		return;

	player1.WalkDirections.ProcessInput(direction => MovePlayer(player1, direction, isMultiplayer ? player2 : null));

	if (isMultiplayer) 
		player2.WalkDirections.ProcessInput(direction => MovePlayer(player2, direction, player1));

	if (IsKeyPressed(KeyboardKey.KEY_Z))
		player1.ChangeDirection();
	if (IsKeyPressed(KeyboardKey.KEY_SLASH))
		player2.ChangeDirection();
}

void Logic()
{
	if (player1.TailLength < 0 || player2.TailLength < 0)
		inputLocked = true;
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
	DrawText($"wasd - move\nz - change direction\nr - reset\n-+ - change size", 32, 84, 20, Color.LIGHTGRAY);
	if (isMultiplayer)
	{
		var score = $"score: {player2.Score}";
		var width = MeasureText(score, 40);
		DrawText(score, Config.WindowWidth - width - 32, 32, 40, Color.BLUE);
		var controls = $"arrows - move";
		width = MeasureText(controls, 20);
		DrawText(controls, Config.WindowWidth - width - 32, 84, 20, Color.LIGHTGRAY);
		controls = $"/ - change direction";
		width = MeasureText(controls, 20);
		DrawText(controls, Config.WindowWidth - width - 32, 112, 20, Color.LIGHTGRAY);

		if (player1.TailLength < 0)
		{
			var text = "blue won";
			width = MeasureText(text, 40);
			DrawText(text, (int) Config.WindowCenter.X - width / 2, Config.WindowHeight - 72, 40, Color.BLUE);
		}
		else if (player2.TailLength < 0)
		{
			var text = "green won";
			width = MeasureText(text, 40);
			DrawText(text, (int) Config.WindowCenter.X - width / 2, Config.WindowHeight - 72, 40, Color.GREEN);
		}
	}
	else if (player1.TailLength < 0)
	{
		var text = "you lost";
		var width = MeasureText(text, 40);
		DrawText(text, (int) Config.WindowCenter.X - width / 2, Config.WindowHeight - 72, 40, Color.LIGHTGRAY);
	}

	EndDrawing();
}

while (!WindowShouldClose())
{
	Input();
	Logic();
	Draw();
}

CloseWindow();