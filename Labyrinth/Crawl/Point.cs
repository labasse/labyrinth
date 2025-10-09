namespace Labyrinth.Crawl
{
    /// <summary>
    /// A point in 2D space.
    /// </summary>
    public record struct Point(int X, int Y)
    {
        /// <summary>
        /// Adds a direction to a point, resulting in a new point.
        /// </summary>
        /// <param name="point">The original point.</param>
        /// <param name="direction">The direction to add.</param>
        /// <returns>A new point with the direction added.</returns>
        public static Point operator +(Point point, Direction direction) =>
            new(point.X + direction.DeltaX, point.Y + direction.DeltaY);

        /// <summary>
        /// Subtracts a direction from a point, resulting in a new point.
        /// </summary>
        /// <param name="point">The original point.</param>
        /// <param name="direction">The direction to subtract.</param>
        /// <returns>A new point with the direction subtracted.</returns>
        public static Point operator -(Point point, Direction direction) =>
            new(point.X - direction.DeltaX, point.Y - direction.DeltaY);
    }
}