using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris2D
{
    public class UI : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Text field to show amount of burned lines
        /// </summary>
        [SerializeField]
        private Text linesWereBurnedText;

        /// <summary>
        /// Text field to show total player score
        /// </summary>
        [SerializeField]
        private Text scoreText;

        /// <summary>
        /// Text field to show current level of game
        /// </summary>
        [SerializeField]
        private Text levelText;

        /// <summary>
        /// Start button
        /// </summary>
        [SerializeField]
        private Button startButton;
        
        /// <summary>
        /// Pause button
        /// </summary>
        [SerializeField]
        private Button pauseButton;
        
        /// <summary>
        /// Pause text
        /// </summary>
        [SerializeField]
        private Text pauseText;
        
        /// <summary>
        /// GameOverText text
        /// </summary>
        [SerializeField]
        private Text gameOverText;
        
        /// <summary>
        /// Start game event
        /// </summary>
        public delegate void StartGameHandler();
        public static event StartGameHandler OnStartGame;

        #endregion

        #region LifeCycles

        private void Awake()
        {
            LineCleaner.onLineWasBurned += OnLineClear;
            linesWereBurnedText.text = "0";
            scoreText.text = "0";
            levelText.text = "1";
        }

        private void LateUpdate()
        {
            switch (GameManager.Instance.GameState)
            {
                case GameState.Play:
                    startButton.interactable = false;
                    pauseButton.interactable = true;
                    gameOverText.enabled = false;
                    break;
                case GameState.Pause:
                    break;
                case GameState.MainMenu:
                    startButton.interactable = true;
                    pauseButton.interactable = false;
                    break;
                case GameState.End:
                    startButton.interactable = true;
                    pauseButton.interactable = false;
                    gameOverText.enabled = true;
                    break;
                default:
                    break;
            }
        }

        #endregion
        
        #region PublicMethods

        /// <summary>
        /// Handle start button pressing 
        /// </summary>
        public void OnClickStart()
        {
            GameManager.Instance.Restart();
            linesWereBurnedText.text = "0";
            scoreText.text = "0";
            levelText.text = GameManager.Instance.CurrentLevel.ToString();
            GameManager.Instance.ChangeGameState(GameState.Play);
            OnStartGame?.Invoke();
        }

        /// <summary>
        /// Handle pause button pressing
        /// </summary>
        public void OnClickPause()
        {
            if (GameManager.Instance.GameState == GameState.Play)
            {
                this.pauseText.enabled = true;
                GameManager.Instance.ChangeGameState(GameState.Pause);
            }
            else
            {
                this.pauseText.enabled = false;
                GameManager.Instance.ChangeGameState(GameState.Play);
            }
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Handle <see cref="LinesBurnedEvent"/> in <see cref="LineCleaner"/>
        /// Display appropriate properties to UI
        /// </summary>
        /// <param name="sender">who sent event</param>
        /// <param name="e">event</param>
        private void OnLineClear(object sender, EventArgs e)
        {
            linesWereBurnedText.text = GameManager.Instance.LinesWereBurned.ToString();

            GameManager.Instance.TotalScore += GameManager.GetScoreForBurnedLines(((LinesBurnedEvent)e).LinesBurnedCount);
            scoreText.text = GameManager.Instance.TotalScore.ToString();

            levelText.text = GameManager.Instance.CurrentLevel.ToString();
        }

        #endregion
    }
}