using System.Drawing;
using Color = Raylib_cs.Color;

// TODO: this looks just like a wrapper around Point,
// might consider either moving some logic to here or removing this class
public class GoodCollectible : Collectible
{
	public Point Position { get; }
	public int TailLengthDelta { get; } = 1;
	public Color SecondaryColor { get; } = new(255, 255, 255, 125);

	public GoodCollectible(Point position) => Position = position;
}