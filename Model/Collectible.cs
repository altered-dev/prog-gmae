using System.Drawing;
using Color = Raylib_cs.Color;

public interface Collectible
{
	public Point Position { get; }
	public int TailLengthDelta { get; }
	public Color SecondaryColor { get; }
}