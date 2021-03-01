/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for Setting the UI in the game.
 * ******************************************/
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel, _pauseMenu, _highScore;
    [SerializeField] private Text _highScoreText, _lastScoreText, _currentScoreText, _scoreText;

    /// <summary>
    /// Sets the UI values for the main game.
    /// </summary>
    /// <param name="highScore"></param>
    /// <param name="lastScore"></param>
    /// <param name="currentScore"></param>
    /// <param name="HighScore"></param>
    public void SetGameOverPanel(uint highScore, uint lastScore, uint currentScore, bool HighScore)
    {
        _highScore.SetActive(HighScore);
         _gameOverPanel.SetActive(true);
        _highScoreText.text = "High Score: " + highScore;
        _lastScoreText.text = "Last Score: " + lastScore;
        _currentScoreText.text = "Current Score: " + currentScore;
    }

    /// <summary>
    /// Sets score text component.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="Score"></param>
    public void SetScoreText(string content, uint Score)
    {
        _scoreText.text = content + Score;
    }

    /// <summary>
    /// Disables the game over panel.
    /// </summary>
    public void DisablePanel()
    {
        _gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Enables and disables the pause menu.
    /// </summary>
    /// <param name="activate"></param>
    public void PauseMenu(bool activate)
    {
        _pauseMenu.SetActive(activate);
    }
}
