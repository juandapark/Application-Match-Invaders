/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for the protectors.
 * ******************************************/
using System.Collections.Generic;
using UnityEngine;

public class Protectors : GameStateLogic
{
    [SerializeField] private int _numberOfHits = 5;
    private List<GameObject> _childs = new List<GameObject>();

    private Collider2D _coll;

    /// <summary>
    /// Get components and game set up.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        _coll = GetComponent<Collider2D>();
        _numberOfHits = LevelManager.Instance.CurrentDifficulty.ProtectorLives;
    }

    /// <summary>
    /// Resets the protector back to the default values.
    /// </summary>
    protected override void OnGameReset()
    {
        _numberOfHits = LevelManager.Instance.CurrentDifficulty.ProtectorLives;
        DisableAndEnableChilds(true);
        _coll.enabled = true;
    }

    /// <summary>
    /// Takes a life from the protector.
    /// </summary>
    public void ReduceLife()
    {
        _numberOfHits--;
        if(_numberOfHits <= 0)
        {
            _coll.enabled = false;
            DisableAndEnableChilds(false);
        }
    }

    /// <summary>
    /// Collision Detection.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
            ReduceLife();
    }

    /// <summary>
    /// Adds all the childs of the protectors.
    /// </summary>
    /// <param name="child"></param>
    public void AddChild(GameObject child)
    {
        _childs.Add(child);
    }

    /// <summary>
    /// Disables or Enables the childs.
    /// </summary>
    /// <param name="active"></param>
    private void DisableAndEnableChilds(bool active)
    {
        for (int i = 0; i < _childs.Count; i++)
        {
            _childs[i].SetActive(active);
        }
    }

    protected override void OnGameOver()
    {
        // Nothing For Now
    }

    protected override void StartGame()
    {
        // Nothing For Now
    }
}
