using UnityEngine;
using UnityEngine.UI;

namespace WordJam
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField]
        private Button EndlessModeButton;
        [SerializeField]
        private Button LevelsModeButton;

        private GameInstanceManager GameInstanceManager;

        void Start()
        {
            EndlessModeButton?.onClick.AddListener(OnClickEndlessMode);
            LevelsModeButton?.onClick.AddListener(OnClickLevelsMode);

            GameInstanceManager = GameInstanceManager.Instance;
        }

        private void OnClickEndlessMode()
        {
            GameInstanceManager?.LoadEndlessMode();
        }

        private void OnClickLevelsMode()
        {
            GameInstanceManager?.LoadLevelMode();
        }
    }
}
