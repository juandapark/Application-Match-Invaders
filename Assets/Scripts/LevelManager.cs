/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for the level.
 * ******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    GameStart,
    GameOver,
    GameReset
}

/// <summary> Manages the state of the level </summary>
public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelManager>();
            }
            return _instance;
        }
    }

    public static Action<GameState> OnGameStateChange; //Fires event of game state.

    public uint Score { get; private set; }
    public GameDifficulty CurrentDifficulty;

    [SerializeField] private GameState _state;

    private int _currentLives = 3;
    [SerializeField] private GameObject _livesPrefab;
    [SerializeField] private Transform _livesTrnasform;
    [SerializeField] private List<GameObject> _lives;

    private LevelUI _ui;
    private bool _gameIsPaused = false;
    private bool _newHighScore = false;

    /// <summary>
    /// Get components and sets the number of lives.
    /// </summary>
    private void Awake()
    {
        _ui = GetComponent<LevelUI>();
        _currentLives = CurrentDifficulty.NumberOfLives;

        for (int i = 0; i < _currentLives; i++)
        {
            GameObject live = Instantiate(_livesPrefab, _livesTrnasform.position, Quaternion.identity);
            live.transform.parent = _livesTrnasform;
            _lives.Add(live);
        }
    }

    /// <summary>
    /// Wait for transition before start.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(GameManager.Instance ? GameManager.Instance.TransitionTime : 0.5f); 
        StartGame();
    }

    /// <summary>
    /// Starts the Game.
    /// </summary>
    public void StartGame()
    {
        _state = GameState.GameStart;
        _newHighScore = false;
        if (OnGameStateChange != null)
        {
            OnGameStateChange.Invoke(_state);
        }
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void PauseGame()
    {
        if(_gameIsPaused)
        {
            _ui.PauseMenu(false);
            Time.timeScale = 1;
            _gameIsPaused = false;
            _ui.PauseMenu(false);
        }
        else
        {
            _ui.PauseMenu(true);
            _gameIsPaused = true;
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Get input to pause game.
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    /// <summary>
    /// Reduce a live.
    /// </summary>
    public void ReduceLife()
    {
        _currentLives--;
        _lives[_currentLives].SetActive(false);

        if(_currentLives <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Sets game over, if game is won, game gets reset.
    /// </summary>
    /// <param name="gameWon"></param>
    public void GameOver(bool gameWon = false)
    {
        _state = GameState.GameOver;
        if (OnGameStateChange != null)
        {
            OnGameStateChange.Invoke(_state);
        }

        if (!gameWon)
        {
            if(GameManager.Instance)
            {
                _ui.SetGameOverPanel(GameManager.Instance.CurrentHighScore, GameManager.Instance.LastScore, Score, _newHighScore);
            }
            else
            {
                _ui.SetGameOverPanel(Score, Score,  Score, _newHighScore);
                Debug.LogWarning("Game Manager Missing");
            }
        }
        else
        {
            StartCoroutine(WaitBeforeReStart(Reset));
        }

        if (GameManager.Instance)
        {
            GameManager.Instance.LastScore = Score;
        }
        else
        {
            Debug.LogWarning("Game Manager Missing");
        }
    }

    /// <summary>
    /// Waits for end of the frame before reseting the levels.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator WaitBeforeReStart(Action action)
    {
        yield return new WaitForEndOfFrame();
        if(action != null)
        {
            action.Invoke();
        }
    }

    /// <summary>
    /// Increases the socre.
    /// </summary>
    /// <param name="amount"></param>
    public void IncrementScore(uint amount)
    {
        Score += amount;

        if (GameManager.Instance)
        {
            if (Score > GameManager.Instance.CurrentHighScore)
            {
                GameManager.Instance.SetNewScore(Score);
                _ui.SetScoreText("New High Score: ", Score);
                _newHighScore = true;
            }
            else
            {
                _ui.SetScoreText("Score: ", Score);
            }
        }
        else
        {
            Debug.LogWarning("Manager Missing!!");
            _ui.SetScoreText("Score: ", Score);

        }
    }

    /// <summary>
    /// Resets game with event.
    /// </summary>
    public void Reset()
    {
        _state = GameState.GameReset;
        _newHighScore = false;
        _currentLives = CurrentDifficulty.NumberOfLives;

        for (int i = 0; i < _lives.Count; i++)
        {
            _lives[i].SetActive(true);
        }
        if (OnGameStateChange != null)
        {
            OnGameStateChange.Invoke(_state);
        }

        Score = 0;
        _ui.DisablePanel();
        _ui.SetScoreText("Score: " ,Score);
        StartCoroutine(WaitBeforeReStart(StartGame));
    }

    /// <summary>
    /// Loads the main menu.
    /// </summary>
    public void LoadMainMenu()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.MainMenu();
        }
        else
        {
            Debug.LogWarning("Game Manager Missing");
        }
    }
}
