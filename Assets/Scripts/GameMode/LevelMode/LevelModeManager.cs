using UnityEngine;

namespace WordJam
{
    public class LevelModeManager : GameModeManager
    {
        protected override void Awake()
        {
            CurrentGameMode = GameModeEnum.LevelMode;
            LoadCurrentLevelNumber();
            LoadLevelData();
            base.Awake();
        }

        private void LoadCurrentLevelNumber()
        {
            CurrentLevelNumber = PlayerPrefs.GetInt("LevelNumber", 1);
        }

        private void SetCurrentLevelNumber()
        {
            CurrentLevelNumber++;
            PlayerPrefs.SetInt("LevelNumber", CurrentLevelNumber);
            PlayerPrefs.Save();
        }

        private void LoadLevelData()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("LevelData");
            string json = textAsset.text;
            GameCommonData.ShowDebugLog($"Level Data JSON: {json}", 0);
            LevelDataList levelDataList = JsonUtility.FromJson<LevelDataList>(json);
            CurrentLevelData = levelDataList.data[CurrentLevelNumber - 1];
        }

        protected override void CheckWin()
        {
            base.CheckWin();

            bool isCompletedLevel = false;

            if (CurrentLevelData.wordCount > 0)
            {
                SetWordCountLeftText();
                if (CurrentLevelData.wordCount <= currentWordCount)
                {
                    isCompletedLevel = true;
                }
            }
            else if (CurrentLevelData.totalScore > 0)
            {
                if (CurrentLevelData.totalScore <= currentScore)
                {
                    isCompletedLevel = true;
                }
            }

            if (isCompletedLevel)
            {
                SetCurrentLevelNumber();
                GameCommonData.ShowDebugLog("Level Completed", 0);
                LoadSceneAgain();
            }
        }
    }
}
