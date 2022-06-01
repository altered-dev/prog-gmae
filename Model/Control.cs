using System.Drawing;
using Raylib_cs;
using Color = Raylib_cs.Color;

public static class Control
{
	public static void Reset(Size size, bool multiplayer, ref Camera2D camera, ref bool inputLocked,
		out Maze maze, out Player player1, out Player player2, out bool isMultiplayer)
	{
		var random = new Random();
		inputLocked = false;
		isMultiplayer = multiplayer;
		maze = new(size.Width, size.Height, random.Next(size.Width / 4, size.Width + 1), 
			random.Next(Math.Min(size.Height / 8, Teleport.MaxTeleports), Math.Min(size.Height / 2, Teleport.MaxTeleports) + 1),
			random.Next(size.Width / 16, size.Width / 4));
		player1 = new(maze.GetRandomFreePoint(), Color.GREEN, Config.Player1WalkDirections);
		player2 = new(maze.GetRandomFreePoint(player1), Color.BLUE, Config.Player2WalkDirections);
		// camera.zoom = 16f / Math.Max(maze.Width, maze.Height);
	}

	public static void MovePlayer(this Maze maze, Player player, Direction direction, Player? otherPlayer = null)
	{
		if (!(player.Position + direction.ToCoords()).IsInBounds(maze.Width, maze.Height))
			return;
		if ((maze[player.Position].Connections & direction) == 0)
		{
			if (!Config.DiggerMode)
				return;
			maze.RemoveWall(player.Position, direction);
		}
		// == true because the result of the expression is "bool?"
		if (otherPlayer?.Contains(player.Position + direction.ToCoords()) == true)
			return;
		player.Move(direction);
		maze.TryTeleportPlayer(player);
		if (otherPlayer != null)
			maze.TryCollect(player, otherPlayer);
		else
			maze.TryCollect(player);
		var random = new Random();
		foreach (var enemy in maze.Enemies)
			if (random.Next(10) < Config.EnemySpeed)
			{
				if (otherPlayer != null)
					enemy.MoveTowards(maze, player, otherPlayer);
				else
					enemy.MoveTowards(maze, player);
			}
	}
}