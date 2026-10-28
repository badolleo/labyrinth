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
            Direction = (Direction)direction.Clone();
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
                int facingX = X + Direction.DeltaX;

                return IsOutside(facingX, facingY)
                    ? Outside.Singleton
                    : _tiles[facingX, facingY];
            }
        }

        public Inventory Walk()
        {
            throw new NotImplementedException();
        }

        private bool IsOutside(int x, int y) => x < 0 || y < 0 || x >= _tiles.GetLength(0) || y >= _tiles.GetLength(1);

    }
}