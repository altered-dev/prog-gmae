using System.Drawing;
using Color = Raylib_cs.Color;

public class Teleport
{
	private static readonly List<Color> colors = new()
		{ Color.BEIGE, Color.DARKBLUE, Color.GOLD, Color.MAGENTA };

	public Point Position { get; }
	public Teleport? Link { get; private set; }
	public Color Color { get; private set; }

	public Teleport(Point position) => Position = position;

	public void LinkTo(Teleport other, int i)
	{
		Link = other;
		other.Link = this;
		Color = other.Color = colors[i];
	}

	public void MovePlayer(Player player)
	{
		if (Link == null || player.Contains(Link.Position))
			return;
		player.Position = Link.Position;
	}
}