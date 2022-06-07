namespace prog_gmae.Model;

public static class Control
{
	public static Maze CreateMaze(Size size, int playerCount)
	{
		var random = new Random();
		return new(size.Width, size.Height, playerCount, random.Next(size.Width / 4, size.Width + 1), 
			random.Next(Math.Min(size.Height / 8, Teleport.MaxTeleports), 
				Math.Min(size.Height / 2, Teleport.MaxTeleports) + 1),
			random.Next(size.Width / 16, size.Width / 4));
	}

	public static void MovePlayer(this Maze maze, Player player, Direction direction, Player? otherPlayer = null)
	{
		if (!(player.Position + direction.ToCoords()).IsInBounds(maze.Width, maze.Height))
			return;
		if ((maze[player.Position]?.Connections & direction) == 0)
		{
			if (!Config.DiggerMode)
				return;
			maze.RemoveWall(player.Position, direction);
		}
		// == true because the result of the expression is "bool?"
		if (otherPlayer?.Contains(player.Position + direction.ToCoords()) == true)
			return;
		player.Position += direction.ToCoords();
		maze.TryTeleportPlayer(player);
		if (otherPlayer != null)
			maze.TryCollect(player, otherPlayer);
		else
			maze.TryCollect(player);
		var random = new Random();
		foreach (var enemy in maze.Enemies.Where(_ => random.Next(10) < Config.EnemySpeed))
			if (otherPlayer != null)
				enemy.MoveTowards(maze, player, otherPlayer);
			else
				enemy.MoveTowards(maze, player);
	}
}