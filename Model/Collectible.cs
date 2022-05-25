using System.Drawing;

// TODO: this looks just like a wrapper around Point,
// might consider either moving some logic to here or removing this class
public class Collectible
{
	public Point Position { get; }

	public Collectible(Point position) => Position = position;
}