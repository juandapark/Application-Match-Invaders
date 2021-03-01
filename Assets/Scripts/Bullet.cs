/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for the bullets.
 * ******************************************/
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const string BARRIER_NAME = "Barrier"; //The name of the barrier tag.

    [SerializeField] private bool _isEnemy = false;
    [SerializeField] private float _speed = 10;

    private Rigidbody2D _rid;

    private Vector2 _startPos;

    /// <summary>
    /// Set the speed of each bullet.
    /// </summary>
    private void Awake()
    {
        _rid = GetComponent<Rigidbody2D>();
        _startPos = _rid.position;
        if (_isEnemy)
        {
            _speed = LevelManager.Instance.CurrentDifficulty.EnemyBulletSpeed;
        }
        else
        {
            _speed = LevelManager.Instance.CurrentDifficulty.PlayerBulletSpeed;
        }
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
   private void FixedUpdate()
    {
        if (_isEnemy)
        {
            _rid.MovePosition(_rid.position + (Vector2.down * Time.deltaTime * _speed));
        }
        else
        {
            _rid.MovePosition(_rid.position + (Vector2.up * Time.deltaTime * _speed));
        }
    }

/// <summary>
/// DEtect 2D Collsions.
/// </summary>
/// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isEnemy)
        {
            EnemyManager.Instance.ReduceNumberOfBullets(gameObject);
        }

        if (collision.gameObject.tag == BARRIER_NAME)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Reset on Disable.
    /// </summary>
    private void OnDisable()
    {
        _rid.position = _startPos;
    }
}
