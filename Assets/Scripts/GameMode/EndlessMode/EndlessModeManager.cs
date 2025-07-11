using System.Collections;
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

        protected override void Awake()
        {
            CurrentGameMode = GameModeEnum.EndlessMode;

            CurrentLevelData = new()
            {
                gridSize = new(4, 4),
                gridData = new()
                {
                    new GridData { tileType = 0, letter = "G" },
                    new GridData { tileType = 0, letter = "A" },
                    new GridData { tileType = 0, letter = "M" },
                    new GridData { tileType = 0, letter = "E" },
                    new GridData { tileType = 0, letter = "W" },
                    new GridData { tileType = 0, letter = "O" },
                    new GridData { tileType = 0, letter = "R" },
                    new GridData { tileType = 0, letter = "D" },
                    new GridData { tileType = 0, letter = "J" },
                    new GridData { tileType = 0, letter = "A" },
                    new GridData { tileType = 0, letter = "M" },
                    new GridData { tileType = 0, letter = "S" },
                    new GridData { tileType = 0, letter = "B" },
                    new GridData { tileType = 0, letter = "U" },
                    new GridData { tileType = 0, letter = "G" },
                    new GridData { tileType = 0, letter = "S" },
                }
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
                tile.SetTileText(gridData[i]);
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
            Invoke(nameof(HideBlockScreen), moveSpeed + 0.25f);
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
                        tileTransform.DOLocalMove(Vector3.zero, moveSpeed).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            tileTransform.localPosition = Vector3.zero;
                        });
                        tileList[columnIndices[j]].TileIndex = columnIndices[i];
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
                tileTransform.DOLocalMove(Vector3.zero, moveSpeed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    tileTransform.localPosition = Vector3.zero;
                });
                tileTransform.GetComponent<Tile>().TileIndex = index;
            }
        }

        private void SortTilesByIndex()
        {
            tileList.Sort((x, y) => x.TileIndex.CompareTo(y.TileIndex));
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
    }
}
