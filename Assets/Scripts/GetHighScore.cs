/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for getting the high score on enable.
 * ******************************************/

using UnityEngine;
using UnityEngine.UI;

public class GetHighScore : MonoBehaviour
{
    [SerializeField] private string _content = "High Score: ";

    private Text _text;

    private void OnEnable()
    {
        _text = GetComponent<Text>();

        _text.text = _content + GameManager.Instance.CurrentHighScore;
    }
}
