/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for the Transitions and saving the game score.
 * ******************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> Manages the state of the whole application </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public uint CurrentHighScore = 0;
    public uint LastScore = 0;
    public float TransitionTime = 0.5f;

    [SerializeField] private string[] _gameScenes;

    /// <summary>
    /// Set framerate and make singleton.
    /// </summary>
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        Application.targetFrameRate = 60; //Set the target framerate

        if (PlayerPrefs.HasKey("HighScore"))
        {
            CurrentHighScore = (uint)PlayerPrefs.GetInt("HighScore");
        }
    }

    /// <summary>
    /// Starts the Game.
    /// </summary>
    public void Play()
    {
        StartCoroutine(LoadScene(_gameScenes[1]));
    }

    /// <summary>
    /// Goes to the main menu scene.
    /// </summary>
    public void MainMenu()
    {
        StartCoroutine(LoadScene(_gameScenes[0]));
    }

    /// <summary>
    /// Coroutine to load Scenes.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadScene(string sceneName)
    {
        Debug.Log("Loading game!");
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Sets the new high score.
    /// </summary>
    /// <param name="score"></param>
    public void SetNewScore(uint score)
    {
        if (score > CurrentHighScore)
        {
            CurrentHighScore = score;
            PlayerPrefs.SetInt("HighScore", (int)CurrentHighScore);
        }
    }
}