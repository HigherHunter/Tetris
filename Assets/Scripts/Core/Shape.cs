using UnityEngine;
using Utility;

namespace Core
{
    public class Shape : MonoBehaviour
    {
        [SerializeField]
        private bool canRotate = true;

        [SerializeField] private Vector3 queuePositionOffset;

        public void MoveLeft() => Move(new Vector3(-1, 0, 0));

        public void MoveRight() => Move(new Vector3(1, 0, 0));

        public void MoveUp() => Move(new Vector3(0, 1, 0));

        public void MoveDown() => Move(new Vector3(0, -1, 0));

        private void Move(Vector3 moveDirection) => transform.position += moveDirection;

        public void RotateRight()
        {
            if (canRotate)
                transform.Rotate(0, 0, -90);
        }

        public void RotateLeft()
        {
            if (canRotate)
                transform.Rotate(0, 0, 90);
        }

        public void LandShapeFx(GameObject[] glowSquareFx)
        {
            int i = 0;

            foreach (Transform child in gameObject.transform)
            {
                if (glowSquareFx[i])
                {
                    glowSquareFx[i].transform.position = child.position;
                    glowSquareFx[i].GetComponent<ParticlePlayer>().PlayParticles();

                    i++;
                }
            }
        }

        public Vector3 GetQueuedPositionOffset() => queuePositionOffset;
    }
}