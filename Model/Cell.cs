namespace prog_gmae.Model;

public record Cell(Point Position)
{
	public Direction Connections { get; set; }
	public Color Color { get; } = new(255, 255, 255, new Random().Next(10));

	public Cell(int x, int y) : this(new Point(x, y)) {}
}