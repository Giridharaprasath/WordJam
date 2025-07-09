using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WordJam
{
    [DefaultExecutionOrder(-10)]
    public class GameInstanceManager : MonoBehaviour
    {
        public static GameInstanceManager Instance { get; private set; }

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

            StartCoroutine(LoadAllPossibleWords());
        }

        private IEnumerator LoadAllPossibleWords()
        {
            Debug.Log("Loading all possible words");

            var resourceRequest = Resources.LoadAsync<TextAsset>("AllWords");
            yield return resourceRequest;
            if (resourceRequest.asset == null)
            {
                Debug.LogError("AllWords.txt not found in Resources folder. Please ensure it is placed correctly.");
                yield break;
            }

            TextAsset textAsset = resourceRequest.asset as TextAsset;

            using StreamReader streamReader = new(new MemoryStream(textAsset.bytes));
            while (!streamReader.EndOfStream)
            {
                string word = streamReader.ReadLine();
                if (!string.IsNullOrWhiteSpace(word))
                {
                    AllWordsTrie.Add(word.Trim().ToUpper());
                }
            }
        }

        public void LoadEndlessMode()
        {
            SceneManager.LoadScene(EndlessModeScene);
        }

        public void LoadLevelMode()
        {
            SceneManager.LoadScene(LevelModeScene);
        }
    }
}
