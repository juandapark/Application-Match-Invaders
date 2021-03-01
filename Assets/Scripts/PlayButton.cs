/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for starting the game.
 * ******************************************/
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameManager.Instance.Play();
    }
}
