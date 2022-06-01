using System.Drawing;
using Raylib_cs;
using Color = Raylib_cs.Color;

public static class Control
{
	public static void Reset(Size size, bool multiplayer, ref Camera2D camera, ref bool inputLocked,
		out Maze maze, out Player player1, out Player player2, out bool isMultiplayer)
	{
		var random = new Random();
		isMultiplayer = multiplayer;
		maze = new(size.Width, size.Height, random.Next(3, 11), random.Next(2, 5));
		player1 = new(maze.GetRandomFreePoint(), Color.GREEN, Config.Player1WalkDirections);
		player2 = new(maze.GetRandomFreePoint(player1), Color.BLUE, Config.Player2WalkDirections);
		// camera.zoom = 16f / Math.Max(maze.Width, maze.Height);
	}

	public static void MovePlayer(this Maze maze, Player player, Direction direction, Player? otherPlayer = null)
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
}