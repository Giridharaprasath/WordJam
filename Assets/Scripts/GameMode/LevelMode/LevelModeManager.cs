using UnityEngine;

namespace WordJam
{
    public class LevelModeManager : GameModeManager
    {
        protected override void Awake()
        {
            CurrentGameMode = GameModeEnum.LevelMode;
            LoadLevelData();
            base.Awake();
        }

        private void LoadLevelData()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("LevelDataL1");
            string json = textAsset.text;
            Debug.Log($"Level Data JSON: {json}");
            CurrentLevelData = JsonUtility.FromJson<LevelData>(json);
        }
    }
}
