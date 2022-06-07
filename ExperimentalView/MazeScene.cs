using prog_gmae.ExperimentalModel;
using static Raylib_cs.Raylib;

namespace prog_gmae.ExperimentalView;

public class MazeScene : IScene
{
    private Maze maze;
    private readonly Camera2D camera = new(Config.WindowCenter, Vector2.Zero, 0f, 1f);
    
    Camera2D IScene.Camera => camera;

    public MazeScene(int mazeWidth, int mazeHeight, int playerCount)
    {
        maze = new(mazeWidth, mazeHeight, playerCount);
    }


    public void Update(double delta)
    {
        foreach (var player in maze.Players)
        {
            var playerMoved = false;
            player.Controls.HandleInput(
                dir => playerMoved |= player.Move(dir, maze[player.Position]!), player.TurnAround);
            if (playerMoved && player.TryCollect(maze[player.Position]!))
                maze.AddFood();

        }
        
    }

    public void Draw()
    {
        BeginDrawing();
        ClearBackground(Color.DARKGRAY);
        BeginMode2D(camera);
        
        maze.DrawMaze();
        
        EndMode2D();
        EndDrawing();
    }
}