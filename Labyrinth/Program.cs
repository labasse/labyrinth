// See https://aka.ms/new-console-template for more information
var multiline = """
        +--+--------+
        |  /        |
        |  +--+--+  |
        |     |k    |
        +--+  |  +--+
        |  |k       |
        +  +-------/|
        |           |
        +-----------+
        """;

var labyrinth = new Labyrinth.Labyrinth(multiline);

Console.WriteLine(labyrinth);