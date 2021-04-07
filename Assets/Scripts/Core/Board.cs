using UnityEngine;
using Utility;

namespace Core
{
    public class Board : MonoBehaviour
    {
        [SerializeField]
        private Transform emptySprite;

        [SerializeField]
        private int height = 30, width = 10, header = 8;

        private Transform[,] _grid;

        [SerializeField] private ParticlePlayer[] rowGlowFx = new ParticlePlayer[4];

        // Awake is called when script is loaded
        private void Awake()
        {
            _grid = new Transform[width, height];
        }

        // Start is called before the first frame update
        private void Start()
        {
            DrawEmptyCells();
        }

        public bool IsOverLimit(Shape shape)
        {
            foreach (Transform child in shape.transform)
            {
                if (child.transform.position.y >= (height - header - 1))
                    return true;
            }

            return false;
        }

        public void StoreShapeInGrid(Shape shape)
        {
            if (shape is null)
                return;

            foreach (Transform child in shape.transform)
            {
                Vector2 pos = Vectorf.Round(child.position);
                _grid[(int)pos.x, (int)pos.y] = child;
            }
        }

        public bool IsValidPosition(Shape shape)
        {
            foreach (Transform child in shape.transform)
            {
                Vector2 pos = Vectorf.Round(child.position);

                if (!IsWithinBoard((int)pos.x, (int)pos.y))
                    return false;

                if (IsOccupied((int)pos.x, (int)pos.y, shape))
                    return false;
            }

            return true;
        }

        private bool IsWithinBoard(int x, int y) => (x >= 0 && x < width && y >= 0);

        private bool IsOccupied(int x, int y, Shape shape) => (!(_grid[x, y] is null) && _grid[x, y].parent != shape.transform);

        public int ClearAllRows()
        {
            int clearedRows = 0;

            // Check board and play fx
            for (int y = 0; y < height; ++y)
            {
                if (IsComplete(y))
                {
                    ClearRowFx(clearedRows, y);
                    clearedRows++;
                }
            }

            // Check board and clear rows
            for (int y = 0; y < height; ++y)
            {
                if (IsComplete(y))
                {
                    ClearRow(y);
                    ShiftRowsDown(y + 1);
                    y--;
                }
            }

            return clearedRows;
        }

        private bool IsComplete(int y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (_grid[x, y] is null)
                    return false;
            }

            return true;
        }

        private void ClearRow(int y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (!(_grid[x, y] is null))
                {
                    // Store parent for further destruction check
                    Transform parent = _grid[x, y].transform.root;

                    _grid[x, y].transform.parent = null;

                    Destroy(_grid[x, y].gameObject);

                    // If parent (shape) doesn't have any pieces left destroy the whole shape
                    if (parent.transform.childCount == 0)
                        Destroy(parent.gameObject);
                }

                _grid[x, y] = null;
            }
        }

        private void ClearRowFx(int id, int y)
        {
            if (rowGlowFx[id])
            {
                rowGlowFx[id].transform.position = new Vector3(0, y, -1);
                rowGlowFx[id].PlayParticles();
            }
        }

        private void ShiftRowsDown(int startY)
        {
            for (int i = startY; i < height; ++i)
                ShiftOneRowDown(i);
        }

        private void ShiftOneRowDown(int y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (!(_grid[x, y] is null))
                {
                    _grid[x, y - 1] = _grid[x, y];
                    _grid[x, y] = null;
                    _grid[x, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }

        private void DrawEmptyCells()
        {
            if (emptySprite)
            {
                for (int y = 0; y < height - header; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Transform clone = Instantiate(emptySprite, new Vector3(x, y, 0), Quaternion.identity);
                        clone.name = "Board Space ( x = " + x + " , y =" + y + " )";
                        clone.transform.parent = transform;
                    }
                }
            }
            else
            {
                Debug.LogWarning("emptySprite is missing!");
            }
        }
    }
}