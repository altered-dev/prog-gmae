using prog_gmae.ExperimentalModel;
using static Raylib_cs.Raylib;

namespace prog_gmae.ExperimentalView;

public static class Drawer
{
    public static void DrawMaze(this Maze maze)
    {
        for (var x = 0; x < maze.Width; x++)
        for (var y = 0; y < maze.Height; y++)
            maze.DrawCell(x, y);
        foreach (var player in maze.Players)
            maze.DrawPlayer(player);
    }

    public static void DrawCell(this Maze maze, int x, int y)
    {
        var cell = maze[x, y];
        if (cell == null)
            return;
        var pos = maze.MazeToScreenV(new(x, y)) + Vector2.One * 0.5f;
        var right = pos.X + Config.CellSize / 2f;
        var up    = pos.Y - Config.CellSize / 2f;
        var left  = pos.X - Config.CellSize / 2f;
        var down  = pos.Y + Config.CellSize / 2f;

        DrawRectangleRec(new(left, up, right - left, down - up), cell.Color);

        foreach (var teleport in cell.Teleports)
            DrawTeleport(left + 1, right - 1, up + 1, down - 1, teleport);
        
        if (cell.HasFood(out var deltaLength))
            DrawFood(pos, deltaLength);

        if (!cell.HasConnection(Direction.Right))
            DrawLineV(new(right, up), new(right, down), Color.LIGHTGRAY);
        if (!cell.HasConnection(Direction.Up))
            DrawLineV(new(left, up), new(right, up), Color.LIGHTGRAY);
        if (!cell.HasConnection(Direction.Left))
            DrawLineV(new(left, up), new(left, down), Color.LIGHTGRAY);
        if (!cell.HasConnection(Direction.Down))
            DrawLineV(new(left, down), new(right, down), Color.LIGHTGRAY);
    }

    public static void DrawPlayer(this Maze maze, Player player)
    {
        var radius = Config.CellSize / 2f - 4;
        foreach (var point in player)
        {
            DrawCircleV(maze.MazeToScreenV(point), radius, player.Color);
            radius -= Config.CellSize * 0.25f / (player.TailLength + 1);
        }
    }

    public static void DrawTeleport(float left, float right, float up, float down, Teleport teleport)
    {
        switch (teleport.Direction)
        {
            case Direction.Right:
                DrawLineEx(new(right, up), new(right, down), 2f, teleport.Color);
                break;
            case Direction.Up:
                DrawLineEx(new(left, up), new(right, up), 2f, teleport.Color);
                break;
            case Direction.Left:
                DrawLineEx(new(left, up), new(left, down), 2f, teleport.Color);
                break;
            case Direction.Down:
                DrawLineEx(new(left, down), new(right, down), 2f, teleport.Color);
                break;
        }
    }

    public static void DrawFood(Vector2 point, int deltaSize)
    {
        DrawCircleV(point, Config.CellSize / 4f, Color.ORANGE);
        DrawCircleV(point, Config.CellSize / 8f, (deltaSize < 0 ? Color.BLACK : Color.WHITE) with {a = 125});
    }
    
    public static Vector2 MazeToScreenV(this Maze maze, Point position) => new(
        (position.X - maze.Width / 2.0f + 0.5f) * Config.CellSize, 
        (position.Y - maze.Height / 2.0f + 0.5f) * Config.CellSize);
}