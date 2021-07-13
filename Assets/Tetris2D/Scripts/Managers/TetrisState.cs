using System.Collections.Generic;
using UnityEngine;

namespace Tetris2D
{
    /// <summary>
    /// Global Tetris state
    /// </summary>
    public static class TetrisState
    {
        #region Fields

        /// <summary>
        /// Amount of column in the game field
        /// </summary>
        public const int GameWidth = 10;

        /// <summary>
        /// Amount of row in the game field
        /// </summary>
        public const int GameHeight = 20;

        /// <summary>
        /// Map from level to level speed
        /// </summary>
        private static readonly Dictionary<int, float> LevelSpeeds = new Dictionary<int, float>()
        {
            {1, 1.0f},
            {2, 0.8f},
            {3, 0.5f},
            {4, 0.3f},
            {5, 0.2f},
            {6, 0.08f},
            {7, 0.05f},
            {8, 0.035f}
        };

        /// <summary>
        /// Tetris grid for store in which "cell" exist a block. [x, y]
        /// </summary>
        public static Transform[,] Grid = new Transform[GameWidth, GameHeight];

        /// <summary>
        /// Cost of the one level
        /// </summary>
        private const int LevelCost = 15;

        #endregion

        #region Properties

        /// <summary>
        /// Total amount of lines which were burned
        /// </summary>
        public static int LinesWereBurned { get; set; } = 0;

        /// <summary>
        /// Total score of current game
        /// </summary>
        public static int TotalScore { get; set; } = 0;

        /// <summary>
        /// Current level
        /// </summary>
        public static int CurrentLevel { get; set; } = 1;

        /// <summary>
        /// Getter/Setter for figure's fall speed
        /// </summary>
        public static float FallSpeed { get; set; } = LevelSpeeds[CurrentLevel];

        /// <summary>
        /// Getter/Setter for force figure's fall speed. When player press down button
        /// </summary>
        public static float ForceFallSpeed { get; set; } = 0.013f;

        #endregion

        #region PublicMethods

        /// <summary>
        /// Get score for amount of burned lines
        /// </summary>
        /// <param name="linesCount">amount of burned lines</param>
        /// <returns>score</returns>
        public static int GetScoreForBurnedLines(int linesCount)
        {
            return linesCount switch
            {
                1 => 100,
                2 => 300,
                3 => 700,
                4 => 1500,
                _ => 0
            };
        }

        /// <summary>
        /// Increase level of game
        /// </summary>
        public static void IncreaseLevel()
        {
            if (LinesWereBurned / LevelCost >= CurrentLevel && CurrentLevel != LevelSpeeds.Count)
                FallSpeed = LevelSpeeds[++CurrentLevel];
        }

        #endregion
    }
}