using Labyrinth;

string mazeString = """
+--+--------+
|  /        |
|  +--+--+  |
|     |k    |
+--+  |  +--+
   |k       |
+  +-------/|
|           |
+-----------+
""";

var labyrinth = new Labyrinth.Labyrinth(mazeString);

Console.WriteLine($"Labyrinth size: {labyrinth.Width}x{labyrinth.Height}");

var tile = labyrinth.GetTile(3, 1);
Console.WriteLine($"Tile at (3,1) is traversable: {tile.IsTraversable}");

if (tile is Door door)
{
    Console.WriteLine("Found a door!");
    Console.WriteLine($"Door is traversable: {door.IsTraversable}");
    
    door.Unlock(door.Key);
    Console.WriteLine($"After unlocking: {door.IsTraversable}");
}

// eh beh
