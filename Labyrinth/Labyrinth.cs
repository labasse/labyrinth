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
            // Delegate parsing (including recherche du 'x') au parseur ASCII
            _tiles = Build.AsciiParser.Parse(ascii_map, out int? sx, out int? sy);
            _startX = sx;
            _startY = sy;
            Width = _tiles.GetLength(0);
            Height = _tiles.GetLength(1);
            if (Width < 3 || Height < 3)
            {
                throw new ArgumentException("Labyrinth must be at least 3x3");
            }
        }


        public int Width { get; private init; }


        public int Height { get; private init; }


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
