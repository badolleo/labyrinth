namespace Labyrinth;

// crée et expose la Key qui l’ouvre et la ferme.
public class Door : Tile
{
    public override bool IsTraversable => true;

    public Key Key { get; } = new Key();

    public override void Pass() {
        if (!IsTraversable)
        {
            throw new InvalidOperationException("The door is closed");
        }
    }
}