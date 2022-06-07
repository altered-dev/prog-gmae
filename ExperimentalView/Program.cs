using prog_gmae.ExperimentalModel;
using prog_gmae.ExperimentalView;
using static Raylib_cs.Raylib;

SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_WINDOW_RESIZABLE);
InitWindow(Config.WindowWidth, Config.WindowHeight, Config.WindowTitle);

var time = DateTime.Now;
IScene scene = new MazeScene(16, 16, 1);

while (!WindowShouldClose())
{
    scene.Update((DateTime.Now - time).TotalSeconds);
    
    scene.Draw();
}