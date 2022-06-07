namespace prog_gmae.ExperimentalModel;

public readonly record struct Controls(Dictionary<KeyboardKey, Direction> Directions, KeyboardKey TurnAroundKey)
{
    public void HandleInput(Action<Direction> directionAction, Action turnAroundAction)
    {
        foreach (var (key, direction) in Directions)
            if (Raylib.IsKeyPressed(key))
                directionAction(direction);
        if (Raylib.IsKeyPressed(TurnAroundKey))
            turnAroundAction();
    }
}