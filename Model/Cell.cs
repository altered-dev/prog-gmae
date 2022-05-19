using System.Drawing;

public class Cell 
{
	public Point Position { get; }

	public Direction Connections { get; set; }

	public Cell(Point position) => Position = position;

	public Cell(int x, int y) => Position = new(x, y);
}