var lab = new Labyrinth.Labyrinth("""
    +--+--------+
    |  /        |
    |  +--+--+  |
    |     |k    |
    +--+  |  +--+
       |k |     |
    +  +-------/|
    |           |
    +-----------+
    """);
    
Console.WriteLine($"{lab.Width} x {lab.Height}"); 