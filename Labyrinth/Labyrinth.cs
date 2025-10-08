using System.Linq;
using System.Text;

public class Labyrinth
{
    private Tile[,] grid;
    public Labyrinth(string s)
    {
        String[] tab = s.Split('\n');
        grid = new Tile[tab.Length, tab[0].Length];
        Key[] trousseau = new Key[5];
        int lenght = 0;
        int fifoIndexTrousseau = 0;
        for (int i = 0; i < tab.Length; i++)
        {
            for (int j = 0; j < tab[i].Length; j++)
            {
                if (tab[i][j] == '#')
                {
                    grid[i,j] = new Wall();
                }
                else if (tab[i][j] == ' ')
                {
                    grid[i,j] = new Room();
                }
                else if (tab[i][j] == '/')
                {
                    grid[i,j] = new Door();
                    trousseau[lenght] = new Key();
                    lenght++;
                }
                else
                {
                    Room r = new Room();
                    r.Item = trousseau[fifoIndexTrousseau];
                    fifoIndexTrousseau ++;
                    grid[i,j]= r;
                }
            }
        }
    }
    public override string ToString()
    {
        StringBuilder res = new StringBuilder();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                res.Append(grid[i, j].Character);
            }
            res.Append('\n');
        }
        return res.ToString();
    }
}