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

        [Header("Parent")]
        [SerializeField]
        private GameObject WordCountLeftParent;
        [SerializeField]
        private GameObject ScoreParent;
        [SerializeField]
        private GameObject TimeLeftParent;

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
    }
}
