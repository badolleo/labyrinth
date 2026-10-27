using Labyrinth.Build;

Console.WriteLine(new Labyrinth.Labyrinth(new AsciiParser("""
    +--+--------+
    |  /        |
    |  +--+--+  |
    |     |k    |
    +--+  |  +--+
       |k  x    |
    +  +-------/|
    |           |
    +-----------+
    """)));