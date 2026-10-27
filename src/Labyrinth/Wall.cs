namespace Labyrinth;

// Ne peut jamais être traversé
public class Wall : Tile
{
    public override bool IsTraversable => false;

    public override void Pass()
    {
        throw new InvalidOperationException("Cannot pass a wall");
    }
}