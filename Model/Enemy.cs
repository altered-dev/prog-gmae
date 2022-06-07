namespace prog_gmae.Model;

public record Enemy(Point Position, Color Color) : IMovable
{
	public Point Position { get; set; } = Position;

	public void MoveTowards(Maze maze, params Player[] players)
	{
		var path = players
			.Select(player => maze.FindPath(Position, player.Position, true))
			.MinBy(path => path?.Count ?? int.MaxValue);
		if (path == null)
			return;
		var newPos = Position + path.FirstOrDefault();
		if (maze.Enemies.Any(e => e.Position == newPos))
			return;
		var player = players.FirstOrDefault(player => player.Contains(newPos));
		if (player != null)
			player.TailLength = -1;
		var collectible = maze.Collectibles.FirstOrDefault(c => c.Position == newPos);
		if (collectible != null)
		{
			maze.Collectibles.Remove(collectible);
			maze.AddCollectible();
		}
		Position = newPos;
	}
}