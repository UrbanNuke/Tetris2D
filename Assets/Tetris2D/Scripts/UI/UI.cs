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

        #endregion

        #region LifeCycles

        private void Awake()
        {
            LineCleaner.onLineWasBurned += OnLineClear;
            linesWereBurnedText.text = "0";
            scoreText.text = "0";
            levelText.text = TetrisState.CurrentLevel.ToString();
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
            linesWereBurnedText.text = TetrisState.LinesWereBurned.ToString();

            TetrisState.TotalScore += TetrisState.GetScoreForBurnedLines(((LinesBurnedEvent)e).LinesBurnedCount);
            scoreText.text = TetrisState.TotalScore.ToString();

            levelText.text = TetrisState.CurrentLevel.ToString();
        }

        #endregion
    }
}