namespace Labyrinth;

public class Labyrinth
{

    private readonly Tile[,] _tiles;

    public int Width { get; }
    public int Height { get; }

    public Labyrinth(string map) 
    {
        string[] lines = map.Split("\n");
        
        Width = lines[0].Length;
        Height = lines.Length;

        _tiles = new Tile[Width, Height];

        var keys = new Queue<Key>();
        var keyRooms = new List<(int X, int Y)>();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                switch (lines[y][x])
                {
                    case '+' or '-' or '|':
                        _tiles[x, y] = new Wall();
                        break;
                    case ' ':
                        _tiles[x, y] = new Room();
                        break;
                    case '/':
                        var door = new Door();
                        keys.Enqueue(door.Key);
                        _tiles[x, y] = door;
                        break;
                    case 'k':
                        keyRooms.Add((x, y));
                        break;
                    default:
                        throw new ArgumentException($"Caractère inconnu : '{lines[y][x]}'");
                }
            }
        }

        if (keys.Count != keyRooms.Count)
        {
            throw new ArgumentException(
                $"Le plan contient {keys.Count} porte(s) mais {keyRooms.Count} clé(s)");
        }

        foreach (var (x, y) in keyRooms)
        {
            _tiles[x, y] = new Room(keys.Dequeue());
        }
    }
}