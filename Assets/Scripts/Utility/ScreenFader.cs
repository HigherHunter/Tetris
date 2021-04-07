using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    [RequireComponent(typeof(Image))]
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] private float startAlpha = 1f, targetAlpha, delay, timeToFade = 1f;

        private float _increment, _currentAlpha;
        private Image _fadeGraphic;
        private Color _originalColor;

        // Start is called before the first frame update
        private void Start()
        {
            _fadeGraphic = GetComponent<Image>();

            _originalColor = _fadeGraphic.color;

            _currentAlpha = startAlpha;

            _fadeGraphic.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);

            _increment = ((targetAlpha - startAlpha) / timeToFade) * Time.deltaTime;

            StartCoroutine(ScreenFade());
        }

        private IEnumerator ScreenFade()
        {
            yield return new WaitForSeconds(delay);

            while (Mathf.Abs(targetAlpha - _currentAlpha) > 0.01f)
            {
                yield return new WaitForEndOfFrame();

                _currentAlpha += _increment;

                _fadeGraphic.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);
            }
        }
    }
}