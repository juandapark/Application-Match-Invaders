/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Enemy Spawn and grid Set Up.
 * ******************************************/
using System.Collections;
using UnityEngine;

public class EnemySpawnLogic : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _numberOfColumns = 13;
    [SerializeField] private int _numberOfRows = 6;
    [SerializeField] private float _gapX = 1;
    [SerializeField] private float _gapY = 1;
    [SerializeField] private float _spawnDelay = 0.3f;
    [SerializeField] private Transform _startPoint;

    /// <summary>
    /// Sets the grid on Start.
    /// </summary>
    private void Start()
    {
        StartCoroutine(SetGrid());
    }

    /// <summary>
    /// Spawn the enemies in an grid form based on the gap and column.
    /// </summary>
    private IEnumerator SetGrid()
    {
        float startPointPositionX = _startPoint.position.x;
        float startPointPositionY = _startPoint.position.y;
        float _currentGapX = 0;
        float _currentGapY = 0;

        for (int i = 0; i < _numberOfRows; i++)
        {
            for (int j = 0; j < _numberOfColumns; j++)
            {
                Vector2 SpawnPos = new Vector2(startPointPositionX + _currentGapX, startPointPositionY + _currentGapY);
                SpawnObject(SpawnPos, j, i);
                _currentGapX += _gapX;

                //To make the spawn a bit quicker.
                if(j % 3 == 0)
                {
                    yield return new WaitForSeconds(_spawnDelay);
                }
            }
            _currentGapX = 0;
            _currentGapY += _gapY;
        }
    }

    /// <summary>
    /// Spawns the enemies and gives them a row and columID.
    /// </summary>
    /// <param name="spawnPos"></param>
    /// <param name="columnID"></param>
    /// <param name="rowID"></param>
    private void SpawnObject(Vector2 spawnPos, int columnID, int rowID)
    {
        GameObject enemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
        enemy.transform.parent = _startPoint;

        Enemy enemyObject = enemy.GetComponent<Enemy>();
        enemyObject.RowID = rowID;
        enemyObject.ColumnID = columnID;

        //if enemy is on the last row, make it so that it sets the direction.
        if (rowID == _numberOfRows - 1)
        {
            enemyObject.SetsDirection = true;
        }
        //if enemy is on the first row, make it so that it can shoot.
        else if(rowID == 0)
        {
            enemyObject.CanShoot = true;
        }
        EnemyManager.Instance.AddEnemy(columnID, enemy.GetComponent<Enemy>());
    }


}
