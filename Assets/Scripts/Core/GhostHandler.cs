using UnityEngine;

namespace Core
{
    public class GhostHandler : MonoBehaviour
    {
        private Shape _ghostShape;
        private bool _hitBottom;
        [SerializeField] private Color ghostColor = new Color(1f, 1f, 1f, 0.2f);

        public void DrawGhost(Shape originalShape, Board gameBoard)
        {
            if (!_ghostShape)
            {
                _ghostShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation);
                _ghostShape.gameObject.name = "GhostShape";

                SpriteRenderer[] allSpriteRenderers = _ghostShape.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer spriteRenderer in allSpriteRenderers)
                {
                    spriteRenderer.color = ghostColor;
                }
            }
            else
            {
                _ghostShape.transform.position = originalShape.transform.position;
                _ghostShape.transform.rotation = originalShape.transform.rotation;
            }

            _hitBottom = false;

            while (!_hitBottom)
            {
                _ghostShape.MoveDown();

                if (!gameBoard.IsValidPosition(_ghostShape))
                {
                    _ghostShape.MoveUp();
                    _hitBottom = true;
                }
            }
        }

        public void ResetGhostShape()
        {
            Destroy(_ghostShape.gameObject);
        }
    }
}