/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for setting the game states of the game.
 * ******************************************/
using UnityEngine;

public abstract class GameStateLogic : MonoBehaviour
{
    /// <summary>
    /// Subscribe to the event.
    /// </summary>
    protected virtual void Awake()
    {
        LevelManager.OnGameStateChange -= OnGameStateChange;
        LevelManager.OnGameStateChange += OnGameStateChange;
    }

    /// <summary>
    /// Call the functions based on the event.
    /// </summary>
    /// <param name="state"></param>
    protected virtual void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.GameOver:
                OnGameOver();
                break;

            case GameState.GameReset:
                OnGameReset();
                break;


            case GameState.GameStart:
                StartGame();
                break;
        }
    }

    /// <summary>
    /// Make the other classes to inherit these funtions.
    /// </summary>
    protected abstract void OnGameOver();
    protected abstract void OnGameReset();
    protected abstract void StartGame();

    /// <summary>
    /// unsubscribe from event.
    /// </summary>
    protected virtual void OnDestroy()
    {
        LevelManager.OnGameStateChange -= OnGameStateChange;
    }
}
