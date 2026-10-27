namespace Labyrinth.Items
{
    /// <summary>
    /// Collectable key item in the labyrinth.
    /// </summary>
    public class Key : ICollectable
    {
        private readonly Guid _id = Guid.NewGuid();
    }
}