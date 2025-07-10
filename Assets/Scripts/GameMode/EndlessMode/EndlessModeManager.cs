using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace WordJam
{
    public class EndlessModeManager : GameModeManager
    {
        protected override void Awake()
        {
            CurrentGameMode = GameModeEnum.EndlessMode;

            CurrentLevelData = new()
            {
                gridSize = new(4, 4)
            };

            base.Awake();
        }

        protected override void SpawnTiles()
        {
            List<GridData> gridData = new(CurrentLevelData.gridData);

            for (int i = 0; i < totalGridCount; i++)
            {
                Tile tile = Instantiate(TilePrefab, gridParentList[i].transform);
                tile.name = $"Tile_{i}";
                tile.SetTileTextRandom();
                tile.TileIndex = i;
                tile.OnTileSelected += OnTileSelected;

                tileList.Add(tile);
            }
        }

        protected override void CheckSelectedWord()
        {
#if UNITY_EDITOR
            if (GameInstanceManager == null) return;
#endif

            if (selectedTilesList.Count == 0) return;

            StringBuilder stringBuilder = new();
            // Debug.Log($"Linked List Count : {SelectedTilesList.Count}");

            List<int> tilesIndex = selectedTilesList.GetData();

            foreach (int tileIndex in tilesIndex)
            {
                Tile tile = tileList[tileIndex];
                string alphabet = GameCommonData.AlphabetToWordMap[tile.LetterIndex];
                stringBuilder.Append(alphabet);
                // Debug.Log($"Linked List Index : {alphabet}");
            }

            string ToCheckWord = stringBuilder.ToString();

            if (GameInstanceManager.AllWordsTrie.Contains(ToCheckWord))
            {
                if (selectedWords.Contains(ToCheckWord))
                {
                    Debug.Log("Cant Select the same words again");
                    selectedTilesList.Clear();
                    lastSelectedTileIndex = -1;
                    LineRenderer.positionCount = 0;
                    return;
                }
                selectedWords.Add(ToCheckWord);
                // Debug.Log($"Selected Word Exists!!");
                currentWordCount++;
                CalculateScore();
                CheckWin();
                SetNewTextForSelectedTiles();
            }

            selectedTilesList.Clear();
            lastSelectedTileIndex = -1;
            LineRenderer.positionCount = 0;
        }

        private void SetNewTextForSelectedTiles()
        {
            List<int> tileIndex = selectedTilesList.GetData();

            foreach (int index in tileIndex)
            {
                tileList[index].SetTileTextRandom();
            }
        }

        protected override void CheckWin()
        {
            SetScoreText();
            SetWordCountLeftText();
        }

        protected override void SetWordCountLeftText()
        {
            GameUIManager.SetWordCountLeftText(currentWordCount);
        }
    }
}
