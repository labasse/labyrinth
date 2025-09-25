public class Labyrinth
{
    private Tile[][] grid;
    public Labyrinth(string s)
    {
        String[] tab = s.Split('\n');
        grid = new Tile[tab.Length][];
        Key[] trousseau = new Key[5];
        int lenght = 0;
        int fifoIndexTrousseau = 0;
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
                else if (tab[i][j] == '/')
                {
                    grid[i][j] = new Door();
                    trousseau[lenght] = new Key("2");
                    lenght += 1;
                }
                else
                {
                    Room r = new Room();
                    r.Item = trousseau[fifoIndexTrousseau];
                    fifoIndexTrousseau += 1;
                    grid[i][j]= r;
                }
            }
        }
    }
    public String toString()
    {
        String res = "";
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                res += grid[i][j].Character;
            }
            res += "\n";
        }
        return res;
    }
}