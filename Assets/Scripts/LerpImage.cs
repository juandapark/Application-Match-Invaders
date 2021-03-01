/*********************************
 * Created by: David Giraldo
 * Date Created: 28/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 28/02/2021
 *
 * Files handles the Logic for lerping the image transition.
 * ******************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LerpImage : MonoBehaviour
{
    [SerializeField] private bool _lerpOnStart = false;
    private Image _image;

    private void Start()
    {
        if(_lerpOnStart)
        {
            StartImageLerp();
        }
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    /// <summary>
    /// Starts the lerp.
    /// </summary>
    public void StartImageLerp()
    {
        if(GameManager.Instance)
        {
            _image.enabled = true;
            StartCoroutine(Lerp());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Lerps the image fill amount.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Lerp()
    {
        float t = 0;
        float value = _lerpOnStart ? 1 : 0;

        while (t <= GameManager.Instance.TransitionTime)
        {
            t += Time.deltaTime;
            value = Mathf.Lerp(_lerpOnStart ? 1 : 0, _lerpOnStart ? 0 : 1, t / GameManager.Instance.TransitionTime);
            _image.fillAmount = value;
            yield return null;
        }
    }
}
