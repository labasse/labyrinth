namespace Labyrinth.Events;

///<summary>
/// Provides data for the event signaling the labyrinth's start position.
/// </summary>
/// <remarks>
/// Contains X and Y coordinates of the start point found in the ASCII labyrinth map.
/// Used with the <c>StartPositionFound</c> event in the parser.
/// </remarks>
/// <param name="x">The X coordinate of the start position.</param>
/// <param name="y">The Y coordinate of the start position.</param>
public class StartEventArgs(int x, int y) : EventArgs
{
    /// <summary>
    /// Gets or sets the X coordinate of the start position.
    /// </summary>
    public int X { get; set; } = x;
    
    /// <summary>
    /// Gets or sets the Y coordinate of the start position.
    /// </summary>
    public int Y { get; set; } = y;
}