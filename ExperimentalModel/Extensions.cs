namespace prog_gmae.ExperimentalModel;

public static class Extensions
{
    private static readonly Random random = new();
    
    public static bool IsInBounds(this Point point, int width, int height) =>
        0 <= point.X && point.X < width &&
        0 <= point.Y && point.Y < height;

    public static T PickRandom<T>(this List<T> list) => list[random.Next(list.Count)];

    public static Direction ToDirection(this Size size) => size switch
    {
        {Width: < 0} => Direction.Left,
        {Width: > 0} => Direction.Right,
        {Height: < 0} => Direction.Up,
        {Height: > 0} => Direction.Down,
        _ => Direction.None,
    };

    public static Direction DirectionTo(this Point a, Point b) =>
        new Size(b.X - a.X, b.Y - a.Y).ToDirection();

    public static Direction Reverse(this Direction direction)
    {
        return (Direction) ((int) ~direction % (1 << 4));
    }

    public static Size ToSize(this Direction direction) => direction switch
    {
        Direction.Right => new(1, 0),
        Direction.Up => new(0, -1),
        Direction.Left => new(-1, 0),
        Direction.Down => new(0, 1),
        _ => Size.Empty,
    };
}