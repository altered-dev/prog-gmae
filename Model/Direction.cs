public enum Direction
{ 
	None = 0,
	Right = 1, 
	Up = 2, 
	Left = 4, 
	Down = 8,
	Horizontal = Left | Right,
	Vertical = Up | Down,
	All = Right | Up | Left | Down,
}