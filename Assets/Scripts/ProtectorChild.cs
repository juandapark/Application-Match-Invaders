/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for the childs of the protector.
 * ******************************************/
using UnityEngine;

public class ProtectorChild : MonoBehaviour
{
    private Protectors _parentProtector;

    /// <summary>
    /// Get components.
    /// </summary>
    private void Awake()
    {
        _parentProtector = GetComponentInParent<Protectors>();
        _parentProtector.AddChild(gameObject);
    }

    /// <summary>
    /// Collsiion Detection.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
        collision.gameObject.SetActive(false);
    }
}
