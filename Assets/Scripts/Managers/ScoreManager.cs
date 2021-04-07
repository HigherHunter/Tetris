using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private int _score, _lines, _level;

        private bool _didLevelUp;

        [SerializeField] private int linesPerLevel = 5;

        private const int MinLines = 1;
        private const int MaxLines = 4;

        [SerializeField] private TextMeshProUGUI scoreText, linesText, levelText;

        // Start is called before the first frame update
        private void Start()
        {
            Reset();
            UpdateUIText();

            if (!scoreText || !linesText || !levelText)
                Debug.Log("Text element references not set!");
        }

        public void Reset()
        {
            _level = 1;
            _lines = linesPerLevel * _level;
        }

        public void ScoreLines(int numberOfLines)
        {
            numberOfLines = Mathf.Clamp(numberOfLines, MinLines, MaxLines);
            _didLevelUp = false;

            switch (numberOfLines)
            {
                case 1:
                    _score += 40 * _level;
                    break;
                case 2:
                    _score += 100 * _level;
                    break;
                case 3:
                    _score += 300 * _level;
                    break;
                case 4:
                    _score += 1200 * _level;
                    break;
            }

            _lines -= numberOfLines;

            if (_lines <= 0)
                LevelUp();

            UpdateUIText();
        }

        private void UpdateUIText()
        {
            scoreText.text = PadZero(_score, 5);
            linesText.text = _lines.ToString();
            levelText.text = _level.ToString();
        }

        private static string PadZero(int number, int numberOfDigits)
        {
            string numberStr = number.ToString();

            while (numberStr.Length < numberOfDigits)
            {
                numberStr = "0" + numberStr;
            }

            return numberStr;
        }

        private void LevelUp()
        {
            _level++;
            _lines = linesPerLevel * _level;
            _didLevelUp = true;
        }

        public bool GetDidLevelUp() => _didLevelUp;
        public int GetLevel() => _level;
    }
}