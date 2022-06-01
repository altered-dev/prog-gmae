using System.Drawing;
using Color = Raylib_cs.Color;

public class Teleport
{
	public static readonly List<Color> Colors = new()
		{ Color.BEIGE, Color.DARKBLUE, Color.GOLD, Color.MAGENTA };

	public Point Position { get; }
	public Teleport? Link { get; private set; }
	public Color Color { get; private set; }

	public Teleport(Point position) => Position = position;

	public void LinkTo(Teleport other, Color color)
	{
		Link = other;
		other.Link = this;
		Color = other.Color = color;
	}

	public void MovePlayer(Player player)
	{
		if (Link == null || player.Contains(Link.Position))
			return;
		player.Position = Link.Position;
	}
}