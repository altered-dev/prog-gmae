namespace prog_gmae.Model;

public record Teleport(Point Position)
{
	private static readonly List<Color> colors = new()
		{ Color.BEIGE, Color.DARKBLUE, Color.GOLD, Color.MAGENTA, Color.LIME };
	public static readonly int MaxTeleports = colors.Count;

	public Teleport? Link { get; private set; }
	public Color Color { get; private set; }

	public void LinkTo(Teleport other, int i)
	{
		Link = other;
		other.Link = this;
		Color = other.Color = colors[i];
	}

	public bool MovePlayer(Player player)
	{
		if (Link == null || player.Contains(Link.Position))
			return false;
		// player.Position = Link.Position;
		return true;
	}
}