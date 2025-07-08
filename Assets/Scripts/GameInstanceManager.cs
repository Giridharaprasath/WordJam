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

            LoadAllPossibleWords();
        }

        private void LoadAllPossibleWords()
        {
            Debug.Log("Loading all possible words");
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
