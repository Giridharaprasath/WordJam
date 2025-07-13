using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WordJam
{
    public class GameUIManager : MonoBehaviour
    {
        private GameInstanceManager GameInstanceManager;

        [Header("Buttons")]
        [SerializeField]
        private Button BackButton;

        [Header("Scoreboard")]
        [SerializeField]
        private TMP_Text ScoreText;
        [SerializeField]
        private TMP_Text WordCountLeftText;
        [SerializeField]
        private TMP_Text TimeLeftText;
        [SerializeField]
        private TMP_Text AverageScoreText;

        [Header("Parent")]
        [SerializeField]
        private GameObject WordCountLeftParent;
        [SerializeField]
        private GameObject ScoreParent;
        [SerializeField]
        private GameObject TimeLeftParent;

        [Header("Screen Block")]
        [SerializeField]
        private GameObject ScreenBlock;

        void Awake()
        {
            GameInstanceManager = GameInstanceManager.Instance;
        }

        void Start()
        {
            BackButton.onClick.AddListener(OnClickBackButton);
        }

        private void OnClickBackButton()
        {
            GameInstanceManager.LoadMainMenu();
        }

        #region Word Count
        public void SetWordCountLeftState(bool Value)
        {
            WordCountLeftParent?.SetActive(Value);
        }
        public void SetWordCountLeftText(int wordCount)
        {
            WordCountLeftText.text = wordCount.ToString();
        }
        #endregion

        #region Score
        public void SetScoreState(bool Value)
        {
            ScoreParent?.SetActive(Value);
        }
        public void SetScoreText(string score)
        {
            ScoreText.text = score;
        }
        #endregion

        #region Time
        public void SetTimeLeftState(bool Value)
        {
            TimeLeftParent?.SetActive(Value);
        }
        public void SetTimeLeftText(string time)
        {
            TimeLeftText.text = time;
        }
        #endregion

        #region Average Score
        public void SetAverageScore(string averageScore)
        {
            AverageScoreText.text = averageScore;
        }
        #endregion

        #region Screen Block
        public void ShowScreenBlock()
        {
            ScreenBlock?.SetActive(true);
        }
        public void HideScreenBlock()
        {
            ScreenBlock?.SetActive(false);
        }
        #endregion
    }
}
