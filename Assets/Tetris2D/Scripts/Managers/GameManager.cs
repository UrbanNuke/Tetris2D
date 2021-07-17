using System.Collections.Generic;
using UnityEngine;

namespace Tetris2D
{

    /// <summary>
    /// Different kind of game states
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Play,
        Pause,
        End
    }
    /// <summary>
    /// Global Tetris state
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Container for all figures on a level
        /// </summary>
        [SerializeField]
        private GameObject figureContainer;

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
        private readonly Dictionary<int, float> LevelSpeeds = new Dictionary<int, float>()
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
        public Transform[,] Grid = new Transform[GameWidth, GameHeight];

        /// <summary>
        /// Cost of the one level
        /// </summary>
        private const int LevelCost = 15;

        #endregion

        #region Properties

        /// <summary>
        /// Instance GameManager
        /// </summary>
        public static GameManager Instance { get; private set; }
        
        /// <summary>
        /// Current game state
        /// </summary>
        public GameState GameState { get; private set; } = GameState.MainMenu; 

        /// <summary>
        /// Total amount of lines which were burned
        /// </summary>
        public int LinesWereBurned { get; set; } = 0;

        /// <summary>
        /// Total score of current game
        /// </summary>
        public int TotalScore { get; set; } = 0;

        /// <summary>
        /// Current level
        /// </summary>
        public int CurrentLevel { get; private set; } = 1;

        /// <summary>
        /// Getter/Setter for figure's fall speed
        /// </summary>
        public float FallSpeed { get; private set; }

        /// <summary>
        /// Getter for shift speed
        /// </summary>
        public float ShiftSpeed { get; } = 0.11f;

        /// <summary>
        /// Getter/Setter for force figure's fall speed. When player press down button
        /// </summary>
        public float ForceFallSpeed { get; } = 0.013f;

        #endregion

        #region LifeCycles

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            FallSpeed = LevelSpeeds[CurrentLevel];
        }

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
        public void IncreaseLevel()
        {
            if (LinesWereBurned / LevelCost >= CurrentLevel && CurrentLevel != LevelSpeeds.Count)
                FallSpeed = LevelSpeeds[++CurrentLevel];
        }

        /// <summary>
        /// Change current game state
        /// </summary>
        /// <param name="state">new state</param>
        public void ChangeGameState(GameState state) => GameState = state;

        public void Restart()
        {
            foreach (Transform child in figureContainer.transform)
            {
                Destroy(child.gameObject);
            }
            Grid = new Transform[GameManager.GameWidth, GameManager.GameHeight];
            CurrentLevel = 1;
            FallSpeed = LevelSpeeds[CurrentLevel];
        }

        #endregion
    }
}