using System.Collections;

namespace prog_gmae.ExperimentalModel;

public record Player(Point Position, Color Color, Controls Controls) : IEnumerable<Point>
{
    private LinkedList<Point> tail = new();
    private Point position = Position;

    public Point Position
    {
        get => position;
        private set
        {
            if (Contains(value))
                return;
            tail.AddFirst(position);
            while (tail.Count > TailLength)
                tail.RemoveLast();
            position = value;
        }
    }

    public int TailLength { get; private set; }
    
    public int Score { get; private set; }

    public bool Move(Direction direction, Cell cell)
    {
        if (cell.HasTeleport(direction, out Point destination))
        {
            Position = destination;
            return true;
        }
        if (!cell.HasConnection(direction)) 
            return false;
        Position += direction.ToSize();
        return true;
    }

    public void TurnAround()
    {
        if (tail.Count == 0)
            return;
        tail = new(tail.Reverse());
        tail.AddLast(position);
        position = tail.First!.Value;
        tail.RemoveFirst();
    }

    public bool TryCollect(Cell cell)
    {
        if (!cell.TryCollect(out var deltaLength)) 
            return false;
        Score++;
        TailLength += deltaLength;
        return true;
    }

    public bool Contains(Point point) => point == position || tail.Contains(point);
    
    public IEnumerator<Point> GetEnumerator() => tail.Prepend(Position).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}