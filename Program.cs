using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

#region initialization

InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowName);
var camera = new Camera2D(Vector2.Zero, -Config.WindowCenter, 0f, 1f);

#endregion

#region main game loop

while (!WindowShouldClose())
{
	BeginDrawing();
	BeginMode2D(camera);
	ClearBackground(Color.WHITE);
	DrawPixel(0, 0, Color.RED);
	EndMode2D();
	EndDrawing();
}

#endregion

CloseWindow();