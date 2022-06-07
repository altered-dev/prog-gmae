namespace prog_gmae.ExperimentalModel;

public partial class Maze
{
    private static Cell?[,] GenerateMaze(int width, int height)
    {
        var cells = new Cell?[width, height];
        var stack = new Stack<Point>();
        var visited = new HashSet<Point>();
        var random = new Random();
        var current = new Point(random.Next(width), random.Next(height));
        stack.Push(current);
        var directions = Config.DirectionSizes
            .OrderBy(_ => random.Next())
            .ToList();

        for (var visitedCount = 0; visitedCount < width * height;)
        {
            var candidates = directions
                .Select(dir => current + dir)
                .Where(pos => pos.IsInBounds(width, height) && !visited.Contains(pos))
                .ToList();
            if (!candidates.Any())
            {
                if (!stack.Any())
                    break;
                current = stack.Pop();
                continue;
            }
            var next = Config.RandomMaze ? candidates.PickRandom() : candidates[0];
            cells[current.X, current.Y]?.Connect(current.DirectionTo(next));
            (cells[next.X, next.Y] ??= new()).Connect(next.DirectionTo(current));
            stack.Push(next);
            if (Config.GetChance(Config.MazeDensity) && !visited.Contains(next))
            {
                visited.Add(next);
                if (Config.SquareMaze)
                    visitedCount++;
            }
            if (!Config.SquareMaze)
                visitedCount++;
            current = next;
        }

        return cells;
    }
}