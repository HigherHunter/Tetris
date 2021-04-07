using UnityEngine;

namespace Core
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private Shape[] allShapes;

        [SerializeField] private Transform[] queuedShapePositions = new Transform[3];

        private Shape[] _queuedShapes = new Shape[3];

        private const float QueueShapesScale = 0.5f;

        // Start is called before the first frame update
        private void Start()
        {
            InitQueue();
        }

        public Shape SpawnShape()
        {
            Shape shape = GetQueuedShape();
            shape.transform.position = transform.position;
            shape.transform.localScale = Vector3.one;

            if (shape)
                return shape;
            else
            {
                Debug.LogWarning("Invalid shape in spawner!");
                return null;
            }
        }

        private Shape GetRandomShape()
        {
            int i = Random.Range(0, allShapes.Length);
            if (allShapes[i])
                return allShapes[i];
            else
            {
                Debug.LogWarning("Invalid shape!");
                return null;
            }
        }

        private Shape GetQueuedShape()
        {
            Shape firstShape = null;

            if (_queuedShapes[0])
                firstShape = _queuedShapes[0];
            else
                Debug.LogWarning("No valid shape to queue!");


            for (int i = 1; i < _queuedShapes.Length; i++)
            {
                _queuedShapes[i - 1] = _queuedShapes[i];
                _queuedShapes[i - 1].transform.position = queuedShapePositions[i - 1].position + _queuedShapes[i].GetQueuedPositionOffset();
            }

            _queuedShapes[_queuedShapes.Length - 1] = null;

            FillQueue();

            return firstShape;
        }

        private void InitQueue()
        {
            for (int i = 0; i < _queuedShapes.Length; i++)
            {
                _queuedShapes[i] = null;
            }

            FillQueue();
        }

        private void FillQueue()
        {
            for (int i = 0; i < _queuedShapes.Length; i++)
            {
                if (!_queuedShapes[i])
                {
                    _queuedShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity);
                    _queuedShapes[i].transform.position = queuedShapePositions[i].position + _queuedShapes[i].GetQueuedPositionOffset();
                    _queuedShapes[i].transform.localScale = new Vector3(QueueShapesScale, QueueShapesScale, QueueShapesScale);
                }
            }
        }
    }
}