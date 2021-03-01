/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for theEnemies.
 * ******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CurrentDirection
{
    Right,
    Left,
    Down
}

[System.Serializable]
public class EnemyColumnReference
{
    public int ColumnID;
    public List<Enemy> Enemies;
}

public class EnemyManager : GameStateLogic
{
    private const int SCORE_MULIPLIER = 10; // the score multiplier fot the fibonacci formula.

    private static EnemyManager _instance;

    public static EnemyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyManager>();
            }
            return _instance;
        }
    }

    public static Action<Vector2> OnDirectionChange;
    public List<EnemyColumnReference> Enemies;

    [SerializeField] private CurrentDirection _direction = CurrentDirection.Right;

    private List<Enemy> _listForMovement = new List<Enemy>();
    private List<Enemy> _listForMovementCopy = new List<Enemy>();
    private EnemyGridAndShootLogic _gridLogicAndShoot;
    private List<Enemy> _enemiesToRemove = new List<Enemy>();
    private bool _canChangeDirection = false;

    /// <summary>
    /// Gets components and events.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        _gridLogicAndShoot = GetComponent<EnemyGridAndShootLogic>();
    }

    /// <summary>
    /// Sets the logic for game Over.
    /// </summary>
    protected override void OnGameOver()
    {
        StopAllCoroutines();
        _gridLogicAndShoot.StopAllCoroutines();
        _gridLogicAndShoot.GameOver();
    }

    /// <summary>
    /// Sets the logic for game Reset.
    /// </summary>
    protected override void OnGameReset()
    {
        _listForMovement.Clear();
        _direction = CurrentDirection.Right;
        _listForMovement = new List<Enemy>(_listForMovementCopy);
        _enemiesToRemove.Clear();
        _gridLogicAndShoot.Reset();

        for (int i = 0; i < _listForMovement.Count; i++)
        {
            if (_listForMovement[i].CanShoot)
            {
                _gridLogicAndShoot.AddEnemyShooters(_listForMovement[i]);
            }
        }
    }

    /// <summary>
    /// Tell the enemy manager that the direction
    /// can now be changed.
    /// </summary>
    public void ChangeDirection()
    {
        _canChangeDirection = true;
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    protected override void StartGame()
    {
        StartCoroutine(MoveEnemies());
    }

    /// <summary>
    /// Changes the direction of the enemies.
    /// </summary>
    private void ChangeTheDirection()
    {
        Vector2 direction = Vector2.right;
        switch (_direction)
        {
            case CurrentDirection.Right:
                _direction = CurrentDirection.Left;
                direction = Vector2.left;
                break;

            case CurrentDirection.Left:
                _direction = CurrentDirection.Right;
                direction = Vector2.right;
                break;
        }

        if (OnDirectionChange != null)
        {
            OnDirectionChange.Invoke(direction);
        }

        //Moves all enemies down.
        for (int i = 0; i < _listForMovement.Count; i++)
        {
            _listForMovement[i].MoveDown();
        }
    }

    /// <summary>
    /// Adds the enemies to the Grid.
    /// </summary>
    /// <param name="coloumnID"></param>
    /// <param name="enemy"></param>
    public void AddEnemy(int coloumnID, Enemy enemy)
    {
        _listForMovement.Add(enemy);
        _listForMovementCopy.Add(enemy);

        //If is set to can shoot, add it ti the list of the shooters.
        if (enemy.CanShoot)
        {
            _gridLogicAndShoot.AddEnemyShooters(enemy);
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].ColumnID == coloumnID)
            {
                Enemies[i].Enemies.Add(enemy);
                return;
            }
        }

        List<Enemy> enemyList = new List<Enemy>();
        enemyList.Add(enemy);
        EnemyColumnReference reference = new EnemyColumnReference();
        reference.ColumnID = coloumnID;
        reference.Enemies = enemyList;
        Enemies.Add(reference);
    }

    /// <summary>
    /// Reduces the number of bullets on screen.
    /// </summary>
    public void ReduceNumberOfBullets(GameObject bullet)
    {
        _gridLogicAndShoot.ReduceNumberOfBullets(bullet);
    }

    /// <summary>
    /// This coroutine moves the enemies and starts shooting, The speed of the enamies
    /// is based on the lenght of the list, the less enemies the quicker they move.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveEnemies()
    {
        //Startd the shooting logic.
        _gridLogicAndShoot.StartShooting();
        while (_listForMovement.Count > 0)
        {
            yield return StartCoroutine(Move()); //Wait for all enemies to Move.

            //After all enemies have moved, the enemies that are dead can be removed from the grid.
            RemoveFromList();

            //After all enemies in the grid have move, they can all change direction.
            if (_canChangeDirection)
            {
                _canChangeDirection = false;
                ChangeTheDirection();
            }
        }

        //means the game has been won.
        LevelManager.Instance.GameOver(true);
    }

    /// <summary>
    /// Removes all the dead enemies from the list.
    /// </summary>
    private void RemoveFromList()
    {
        if (_enemiesToRemove.Count > 0)
        {
            for (int i = _listForMovement.Count - 1; i >= 0; i--)
            {
                for (int k = _enemiesToRemove.Count - 1; k >= 0; k--)
                {
                    if (_enemiesToRemove[k] == _listForMovement[i])
                    {
                        _listForMovement.RemoveAt(i);
                        _enemiesToRemove.RemoveAt(k);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Moves all the enemies on the list.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move()
    {
        for (int i = 0; i < _listForMovement.Count; i++)
        {
            if (_listForMovement[i])
            {
                _listForMovement[i].StartMovement();
                yield return null;
            }
        }
    }

    /// <summary>
    /// Gets all the enemies that can be killed  and adds it to the list to be destroyed.
    /// calculates the score based on fibonacci formula.
    /// </summary>
    /// <param name="enemies"></param>
    public void RemoveEnemies(Enemy enemies)
    {
        //get all the possible enemies that can be killed.
        List<Enemy> possibleEnemiesToRemove = GetPossibleEnemiesToRemove(enemies);
        _enemiesToRemove.AddRange(possibleEnemiesToRemove);

        uint count = (uint)possibleEnemiesToRemove.Count;

        // calculates the score based on fibonacci formula.
        LevelManager.Instance.IncrementScore(count * (FibonacciFormula(count + 1) * SCORE_MULIPLIER));

        //disables all the enemies and sets a new shooter and a new enemy that sets the direction.
        for (int i = 0; i < possibleEnemiesToRemove.Count; i++)
        {
            possibleEnemiesToRemove[i].Die();
            if (possibleEnemiesToRemove[i].SetsDirection)
            {
                _gridLogicAndShoot.SetNewCanCollideEnemy(possibleEnemiesToRemove[i].columnID);
            }

            if (possibleEnemiesToRemove[i].CanShoot)
            {
                _gridLogicAndShoot.RemoveShooter(possibleEnemiesToRemove[i]);
            }
        }
    }
    /// <summary>
    /// Calculate score based on Fibonacci Formula.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    private uint FibonacciFormula(uint n)
    {
        uint firstnumber = 0, secondnumber = 1, result = 0;

        if (n == 0) return 0;
        if (n == 1) return 1;   

        for (uint i = 2; i <= n; i++)
        {
            result = firstnumber + secondnumber;
            firstnumber = secondnumber;
            secondnumber = result;
        }

        return result;
    }

    /// <summary>
    /// This gets all the possible enemies to be killed on the position of the grid to the enemy that you passed.
    /// 
    /// </summary>
    /// <param name="enemies"></param>
    /// <returns></returns>
    private List<Enemy> GetPossibleEnemiesToRemove(Enemy enemies)
    {
        List<Enemy> possibleEnemiesToRemove = new List<Enemy>();
        possibleEnemiesToRemove.Add(enemies);

        for (int i = 0; i < Enemies.Count; i++)
        {
            for (int j = 0; j < Enemies[i].Enemies.Count; j++)
            {
                //Remove enemy from the right
                if (Enemies[i].Enemies[j].columnID == enemies.columnID + 1 && Enemies[i].Enemies[j].RowID == enemies.RowID && Enemies[i].Enemies[j].ColourType == enemies.ColourType)
                {
                    possibleEnemiesToRemove.Add(Enemies[i].Enemies[j]);
                }
                else if (Enemies[i].Enemies[j].columnID == enemies.columnID - 1 && Enemies[i].Enemies[j].RowID == enemies.RowID && Enemies[i].Enemies[j].ColourType == enemies.ColourType) //Remove Enemy From the left
                {
                    possibleEnemiesToRemove.Add(Enemies[i].Enemies[j]);
                }
                else if (Enemies[i].Enemies[j].RowID == enemies.RowID - 1 && Enemies[i].Enemies[j].columnID == enemies.columnID && Enemies[i].Enemies[j].ColourType == enemies.ColourType)//remove enemy from the top
                {
                    possibleEnemiesToRemove.Add(Enemies[i].Enemies[j]);
                }
                else if (Enemies[i].Enemies[j].RowID == enemies.RowID + 1 && Enemies[i].Enemies[j].columnID == enemies.columnID && Enemies[i].Enemies[j].ColourType == enemies.ColourType)//Remove enemy from the bottom
                {
                    possibleEnemiesToRemove.Add(Enemies[i].Enemies[j]);
                }
            }
        }

        return possibleEnemiesToRemove;
    }

    /// <summary>
    /// Destroy this instance.
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

}
