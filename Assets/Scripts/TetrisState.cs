using UnityEngine;

public static class TetrisState
{
    /// <summary>
    /// Amount of column in the game field
    /// </summary>
    public const int GameWidth = 10;
    
    /// <summary>
    /// Amount of row in the game field
    /// </summary>
    public const int GameHeight = 20;

    /// <summary>
    /// Getter/Setter for figure's fall speed
    /// </summary>
    public static float FallSpeed { get; set; } = 0.4f;

    /// <summary>
    /// Tetris grid for store in which "cell" exist a block. [x, y]
    /// </summary>
    public static Transform[,] Grid = new Transform[GameWidth, GameHeight];
}