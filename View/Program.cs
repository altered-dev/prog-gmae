// using prog_gmae.Model;
// using static Raylib_cs.Raylib;
// using static prog_gmae.View.Drawer;
// using static prog_gmae.Model.Control;
//
// #region initialization
//
// SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_WINDOW_RESIZABLE);
// InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowName);
//
// var camera = new Camera2D(Config.WindowCenter, Vector2.Zero, 0f, 1f);
// var cameraOffset = new Vector2();
// var isMultiplayer = Config.StartWithMultiplayer;
// var inputLocked = false;
// var playerCount = 1;
// var maze = CreateMaze(new(Config.InitialWidth, Config.InitialHeight), playerCount);
//
// #endregion
//
// void Input()
// {
// 	Config.ResetDirections.ProcessInput(deltaSize => 
// 		CreateMaze(deltaSize + new Size(maze.Width, maze.Height), playerCount));
//
// 	if (IsKeyPressed(KeyboardKey.KEY_M))
// 	{
// 		isMultiplayer = !isMultiplayer;
// 		CreateMaze(new(maze.Width, maze.Height), playerCount);
// 	}
// 	
// 	if (inputLocked)
// 		return;
//
// 	player1.WalkDirections.ProcessInput(direction => 
// 		maze.MovePlayer(player1, direction, isMultiplayer ? player2 : null));
//
// 	if (isMultiplayer) 
// 		player2.WalkDirections.ProcessInput(direction => 
// 			maze.MovePlayer(player2, direction, player1));
//
// 	if (IsKeyPressed(KeyboardKey.KEY_Z))
// 		player1.ChangeDirection();
// 	if (IsKeyPressed(KeyboardKey.KEY_SLASH))
// 		player2.ChangeDirection();
//
// 	camera.zoom += GetMouseWheelMove() * 0.1f;
// 	if (IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
// 		camera.offset = Config.WindowCenter + (cameraOffset += GetMouseDelta());
// }
//
// void Logic()
// {
// 	if (player1.TailLength < 0 || player2.TailLength < 0)
// 		inputLocked = true;
//
// 	if (!IsWindowResized()) 
// 		return;
// 	Config.WindowWidth = GetScreenWidth();
// 	Config.WindowHeight = GetScreenHeight();
// 	camera.offset = Config.WindowCenter;
// }
//
// void Draw()
// {
// 	BeginDrawing();
// 	BeginMode2D(camera);
//
// 	if (isMultiplayer)
// 	{
// 		maze.DrawPath(player1.Position, maze.ScreenToMaze(GetMousePosition(), camera), players: new[] {player1, player2});
// 		maze.DrawPath(player2.Position, maze.ScreenToMaze(GetMousePosition(), camera), players: new[] {player1, player2});
// 	}
// 	else
// 		maze.DrawPath(player1.Position, maze.ScreenToMaze(GetMousePosition(), camera), players: player1);
//
// 	ClearBackground(Color.DARKGRAY);
// 	maze.DrawMaze();
// 	maze.DrawPlayer(player1);
// 	if (isMultiplayer)
// 		maze.DrawPlayer(player2);
//
// 	foreach (var enemy in maze.Enemies)
// 		maze.DrawEnemy(enemy);
//
// 	EndMode2D();
//
// 	DrawText($"score: {player1.Score}", 32, 32, 40, Color.GREEN);
// 	DrawText("wasd - move\nz - change direction\nr - reset\n-+ - change size\nm - toggle multiplayer\nmouse - move and zoom", 32, 84, 20, Color.LIGHTGRAY);
// 	if (isMultiplayer)
// 	{
// 		DrawTextRight($"score: {player2.Score}", 32, 32, 40, Color.BLUE);
// 		DrawTextRight("arrows - move", 32, 84, 20, Color.LIGHTGRAY);
// 		DrawTextRight("/ - change direction", 32, 114, 20, Color.LIGHTGRAY);
//
// 		if (player1.TailLength < 0)
// 			DrawTextCenter("blue wins", 0, Config.WindowHeight - 72, 40, Color.BLUE);
// 		else if (player2.TailLength < 0)
// 			DrawTextCenter("green wins", 0, Config.WindowHeight - 72, 40, Color.GREEN);
// 	}
// 	else if (player1.TailLength < 0)
// 		DrawTextCenter("you lose", 0, Config.WindowHeight - 72, 40, Color.LIGHTGRAY);
//
// 	EndDrawing();
// }
//
// while (!WindowShouldClose())
// {
// 	Input();
// 	Logic();
// 	Draw();
// }
//
// CloseWindow();