using Labyrinth.Crawl;
using Labyrinth.Tiles;
using System.Text;

namespace Labyrinth
{
    public class Labyrinth
    {
        /// <summary>
        /// Labyrinth avec murs, portes et objets.
        /// </summary>
        /// <param name="ascii_map">Chaîne multi-lignes avec '+', '-' ou '|' pour les murs, '/' pour les portes, 'k' pour les clés et 'x' pour la position de départ.</param>
        /// <exception cref="ArgumentException">Incohérences de carte.</exception>
        /// <exception cref="NotSupportedException">Multiples portes/clés avant placement.</exception>
        public Labyrinth(string ascii_map)
        {
            var separators = "\n,\r\n".Split(',');
            var lines = ascii_map.Split(separators, StringSplitOptions.None);
            int? sx = null, sy = null;
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == 'x')
                    {
                        sx = x;
                        sy = y; 
                    }
                }
            }

            _startX = sx;
            _startY = sy;

            var cleaned = sx is null ? ascii_map : ascii_map.Replace('x', ' ');

            _tiles = Build.AsciiParser.Parse(cleaned);
            Width = _tiles.GetLength(0);
            Height = _tiles.GetLength(1);
            if (Width < 3 || Height < 3)
            {
                throw new ArgumentException("Labyrinth must be at least 3x3");
            }
        }

        /// <summary>
        /// Largeur (colonnes)
        /// </summary>
        public int Width { get; private init; }

        /// <summary>
        /// Hauteur (lignes)
        /// </summary>
        public int Height { get; private init; }

        /// <summary>
        /// Représentation ascii du labyrinthe.
        /// </summary>
        public override string ToString()
        {
            var res = new StringBuilder();

            for (int y = 0; y < _tiles.GetLength(1); y++)
            {
                for (int x = 0; x < _tiles.GetLength(0); x++)
                {
                    res.Append(_tiles[x, y] switch
                    {
                        Room => ' ',
                        Wall => '#',
                        Door => '/',
                        _ => throw new NotSupportedException("Unknown tile type")
                    });
                }
                res.AppendLine();
            }
            return res.ToString();
        }

        public ICrawler NewCrawler()
        {
            if (_startX is null || _startY is null)
            {
                throw new ArgumentException("No starting position 'x' defined in map");
            }
            return new Crawl.Crawler(_tiles, _startX.Value, _startY.Value);
        }

        private readonly Tile[,] _tiles;
        private readonly int? _startX;
        private readonly int? _startY;
    }
}
