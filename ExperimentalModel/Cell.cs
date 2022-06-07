namespace prog_gmae.ExperimentalModel;

public class Cell
{
    private Direction connections = Direction.None;
    private Food food = Food.None;
    private readonly HashSet<Teleport> teleports = new();
    public readonly Color Color = Color.WHITE with { a = (byte) new Random().Next(10) };

    public IEnumerable<Teleport> Teleports => teleports.AsEnumerable();

    public bool HasConnection(Direction direction) => (connections & direction) != 0;

    public void Connect(Direction direction) => connections |= direction;

    public void Disconnect(Direction direction) => connections &= ~direction;

    public bool HasTeleport(Direction direction, out Point destination)
    {
        var teleport = teleports.FirstOrDefault(t => (t.Direction & direction) != 0);
        destination = teleport?.Destination ?? Point.Empty;
        return teleport != null;
    }

    public bool HasTeleport(Direction direction, out Color color)
    {
        var teleport = teleports.FirstOrDefault(t => (t.Direction & direction) != 0);
        color = teleport?.Color ?? Color.BLANK;
        return teleport != null;
    }

    public bool AddTeleport(Direction direction, Point destination, Color color)
    {
        if (teleports.Any(t => (t.Direction & direction) != 0))
            return false;
        teleports.Add(new(direction, destination, color));
        return true;
    }

    public void SpawnFood() => 
        food = Config.GetChance(Config.FoodBalance)
            ? Food.Extending 
            : Food.Shortening;

    public bool HasFood() => food != Food.None;

    public bool HasFood(out int deltaLength) => (deltaLength = (int) food) != 0;
    
    public bool TryCollect(out int deltaLength)
    {
        var success = food != Food.None;
        deltaLength = (int) food;
        food = Food.None;
        return success;
    }
}