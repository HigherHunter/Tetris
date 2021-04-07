using UnityEngine;

namespace Core
{
    public class ShapeHolder : MonoBehaviour
    {
        [SerializeField] private Transform shapeHoldingPosition;

        [SerializeField] private Shape heldShape;

        private const float ShapeHoldScale = 0.5f;

        public bool CanRelease { get; set; }

        public void CatchShape(Shape shape)
        {
            if (heldShape)
                return;

            if (!shape)
            {
                Debug.LogWarning("Invalid shape!");
                return;
            }

            if (shapeHoldingPosition)
            {
                Transform shapeTransform;
                (shapeTransform = shape.transform).position = shapeHoldingPosition.position + shape.GetQueuedPositionOffset();
                shapeTransform.rotation = Quaternion.identity;
                shapeTransform.localScale = new Vector3(ShapeHoldScale, ShapeHoldScale, ShapeHoldScale);
                heldShape = shape;
            }
            else
            {
                Debug.LogWarning("No holding position!");
            }
        }

        public Shape ReleaseShape()
        {
            heldShape.transform.localScale = Vector3.one;

            Shape tempShape = heldShape;

            heldShape = null;

            CanRelease = false;

            return tempShape;
        }

        public Shape GetHeldShape() => heldShape;
    }
}