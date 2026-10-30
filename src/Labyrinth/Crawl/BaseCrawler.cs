using Labyrinth.Items;
using Labyrinth.Tiles;


namespace Labyrinth.Crawl
{
    public class BaseCrawler : ICrawler
    {
        public static Direction defaultDirection => Direction.North;
        public Inventory Inventory { get; } = new MyInventory();

        private readonly Tile[,] _tiles;

        public BaseCrawler(int x, int y, Direction direction, Tile[,] tiles)
        {
            X = x;
            Y = y;
            Direction = direction;
            _tiles = tiles;
            Inventory = new MyInventory();
        }

        public int X { get; private set; }
        public int Y { get; private set; }


        public Direction Direction { get; private set; }

        public Tile FacingTile
        {
            get
            {
                int facingY = Y + Direction.DeltaY;
                int facingX = X + Direction.DeltaX;

                return IsOutside(facingX, facingY)
                    ? Outside.Singleton
                    : _tiles[facingX, facingY];
            }
        }

        public Inventory Walk()
        {
            // first, we try to open the door
            tryOpenDoor();

            try
            { 
                var inventory = FacingTile.Pass();   // lève une exception si la porte est restée fermée
                X += Direction.DeltaX;
                Y += Direction.DeltaY;
                return inventory;
            }
            catch (InvalidOperationException)
            {
                return _tiles[X, Y].Pass(); // return the inventory of the current tile if the door is locked and cannot be opened
            }
        }

        private void tryOpenDoor()
        {
            if (FacingTile is Door door)
            {
                try
                {
                    door.Open(Inventory);
                }
                catch (InvalidOperationException)
                {
                    // The door is already opened, do nothing
                }
            }
        }

        private bool IsOutside(int x, int y) => x < 0 || y < 0 || x >= _tiles.GetLength(0) || y >= _tiles.GetLength(1);
    }
}