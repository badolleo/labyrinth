using Labyrinth.Items;
using Labyrinth.Tiles;



namespace Labyrinth.Crawl
{
    public class BaseCrawler : ICrawler
    {
        public static Direction defaultDirection = Direction.North;

        private readonly Tile[,] _tiles;

        public BaseCrawler(int x, int y, Direction direction, Tile[,] tiles)
        {
            X = x;
            Y = y;
            Direction = direction;
            _tiles = tiles;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Direction { get; private set; }

        public Tile FacingTile
        {
            get
            {
                int facingY = Y + Direction.DeltaY;

                if (facingY < 0)
                {
                    return Outside.Singleton;
                }
                return _tiles[X + Direction.DeltaX, facingY];
            }
        }

        public Inventory Walk()
        {
            throw new NotImplementedException();
        }

    }
}