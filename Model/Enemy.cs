using System.Drawing;
using Color = Raylib_cs.Color;

public class Enemy
{
	public Point Position { get; set; }
	public Color Color { get; set; }

	public Enemy(Point position, Color color)
	{
		Position = position;
		Color = color;
	}

	public void MoveTowards(Maze maze, params Player[] players)
	{
		var path = players
			.Select(player => maze.FindPath(Position, player.Position, true))
			.OrderBy(path => path?.Count ?? int.MaxValue)
			.FirstOrDefault();
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
			maze.AddCollectible(players);
		}
		Position = newPos;
	}
}