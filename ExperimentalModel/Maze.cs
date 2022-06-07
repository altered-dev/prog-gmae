namespace prog_gmae.ExperimentalModel;

public partial class Maze
{
    public readonly int Width, Height;
    public readonly HashSet<Player> Players = new();
    private readonly Cell?[,] cells;
    private readonly Random random = new();

    public Cell? this[int x, int y] => cells[x, y];
    public Cell? this[Point position] => 
        position.IsInBounds(Width, Height) ? cells[position.X, position.Y] : null;
    
    public Maze(int width, int height, int playerCount)
    {
        Width = width;
        Height = height;
        cells = GenerateMaze(width, height);
        for (var i = 0; i < Math.Min(Config.TeleportColors.Length, random.Next(width)); i++)
            AddTeleports(i);
        for (var i = 0; i < playerCount; i++)
            Players.Add(new(GetRandomFreePoint(), Config.PlayerColors[i], Config.PlayerControls[i]));
        for (var i = 0; i < random.Next(Width / 8, Width); i++)
            AddFood();
    }

    public bool IsCellOccupied(Point position) => 
        !position.IsInBounds(Width, Height) || 
        this[position]?.HasFood() == true || 
        Players.Any(player => player.Contains(position));

    public Point GetRandomFreePoint()
    {
        Point point;
        do point = new(random.Next(Width), random.Next(Height));
        while (IsCellOccupied(point));
        return point;
    }

    public void AddFood() => this[GetRandomFreePoint()]!.SpawnFood();

    private void AddTeleports(int colorIndex)
    {
        Direction GetRandomDirection(Point point) => Config.Directions
            .Where(dir => this[point]?.HasConnection(dir) == false && 
                this[point]?.HasTeleport(dir, out Point _) == false)
            .DefaultIfEmpty(Direction.None)
            .ToList()
            .PickRandom();
        
        // TODO: oh god, this is shitcode
        while (true)
        {
            var pointA = GetRandomFreePoint();
            var pointB = GetRandomFreePoint();
            var directionA = GetRandomDirection(pointA);
            var directionB = GetRandomDirection(pointB);
            if (directionA == Direction.None || directionB == Direction.None)
                continue;
            var cellA = this[pointA]!;
            var cellB = this[pointB]!;
            cellA.AddTeleport(directionA, pointB, Config.TeleportColors[colorIndex]);
            cellB.AddTeleport(directionB, pointA, Config.TeleportColors[colorIndex]);
            break;
        }
    }
}