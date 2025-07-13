using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace WordJam
{
    public class EndlessModeManager : GameModeManager
    {
        [SerializeField]
        private GameObject[] ColumnTopPlaceholder;

        private int totalPointsCount = 0;
        private readonly float moveSpeed = 0.5f;

        public HashSet<string> AllCombinations = new();
        private Dictionary<int, int> TileIndexSwapped = new();

        protected override void Awake()
        {
            CurrentGameMode = GameModeEnum.EndlessMode;

            CurrentLevelData = new()
            {
                gridSize = new(4, 4),
                // gridData = new()
                // {
                //     new GridData { tileType = 0, letter = "G" },
                //     new GridData { tileType = 0, letter = "A" },
                //     new GridData { tileType = 0, letter = "M" },
                //     new GridData { tileType = 0, letter = "E" },
                //     new GridData { tileType = 0, letter = "W" },
                //     new GridData { tileType = 0, letter = "O" },
                //     new GridData { tileType = 0, letter = "R" },
                //     new GridData { tileType = 0, letter = "D" },
                //     new GridData { tileType = 0, letter = "J" },
                //     new GridData { tileType = 0, letter = "A" },
                //     new GridData { tileType = 0, letter = "M" },
                //     new GridData { tileType = 0, letter = "S" },
                //     new GridData { tileType = 0, letter = "B" },
                //     new GridData { tileType = 0, letter = "U" },
                //     new GridData { tileType = 0, letter = "G" },
                //     new GridData { tileType = 0, letter = "S" },
                // }
            };

            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            FindAllWords();
            if (AllCombinations.Count == 0)
            {
                GameCommonData.ShowDebugLog("No valid words found in the grid. Please check the grid data.", 1);
                ResetAllTiles();
            }
        }

        private void ResetAllTiles()
        {
            do
            {
                foreach (Tile tile in tileList)
                {
                    GameCommonData.ShowDebugLog($"Resetting tile: {tile.name}", 0);
                    tile.SetTileTextRandom();
                }
                FindAllWords();
            } while (AllCombinations.Count == 0);
        }

        private void ResetSelectedTiles(List<int> selectedTiles)
        {
            do
            {
                foreach (int index in selectedTiles)
                {
                    int newIndex = TileIndexSwapped[index];
                    GameCommonData.ShowDebugLog($"Resetting selected tile: {tileList[newIndex].name}", 0);
                    tileList[newIndex].SetTileTextRandom();
                }
                FindAllWords();
            } while (AllCombinations.Count == 0);

            TileIndexSwapped.Clear();
            MoveAllTilesToGrid();
        }

        protected override void SpawnTiles()
        {
            List<GridData> gridData = new(CurrentLevelData.gridData);

            for (int i = 0; i < totalGridCount; i++)
            {
                Tile tile = Instantiate(TilePrefab, gridParentList[i].transform);
                tile.name = $"Tile_{i}";
                // tile.SetTileText(gridData[i]);
                tile.SetTileTextRandom();
                tile.TileIndex = i;
                tile.OnTileSelected += OnTileSelected;

                tileList.Add(tile);
            }
        }

        protected override void CheckSelectedWord()
        {
            base.CheckSelectedWord();
        }

        protected override void CalculateScore()
        {
            base.CalculateScore();
            totalPointsCount += selectedTilesList.Count;
        }

        private void OnWordTileCompleted()
        {
            List<int> tileIndex = selectedTilesList.GetData();
            bool[] columnToMove = new bool[4] { false, false, false, false };

            foreach (int index in tileIndex)
            {
                // tileList[index].SetTileTextRandom();
                int column = index % 4;
                columnToMove[column] = true;

                tileList[index].transform.SetParent(ColumnTopPlaceholder[column].transform, false);
                tileList[index].transform.localPosition = Vector3.zero;
            }

            ShowBlockScreen();

            for (int i = 0; i < 4; i++)
            {
                if (!columnToMove[i]) continue;

                List<int> columnIndices = new();
                for (int j = 0; j < 4; j++)
                {
                    columnIndices.Add(i + j * 4);
                }
                columnIndices.Reverse();

                MoveTileDownByColumn(columnIndices);
                MoveTileFromPlaceholderToGrid(columnIndices, i);
            }

            SortTilesByIndex();
            ResetSelectedTiles(tileIndex);
            Invoke(nameof(HideBlockScreen), moveSpeed + 0.5f);
        }

        private void MoveTileDownByColumn(List<int> columnIndices)
        {
            int count = columnIndices.Count;
            for (int i = 0; i < count - 1; i++)
            {
                var targetParent = gridParentList[columnIndices[i]].transform;
                if (targetParent.childCount > 0)
                    continue;

                for (int j = i + 1; j < count; j++)
                {
                    var sourceParent = gridParentList[columnIndices[j]].transform;
                    if (sourceParent.childCount > 0)
                    {
                        Transform tileTransform = sourceParent.GetChild(0);
                        tileTransform.SetParent(targetParent, true);
                        // tileTransform.DOLocalMove(Vector3.zero, moveSpeed).SetEase(Ease.Linear).OnComplete(() =>
                        // {
                        //     tileTransform.localPosition = Vector3.zero;
                        // });
                        int oldIndex = tileList[columnIndices[j]].TileIndex;
                        tileList[columnIndices[j]].TileIndex = columnIndices[i];
                        TileIndexSwapped[oldIndex] = columnIndices[i];
                        break;
                    }
                }
            }
        }

        private void MoveTileFromPlaceholderToGrid(List<int> columnIndices, int column)
        {
            int childCount = ColumnTopPlaceholder[column].transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                int index = columnIndices[columnIndices.Count - 1 - i];
                Transform tileTransform = ColumnTopPlaceholder[column].transform.GetChild(0);
                tileTransform.SetParent(gridParentList[index].transform, true);
                // tileTransform.DOLocalMove(Vector3.zero, moveSpeed).SetEase(Ease.Linear).OnComplete(() =>
                // {
                //     tileTransform.localPosition = Vector3.zero;
                // });
                Tile tile = tileTransform.GetComponent<Tile>();
                int oldIndex = tile.TileIndex;
                tile.TileIndex = index;
                TileIndexSwapped[oldIndex] = index;
            }
        }

        private void SortTilesByIndex()
        {
            tileList.Sort((x, y) => x.TileIndex.CompareTo(y.TileIndex));
            for (int i = 0; i < tileList.Count; i++)
            {
                tileList[i].name = $"Tile_{i}";
            }
        }

        private void MoveAllTilesToGrid()
        {
            GameCommonData.ShowDebugLog("Moving all tiles to grid", 0);
            foreach (var tile in tileList)
            {
                tile.transform.DOLocalMove(Vector3.zero, moveSpeed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    tile.transform.localPosition = Vector3.zero;
                });
            }
        }

        protected override void CheckWin()
        {
            SetScoreText();
            SetWordCountLeftText();
            SetAverageScoreText();
            OnWordTileCompleted();
        }

        protected override void SetWordCountLeftText()
        {
            GameUIManager.SetWordCountLeftText(currentWordCount);
        }

        private void SetAverageScoreText()
        {
            int averageScore = currentScore / totalPointsCount;
            GameUIManager.SetAverageScore($"{averageScore}");
        }

        private void ShowBlockScreen()
        {
            GameUIManager.ShowScreenBlock();
        }

        private void HideBlockScreen()
        {
            GameUIManager.HideScreenBlock();
        }
        public void FindAllWords()
        {
            AllCombinations.Clear();

            for (int i = 0; i < tileList.Count; i++)
            {
                SearchWordsInGrid(i, new List<int>(), string.Empty);
            }

            PrintAllCombinations();
        }

        private void SearchWordsInGrid(int index, List<int> allPaths, string currentWord)
        {
            Tile tile = tileList[index];
            string letter = GameCommonData.AlphabetToWordMap[tile.LetterIndex];
            allPaths.Add(index);

            currentWord += letter;

            if (currentWord.Length >= 3)
            {
                if (GameInstanceManager.AllWordsTrie.Contains(currentWord))
                {
                    if (!selectedWords.Contains(currentWord))
                        AllCombinations.Add(currentWord);
                }

                List<string> foundWords = GameInstanceManager.AllWordsTrie.StartsWith(currentWord);
                if (foundWords.Count == 0) return;

                if (currentWord.Length == 8) return;
            }

            foreach (int adjacentIndex in tileGraph.GetAdjacentNodes(index))
            {
                if (allPaths.Contains(adjacentIndex)) continue;

                SearchWordsInGrid(adjacentIndex, allPaths, currentWord);
            }
        }

        [ContextMenu("Print All Combinations")]
        private void PrintAllCombinations()
        {
            foreach (var word in AllCombinations)
            {
                GameCommonData.ShowDebugLog($"Found word: {word}", 0);
            }
        }
    }
}
