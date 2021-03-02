/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for Setting the Enemy Sprite
 * Colour Type.
 * ******************************************/
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct SpritePack
{
    public ColorType ColourType;
    public Sprite[] Sprites;
}

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyGraphics : MonoBehaviour
{
    [SerializeField] protected SpritePack[] _enemySprite;
    [SerializeField] private float _animationChangeDelay = 1;

    private SpriteRenderer _renderer;
    private int _currentSprite;
    private Enemy _enemy;

    /// <summary>
    /// Get Components
    /// </summary>
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Set the colout type and start the animation.
    /// </summary>
    private void OnEnable()
    {
        SetColourType();
        StartAnimation();
    }

    /// <summary>
    /// Sets the colour type.
    /// </summary>
    public void SetColourType()
    {
        _currentSprite = Random.Range(0, _enemySprite.Length);
        _renderer.sprite = _enemySprite[_currentSprite].Sprites[0];
        _enemy.ColourType = _enemySprite[_currentSprite].ColourType;
    }

    /// <summary>
    /// Starts Playing the animation, the animation is simple therofore
    /// no need to add an animator.
    /// </summary>
    public void StartAnimation()
    {
        StartCoroutine(LoopSprites());
    }

    /// <summary>
    /// Coroutine To loop Sprites.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoopSprites()
    {
        while(_enemy.IsAlive)
        {
            _renderer.sprite = _enemySprite[_currentSprite].Sprites[0];
            yield return new WaitForSeconds(_animationChangeDelay);
            _renderer.sprite = _enemySprite[_currentSprite].Sprites[1];
            yield return new WaitForSeconds(_animationChangeDelay);
        }
    }

    /// <summary>
    /// Stop playing tha animation when dying.
    /// </summary>
    public void Die()
    {
        StopAllCoroutines();
        _renderer.sprite = _enemySprite[_currentSprite].Sprites[2];
        if(gameObject.activeSelf && gameObject.activeInHierarchy)
        StartCoroutine(DieAnimation());
    }

    /// <summary>
    /// Plays the die animation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(_animationChangeDelay);
        gameObject.SetActive(false);
    }

}
