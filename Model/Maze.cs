using System.Drawing;
using Color = Raylib_cs.Color;

public class Maze
{
	public readonly int Width, Height;
	public int Score { get; private set; }

	private readonly Cell[,] cells;
	public readonly List<Teleport> Teleports = new();
	public readonly List<Collectible> Collectibles = new();

	private static readonly Random random = new();

	public Maze(int width, int height, int collectibleCount, int teleportCount)
	{
		Width = width < 1 ? 1 : width;
		Height = height < 1 ? 1 : height;
		cells = Generator.GenerateMaze(Width, Height);
		for (var i = 0; i < collectibleCount; i++)
			AddCollectible();
		for (var i = 0; i < teleportCount; i++)
			AddTeleports(Teleport.Colors[i]);
		for (var i = 0; i < random.Next(Width * 2); i++)
			RemoveWall(GetRandomFreePoint(), Direction.Down);
		for (var i = 0; i < random.Next(Height * 2); i++)
			RemoveWall(GetRandomFreePoint(), Direction.Right);
	}

	public Cell this[int x, int y] => cells[x, y];

	public Cell this[Point pos] => cells[pos.X, pos.Y];

	public bool IsCellOccupied(Point position, params Player[] players) => 
		players.Any(player => player.Contains(position)) ||
		Collectibles.Any(c => c.Position == position) ||
		Teleports.Any(t => t.Position == position);

	public Point GetRandomFreePoint(params Player[] players)
	{
		Point result;
		do result = new(random.Next(Width), random.Next(Height));
		while (IsCellOccupied(result, players));
		return result;
	}

	private void AddCollectible(params Player[] players) => 
		Collectibles.Add(Generator.CreateCollectible(GetRandomFreePoint(players)));

	private void AddTeleports(Color color)
	{
		var teleportA = new Teleport(GetRandomFreePoint());
		var teleportB = new Teleport(GetRandomFreePoint());
		Teleports.Add(teleportA);
		Teleports.Add(teleportB);
		teleportA.LinkTo(teleportB, color);
	}

	public void TryCollect(params Player[] players)
	{
		foreach (var player in players)
		{
			var collectible = Collectibles.Find(c => c.Position == player.Position);
			if (collectible == null)
				continue;

			Collectibles.Remove(collectible);
			AddCollectible(players);
			player.Score += 1;
			player.TailLength += collectible.TailLengthDelta;
		}
	}

	public void AddWall(Point position, Direction direction)
	{
		this[position].Connections &= ~direction;
		this[position + direction.ToCoords()].Connections &= ~direction.Reverse();
	}

	public void RemoveWall(Point position, Direction direction)
	{
		var newPos = position + direction.ToCoords();
		if (0 > newPos.X || newPos.X >= Width || 0 > newPos.Y || newPos.Y >= Height)
			return;
		this[position].Connections |= direction;
		this[newPos].Connections |= direction.Reverse();
	}

	public void TryTeleportPlayer(Player player) => Teleports
		.Find(t => t.Position == player.Position)?
		.MovePlayer(player);
}