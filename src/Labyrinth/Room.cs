namespace Labyrinth;

// peut contenir un collectable, éventuellement null
public class Room : Tile
{
    public override bool IsTraversable => true;

    public override void Pass() {

    }

    private ICollectable? _item;

    public Room(ICollectable? item = null)
    {
        _item = item;
    }
}