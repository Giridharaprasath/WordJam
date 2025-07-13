using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WordJam
{
    [DefaultExecutionOrder(-10)]
    public class GameInstanceManager : MonoBehaviour
    {
        public static GameInstanceManager Instance { get; private set; }

        private readonly string MainMenuScene = "MainMenu";
        private readonly string EndlessModeScene = "EndlessModeScene";
        private readonly string LevelModeScene = "LevelModeScene";

        public Trie AllWordsTrie { get; private set; } = new Trie();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            ChangeGameSettings();
            StartCoroutine(LoadAllPossibleWords());
        }

        private IEnumerator LoadAllPossibleWords()
        {
            GameCommonData.ShowDebugLog("Loading all possible words", 0);

            var resourceRequest = Resources.LoadAsync<TextAsset>("AllWords");
            yield return resourceRequest;
            if (resourceRequest.asset == null)
            {
                GameCommonData.ShowDebugLog("AllWords.txt not found in Resources folder. Please ensure it is placed correctly.", 2);
                yield break;
            }

            TextAsset textAsset = resourceRequest.asset as TextAsset;

            List<double> probabilities = new(new double[26]);

            using StreamReader streamReader = new(new MemoryStream(textAsset.bytes));
            while (!streamReader.EndOfStream)
            {
                string word = streamReader.ReadLine();
                if (!string.IsNullOrWhiteSpace(word))
                {
                    AllWordsTrie.Add(word.Trim().ToUpper());
                }
                foreach (char letter in word.Trim().ToUpper())
                {
                    int letterIndex = GameCommonData.GetLetterIndex(letter.ToString());
                    probabilities[letterIndex - 1]++;
                }
            }

            for (int i = 0; i < probabilities.Count; i++)
            {
                GameCommonData.WeightedRandom.AddItem(i + 1, probabilities[i]);
            }

            GameCommonData.ShowDebugLog("All possible words loaded successfully.", 0);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(MainMenuScene);
        }

        public void LoadEndlessMode()
        {
            SceneManager.LoadScene(EndlessModeScene);
        }

        public void LoadLevelMode()
        {
            SceneManager.LoadScene(LevelModeScene);
        }

        private void ChangeGameSettings()
        {
#if UNITY_EDITOR
            Application.targetFrameRate = 1000;
#elif UNITY_ANDROID
            Application.targetFrameRate = 90;
#endif
        }
    }
}
