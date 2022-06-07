namespace prog_gmae.ExperimentalModel;

[Flags] public enum Direction
{
    None  = 0,
    Right = 1 << 0,
    Up    = 1 << 1,
    Left  = 1 << 2,
    Down  = 1 << 3,
}