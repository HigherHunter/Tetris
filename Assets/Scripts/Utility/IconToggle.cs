using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    [RequireComponent(typeof(Image))]
    public class IconToggle : MonoBehaviour
    {
        [SerializeField] private Sprite iconTrue, iconFalse;

        private bool _iconState = true;

        private Image _image;

        // Start is called before the first frame update
        private void Start()
        {
            _image = GetComponent<Image>();
            _image.sprite = _iconState ? iconTrue : iconFalse;
        }

        public void ToggleIcon()
        {
            if (!_image || !iconTrue || !iconFalse)
            {
                Debug.LogWarning("Missing image component or sprite for icon toggle");
                return;
            }

            _iconState = !_iconState;
            _image.sprite = _iconState ? iconTrue : iconFalse;
        }
    }
}