/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for shooting and setting a new enemy to set the direction.
 * ******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGridAndShootLogic : MonoBehaviour
{
    [SerializeField] private int _maxNumberOfBulletsOnScreen = 0;
    [SerializeField] private EnemyManager _manager;
    [SerializeField] private float _shootDelayMin = 2;
    [SerializeField] private float _shootDelayMax = 6;
    [SerializeField] private GameObject _bullet;

    private List<Enemy> _enemyShooters = new List<Enemy>();
    private List<GameObject> _bulletPool = new List<GameObject>();
    private List<GameObject> _bulletPoolCopy = new List<GameObject>();
    private int _currentShooter = -1;

    /// <summary>
    /// Creates the bullet pool and gets components.
    /// </summary>
    private void Awake()
    {
        _manager = GetComponent<EnemyManager>();
        _shootDelayMax = LevelManager.Instance.CurrentDifficulty.EnemyShootMaxDelay;

        for (int i = 0; i < _maxNumberOfBulletsOnScreen; i++)
        {
            GameObject obj =  Instantiate(_bullet, transform.position, Quaternion.identity, transform);
            obj.SetActive(false);
            _bulletPool.Add(obj);
        }
        _bulletPoolCopy = new List<GameObject>(_bulletPool);
    }

    /// <summary>
    /// Sets a new enemy that can set the direction, it must be the closest enemy to the top of the column.
    /// </summary>
    /// <param name="columnID"></param>
    public void SetNewCanCollideEnemy(int columnID)
    {
        for (int i = 0; i < _manager.Enemies.Count; i++)
        {
            if(i == columnID)
            {
                //Lets start from the top to set a new enemy to Set the Direction.
                for (int j = _manager.Enemies[i].Enemies.Count -1; j >= 0; j--)
                {
                    if(_manager.Enemies[i].Enemies[j].IsAlive)
                    {
                        _manager.Enemies[i].Enemies[j].SetsDirection = true;
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Stars Shooting.
    /// </summary>
    public void StartShooting()
    {
        StartCoroutine(ShootDelay());
    }

    /// <summary>
    /// Shoot coroutine with delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootDelay()
    {
        while (_manager.Enemies.Count > 0)
        {
            yield return new WaitForSeconds(Random.Range(_shootDelayMin, _shootDelayMax));
            EnemyShoot(GetShooter());
        }
    }

    /// <summary>
    /// Sets a new shooter.
    /// </summary>
    /// <param name="columnID"></param>
    public void SetNewShooter(int columnID)
    {
        for (int i = 0; i < _manager.Enemies.Count; i++)
        {
            if (i == columnID)
            {
                //Lets start from the top to set a new enemy to Set the Direction.
                for (int j = 0; j < _manager.Enemies[i].Enemies.Count; j++)
                {
                    if (_manager.Enemies[i].Enemies[j].IsAlive)
                    {
                        _manager.Enemies[i].Enemies[j].CanShoot = true;
                        AddEnemyShooters(_manager.Enemies[i].Enemies[j]);
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Puts the bullet back in the pool.
    /// </summary>
    /// <param name="bullet"></param>
    public void ReduceNumberOfBullets(GameObject bullet)
    {
        if(!_bulletPool.Contains(bullet))
        {
            _bulletPool.Add(bullet);
        }
    }

    /// <summary>
    /// Resets the lists.
    /// </summary>
    public void Reset()
    {
        _bulletPool.Clear();
        _bulletPool = new List<GameObject>(_bulletPoolCopy);
        _enemyShooters.Clear();
    }

    /// <summary>
    /// Set all bullets active to false. so it does not collide with enemy after game over.
    /// </summary>
    public void GameOver()
    {
        for (int i = 0; i < _bulletPoolCopy.Count; i++)
        {
            if(_bulletPoolCopy[i].activeSelf)
            {
                _bulletPoolCopy[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Enemy shoots and bullet is removed from pool.
    /// </summary>
    /// <param name="shooter"></param>
    private void EnemyShoot(int shooter)
    {
        if (_bulletPool.Count > 0 && shooter < _enemyShooters.Count)
        {
            _enemyShooters[shooter].Shoot(_bulletPool[0]);
            _bulletPool.RemoveAt(0);
        }
    }

    /// <summary>
    /// Adds the shooters, these are the enemies closest to the player.
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemyShooters(Enemy enemy)
    {
        _enemyShooters.Add(enemy);
    }

    /// <summary>
    /// Removes a shooter from the list of shooters.
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveShooter(Enemy enemy)
    {
        for (int i = 0; i < _enemyShooters.Count; i++)
        {
            if(_enemyShooters[i] == enemy)
            {
                enemy.CanShoot = false;
                _enemyShooters.RemoveAt(i);
                break;
            }
        }
        SetNewShooter(enemy.columnID);
    }

    /// <summary>
    /// Selects a random shooter.
    /// </summary>
    /// <returns></returns>
    private int GetShooter()
    {
        int randomShooter = Random.Range(0, _enemyShooters.Count);
        while(_currentShooter == randomShooter && _enemyShooters.Count > 1)
        {
            randomShooter = Random.Range(0, _enemyShooters.Count);
        }
        _currentShooter = randomShooter;

        return randomShooter;
    }
}
