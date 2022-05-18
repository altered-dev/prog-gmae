public class Cell 
{
	public (int x, int y) Position { get; }

	public Direction Connections { get; set; }

	public Cell(int x, int y) => Position = (x, y);
}