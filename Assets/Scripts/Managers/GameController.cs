using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameController : MonoBehaviour
    {
        private Board _gameBoard;

        private Spawner _spawner;

        private SoundManager _soundManager;

        private ScoreManager _scoreManager;

        private Shape _currentlyActiveShape;

        private GhostHandler _ghostShape;

        private ShapeHolder _shapeHolder;

        private enum Direction { None, Tapped, Left, Right, Up, Down }

        private Direction _tapTouch;
        private Direction _swipeDirection = Direction.None;

        private float _timeToDrop;
        [SerializeField]
        private float baseDropInterval = 0.5f;

        private float _dropInterval;

        private float _timeToNextKeyLeftRight;
        private float _timeToNextKeyDown;
        private float _timeToNextKeyRotate;
        [SerializeField]
        private float keyRepeatRateLeftRight = 0.25f, keyRepeatRateDown = 0.01f, keyRepeatRateRotate = 0.25f;

        private bool _gameOver;

        [SerializeField] private GameObject pausePanel;
        private bool _isPaused;

        [SerializeField] private GameObject gameOverPanel;

        [SerializeField] private GameObject[] glowSquareFx = new GameObject[4];

        private void OnEnable()
        {
            TouchController.TapEvent += TapHandler;
            TouchController.SwipeEvent += SwipeHandler;
            TouchController.SwipeEndEvent += SwipeEndHandler;
        }

        private void OnDisable()
        {
            TouchController.TapEvent -= TapHandler;
            TouchController.SwipeEvent -= SwipeHandler;
            TouchController.SwipeEndEvent -= SwipeEndHandler;
        }

        // Start is called before the first frame update
        private void Start()
        {
            _dropInterval = baseDropInterval;

            _gameBoard = FindObjectOfType<Board>();
            _spawner = FindObjectOfType<Spawner>();
            _soundManager = FindObjectOfType<SoundManager>();
            _scoreManager = FindObjectOfType<ScoreManager>();
            _ghostShape = FindObjectOfType<GhostHandler>();
            _shapeHolder = FindObjectOfType<ShapeHolder>();

            if (_spawner)
            {
                if (_currentlyActiveShape == null)
                    _currentlyActiveShape = _spawner.SpawnShape();
            }
            else
            {
                Debug.LogWarning("There is no spawner!");
            }

            if (!_gameBoard)
                Debug.LogWarning("There is no game board!");

            if (!_soundManager)
                Debug.LogWarning("There is no sound manager!");

            if (!_scoreManager)
                Debug.LogWarning("There is no score manager!");

            if (gameOverPanel)
                gameOverPanel.SetActive(false);

            if (pausePanel)
                pausePanel.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_currentlyActiveShape || _gameOver)
                return;
            PlayerInput();
        }

        private void LateUpdate()
        {
            if (_ghostShape)
                _ghostShape.DrawGhost(_currentlyActiveShape, _gameBoard);
        }

        public void HoldShape()
        {
            // Nothing is held, we can catch
            if (!_shapeHolder.GetHeldShape())
            {
                _shapeHolder.CatchShape(_currentlyActiveShape);

                _currentlyActiveShape = _spawner.SpawnShape();

                _soundManager.PlayHoldShapeSound();
            }
            else if (_shapeHolder.CanRelease)
            {
                Shape tempShape = _currentlyActiveShape;
                _currentlyActiveShape = _shapeHolder.ReleaseShape();
                _currentlyActiveShape.transform.position = _spawner.transform.position;
                _shapeHolder.CatchShape(tempShape);

                _soundManager.PlayHoldShapeSound();
            }
            else
            {
                _soundManager.PlayWrongMoveSound();
            }

            if (_ghostShape)
                _ghostShape.ResetGhostShape();
        }

        public void TogglePause()
        {
            if (_gameOver)
                return;

            _isPaused = !_isPaused;

            if (pausePanel)
            {
                pausePanel.SetActive(_isPaused);

                Time.timeScale = _isPaused ? 0 : 1;
            }
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        private void PlayerInput()
        {
            /*if (Input.GetButton("MoveRight") && Time.time > _timeToNextKeyLeftRight || Input.GetButtonDown("MoveRight"))
            {
                PlayerMoveRight();
            }
            else if (Input.GetButton("MoveLeft") && Time.time > _timeToNextKeyLeftRight || Input.GetButtonDown("MoveLeft"))
            {
                PlayerMoveLeft();
            }
            else if (Input.GetButtonDown("Rotate") && Time.time > _timeToNextKeyRotate)
            {
                PlayerRotate();
            }
            else if (Input.GetButton("MoveDown") && Time.time > _timeToNextKeyDown || Time.time > _timeToDrop)
            {
                PlayerMoveDown();
            }*/

            if (_isPaused) return;

            if (_swipeDirection == Direction.Right && Time.time > _timeToNextKeyLeftRight)
            {
                PlayerMoveRight();

                _swipeDirection = Direction.None;
            }
            else if (_swipeDirection == Direction.Left && Time.time > _timeToNextKeyLeftRight)
            {
                PlayerMoveLeft();

                _swipeDirection = Direction.None;
            }
            else if (_tapTouch == Direction.Tapped && Time.time > _timeToNextKeyRotate)
            {
                PlayerRotate();
                _tapTouch = Direction.None;
            }
            else if (_swipeDirection == Direction.Down && Time.time > _timeToNextKeyDown || Time.time > _timeToDrop)
            {
                PlayerMoveDown();

                _swipeDirection = Direction.None;
            }
        }

        private void PlayerMoveRight()
        {
            _currentlyActiveShape.MoveRight();
            _timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

            if (!_gameBoard.IsValidPosition(_currentlyActiveShape))
            {
                _currentlyActiveShape.MoveLeft();
                _soundManager.PlayWrongMoveSound(0.4f);
            }
            else
                _soundManager.PlayMoveSound(0.25f);
        }

        private void PlayerMoveLeft()
        {
            _currentlyActiveShape.MoveLeft();
            _timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

            if (!_gameBoard.IsValidPosition(_currentlyActiveShape))
            {
                _currentlyActiveShape.MoveRight();
                _soundManager.PlayWrongMoveSound(0.4f);
            }
            else
                _soundManager.PlayMoveSound(0.25f);
        }

        private void PlayerRotate()
        {
            _currentlyActiveShape.RotateRight();
            _timeToNextKeyRotate = Time.time + keyRepeatRateRotate;

            if (!_gameBoard.IsValidPosition(_currentlyActiveShape))
            {
                _currentlyActiveShape.RotateLeft();
                _soundManager.PlayWrongMoveSound(0.4f);
            }
            else
                _soundManager.PlayMoveSound(0.25f);
        }

        private void PlayerMoveDown()
        {
            _timeToDrop = Time.time + _dropInterval;
            _timeToNextKeyDown = Time.time + keyRepeatRateDown;

            _currentlyActiveShape.MoveDown();
            if (!_gameBoard.IsValidPosition(_currentlyActiveShape))
            {
                if (_gameBoard.IsOverLimit(_currentlyActiveShape))
                {
                    GameOver();
                }
                else
                {
                    LandShape();
                }
            }
        }

        private void LandShape()
        {
            _currentlyActiveShape.MoveUp();

            _gameBoard.StoreShapeInGrid(_currentlyActiveShape);

            int numberOfClearedRows = _gameBoard.ClearAllRows();

            if (numberOfClearedRows <= 0)
            {
                _currentlyActiveShape.LandShapeFx(glowSquareFx);
            }
            else
            {
                _soundManager.PlayClearRowSound(0.5f);
                _scoreManager.ScoreLines(numberOfClearedRows);

                if (_scoreManager.GetDidLevelUp())
                {
                    _dropInterval = Mathf.Clamp(_dropInterval - (_scoreManager.GetLevel() - 1) * 0.025f, 0.05f, 1f);
                    _soundManager.PlayLevelUpSound();
                }
            }

            if (_ghostShape)
                _ghostShape.ResetGhostShape();

            if (_shapeHolder)
                _shapeHolder.CanRelease = true;

            _currentlyActiveShape = _spawner.SpawnShape();

            _timeToNextKeyLeftRight = Time.time;
            _timeToNextKeyDown = Time.time;
            _timeToNextKeyRotate = Time.time;

            _soundManager.PlayDropSound();
        }

        private void TapHandler(Vector2 swipeMovement)
        {
            _tapTouch = GetDirection(swipeMovement);
        }

        private void SwipeHandler(Vector2 swipeMovement)
        {
            _swipeDirection = GetDirection(swipeMovement);
        }

        private void SwipeEndHandler(Vector2 swipeMovement)
        {
            _swipeDirection = Direction.None;
        }

        private Direction GetDirection(Vector2 swipeMovement)
        {
            Direction swipeDirection = Direction.None;

            if (Mathf.Abs(swipeMovement.x) == 0 && Mathf.Abs(swipeMovement.y) == 0)
                swipeDirection = Direction.Tapped;
            // Horizontal movement
            else if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
                swipeDirection = (swipeMovement.x >= 0) ? Direction.Right : Direction.Left;
            // Vertical movement
            else
                swipeDirection = (swipeMovement.y >= 0) ? Direction.Up : Direction.Down;

            return swipeDirection;
        }

        private void GameOver()
        {
            _currentlyActiveShape.MoveUp();
            _gameOver = true;
            if (gameOverPanel)
                gameOverPanel.SetActive(true);
            _soundManager.PlayGameOverSound();
        }
    }
}