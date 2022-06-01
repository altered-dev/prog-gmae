using System.Drawing;
using Color = Raylib_cs.Color;

public class BadCollectible : Collectible
{
	public Point Position { get; }
	public int TailLengthDelta { get; } = -1;
	public Color SecondaryColor { get; } = new(0, 0, 0, 125);

	public BadCollectible(Point position) => Position = position;
}