namespace prog_gmae.ExperimentalView;

public interface IScene
{
    protected internal Camera2D Camera { get; }
    
    public void Update(double delta);

    public void Draw();
}