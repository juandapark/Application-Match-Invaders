/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for the player movement.
 * ******************************************/
using UnityEngine;

public class MainCharacter : GameStateLogic
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _coolDown = 4;
    [SerializeField] private float _xMinPos, _xMaxPos;

    private GameObject _bullet;
    private float _coolDownTimer = 0;
    private Vector2 _startPos;
    private Rigidbody2D _rid;
    private Vector2 _directionVector;
    private bool _canMove = false;

    /// <summary>
    /// GEt components.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        _bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
         _bullet.SetActive(false);
        _coolDownTimer = _coolDown;
        _rid = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
        _directionVector = transform.position;
    }

    /// <summary>
    /// Disable the players bullet and stop movement.
    /// </summary>
    protected override void OnGameOver()
    {
        _bullet.SetActive(false);
        enabled = false;
    }

    protected override void OnGameReset()
    {
        enabled = true;
    }

    /// <summary>
    /// Resume movement and reset position.
    /// </summary>
    protected override void StartGame()
    {
        _rid.position = _startPos;
        enabled = true;
    }

    /// <summary>
    /// Get the user input on update.
    /// </summary>
    private void Update()
    {
        _coolDownTimer -= Time.deltaTime;

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && transform.position.x > _xMinPos)
        {
            _directionVector = Vector2.left;
            _canMove = true;
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && transform.position.x < _xMaxPos)
        {
            _directionVector = Vector2.right;
            _canMove = true;
        }
        else
        {
            _canMove = false;
        }

        if (Input.GetKey(KeyCode.Space) && _coolDownTimer <= 0)
        {
            _coolDownTimer = _coolDown;
            _bullet.transform.position = transform.position;

            if (!_bullet.activeSelf)
            {
                _bullet.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Move the player rigidbody on fixed update.
    /// </summary>
    private void FixedUpdate()
    {
        if (_canMove)
        {
            Vector2 dir = (_directionVector * speed * Time.deltaTime);
            _rid.MovePosition(_rid.position + (dir));
        }
    }

    /// <summary>
    /// Collision Detection.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            collision.gameObject.SetActive(false);
        }

        LevelManager.Instance.ReduceLife();
    }
}
