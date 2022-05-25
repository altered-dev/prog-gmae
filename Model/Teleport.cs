using System.Drawing;

public class Teleport
{
	public Point Position { get; }
	public Teleport? Link { get; private set; }

	public Teleport(Point position) => Position = position;

	public void LinkTo(Teleport other)
	{
		Link = other;
		other.Link = this;
	}

	public void MovePlayer(Player player)
	{
		if (Link == null || player.Contains(Link.Position))
			return;
		player.Position = Link.Position;
	}
}