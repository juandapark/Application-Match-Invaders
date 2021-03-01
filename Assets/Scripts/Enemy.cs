/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for the enemies.
 * ******************************************/
using UnityEngine;

public enum ColorType
{
    White,
    Red,
    Blue,
    Yellow,
    Green
}

[RequireComponent(typeof(EnemyGraphics))]
public class Enemy : GameStateLogic, IEnemy
{
    private const string BARRIER_NAME = "Barrier"; //The name of the barrier tag.
    private const string BULLET_NAME = "Bullet"; //The name of the Bullet tag.

    public int columnID { get; set; }
    public int RowID { get; set; }

    public float MoveUnits { get;  set; }
    public bool IsAlive = true;
    public bool SetsDirection = false;
    public bool CanShoot = false;
    public ColorType ColourType;

    private EnemyGraphics _enemyUI;
    private Vector2 _startPosition;
    private bool _canMove = false;
    private Rigidbody2D _rid;
    private Vector2 _direction = Vector2.right;
    private float _moveUnitsDown;

    /// <summary>
    /// Get components and Events.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        _enemyUI = GetComponent<EnemyGraphics>();
        _rid = GetComponent<Rigidbody2D>();
        _startPosition = _rid.position;

        EnemyManager.OnDirectionChange -= OnDirectionChange;
        EnemyManager.OnDirectionChange += OnDirectionChange;

        MoveUnits = LevelManager.Instance.CurrentDifficulty.EnemySpeedInUnits;
        _moveUnitsDown = LevelManager.Instance.CurrentDifficulty.EnemySpeedInUnitsDown;
    }

    /// <summary>
    /// Reset Values on game over.
    /// </summary>
    protected override void OnGameOver()
    {
       SetsDirection = RowID == 5 ? true : false;
       CanShoot = RowID == 0 ? true : false;
       _enemyUI.StopAllCoroutines();
    }

    /// <summary>
    /// REset values on game reset.
    /// </summary>
    protected override void OnGameReset()
    {
        Reset();
        _enemyUI.StopAllCoroutines();
    }

    protected override void StartGame()
    {
       // Nothing For Now
    }

    /// <summary>
    /// Disables enemy.
    /// </summary>
    public void Die()
    {
        IsAlive = false;
        _enemyUI.Die();
    }

    /// <summary>
    /// Sets the direction of movement.
    /// </summary>
    /// <param name="direction"></param>
    private void OnDirectionChange(Vector2 direction)
    {
        _direction = direction;
    }

    /// <summary>
    /// Resets the game object.
    /// </summary>
    public void Reset()
    {
        gameObject.SetActive(true);
        IsAlive = true;
        _direction = Vector2.right;
        _rid.position = _startPosition;
    }

    /// <summary>
    /// Shoots a bullet from enemy position.
    /// </summary>
    /// <param name="bullet"></param>
    public void Shoot(GameObject bullet)
    {
        bullet.transform.position = transform.position;
        bullet.SetActive(true);
    }

    /// <summary>
    /// Starts moving the enemy, this is based on units.
    /// </summary>
    public void StartMovement()
    {
        _rid.position += _direction * MoveUnits;
    }

    /// <summary>
    /// Moves the enemy Down.
    /// </summary>
    public void MoveDown()
    {
        _rid.position += Vector2.down * _moveUnitsDown;
    }

    /// <summary>
    /// Detects collison detection.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SetsDirection && collision.gameObject.tag == BARRIER_NAME)
        {
            EnemyManager.Instance.ChangeDirection();
        }
        else
        {
            if (collision.gameObject.tag != BARRIER_NAME)
                EnemyManager.Instance.RemoveEnemies(this);
        }

        if (collision.gameObject.tag == BULLET_NAME)
        {
            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Destroy object and remove events.
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
        EnemyManager.OnDirectionChange -= OnDirectionChange;
    }
}


