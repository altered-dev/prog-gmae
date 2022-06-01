using System.Drawing;
using Color = Raylib_cs.Color;

public class Cell 
{
	public Point Position { get; }
	public Direction Connections { get; set; }
	public Color Color { get; } = new(255, 255, 255, new Random().Next(10));

	public Cell(Point position) => Position = position;

	public Cell(int x, int y) => Position = new(x, y);
}