public class Labyrinth
{
    private Tile[][] grid;
    public Labyrinth(string s)
    {
        String[] tab = s.Split('\n');
        grid = new Tile[tab.Length][];
        Key[] trousseau = new Key[5];
        int id = 0;
        for (int i = 0; i < tab.Length; i++)
        {
            grid[i] = new Tile[tab[i].Length];
            for (int j = 0; j < tab[i].Length; j++)
            {
                if (tab[i][j] == '#')
                {
                    grid[i][j] = new Wall();
                }
                else if (tab[i][j] == ' ')
                {
                    grid[i][j] = new Room();
                }
                else
                {
                    grid[i][j] = new Door();
                    trousseau[id] = new Key("2");
                    id += 1;
                }
            }
        }
    }
}