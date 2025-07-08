using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordJam
{
    public abstract class GameModeManager : MonoBehaviour
    {
        [Header("Game Mode Settings")]
        public GameModeEnum CurrentGameMode;
        public LevelData CurrentLevelData;

        [Header("Grid Settings")]
        public GridLayoutGroup GridLayoutGroup;
        public GameObject GridParentPrefab;

        [SerializeField]
        private List<GameObject> gridParentList;

        protected virtual void Awake()
        {
            GridLayoutGroup.constraintCount = CurrentLevelData.GridSize.X;
        }

        protected virtual void Start()
        {
            SpawnGridParent();
        }

        internal virtual void SpawnGridParent()
        {
            int totalCount = CurrentLevelData.GridSize.X * CurrentLevelData.GridSize.Y;

            for (int i = 0; i < totalCount; i++)
            {
                GameObject gridParent = Instantiate(GridParentPrefab, GridLayoutGroup.transform);
                gridParent.name = $"GridParent_{i}";
                gridParentList.Add(gridParent);
            }
        }
    }
}
