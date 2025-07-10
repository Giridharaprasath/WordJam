using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WordJam
{
    public abstract class GameModeManager : MonoBehaviour
    {
        protected GameInstanceManager GameInstanceManager;
        protected GameUIManager GameUIManager;

        [Header("Game Mode Settings")]
        protected GameModeEnum CurrentGameMode;
        protected LevelData CurrentLevelData;
        protected int CurrentLevelNumber = 1;

        [Header("Grid Settings")]
        public GridLayoutGroup GridLayoutGroup;
        public GameObject GridParentPrefab;
        protected List<GameObject> gridParentList = new();

        [Header("Tile Settings")]
        public Tile TilePrefab;
        protected List<Tile> tileList = new();
        protected LinkedList selectedTilesList = new();

        [Header("Line Renderer")]
        public LineRenderer LineRenderer;

        // * Private Variables
        protected Trie selectedWords = new();
        protected Graph tileGraph = new();
        protected int lastSelectedTileIndex = -1;
        protected int totalGridCount = 0;
        protected int currentWordCount = 0;
        protected int currentScore = 0;
        protected float currentTimeLeft = -1;
        protected bool isDragging = false;
        protected bool hasTimer = false;

        private int minutes, seconds;

        protected virtual void Awake()
        {
            GameInstanceManager = GameInstanceManager.Instance;
            GridLayoutGroup.constraintCount = CurrentLevelData.gridSize.x;
        }

        protected virtual void Start()
        {
            totalGridCount = CurrentLevelData.gridSize.x * CurrentLevelData.gridSize.y;

            SpawnGridParent();
            SpawnTiles();
            CreateTileGraph();
            SetGameModeUI();
        }

        protected virtual void Update()
        {
            if (isDragging)
            {
                if (Input.touchCount == 0)
                {
                    isDragging = false;
                    CheckSelectedWord();
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    isDragging = true;
                }
            }

            if (hasTimer)
            {
                SetCurrentTimeLeft();
            }
        }

        protected void SetCurrentTimeLeft()
        {
            if (currentTimeLeft <= 0)
            {
                LoadSceneAgain();
                hasTimer = false;
                return;
            }

            currentTimeLeft -= Time.deltaTime;
            SetTimeLeftText();
        }

        protected virtual void SpawnGridParent()
        {
            for (int i = 0; i < totalGridCount; i++)
            {
                GameObject gridParent = Instantiate(GridParentPrefab, GridLayoutGroup.transform);
                gridParent.name = $"GridParent_{i}";
                gridParentList.Add(gridParent);
            }
        }

        protected virtual void SpawnTiles()
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

        protected virtual void CreateTileGraph()
        {
            for (int i = 0; i < totalGridCount; i++)
            {
                tileGraph.Add(i, GetAdjacentElements(i, CurrentLevelData.gridSize.x, CurrentLevelData.gridSize.y));
            }
        }

        protected virtual void SetGameModeUI()
        {
            if (GameUIManager == null)
            {
                GameUIManager = FindAnyObjectByType<GameUIManager>();
            }

            if (CurrentLevelData.wordCount > 0)
            {
                GameUIManager.SetWordCountLeftState(true);
                SetWordCountLeftText();
            }
            if (CurrentLevelData.totalScore > 0)
            {
                GameUIManager.SetScoreState(true);
                SetScoreText();
            }
            if (CurrentLevelData.timeSec > 0)
            {
                currentTimeLeft = CurrentLevelData.timeSec;
                GameUIManager.SetTimeLeftState(true);
                SetTimeLeftText();
                hasTimer = true;
            }
        }

        private List<int> GetAdjacentElements(int index, int row, int column)
        {
            // TODO : FIND ANOTHER WAY TO GET ADJACENT ELEMENTS
            List<int> AdjacentNodes = new();

            int x = index % row;
            int y = index / row;

            int[,] directions = new int[,]
            {
                { -1, -1 }, { 0, -1 }, { 1, -1 },
                { -1,  0 },           { 1,  0 },
                { -1,  1 }, { 0,  1 }, { 1,  1 }
            };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = x + directions[i, 0];
                int newY = y + directions[i, 1];

                if (newX >= 0 && newX < row && newY >= 0 && newY < column)
                {
                    int adjacentIndex = newY * row + newX;
                    AdjacentNodes.Add(adjacentIndex);
                }
            }

            return AdjacentNodes;
        }

        protected virtual void OnTileSelected(Tile tile)
        {
            if (selectedTilesList.Contains(tile.TileIndex))
            {
                Debug.Log($"Tile Already Selected : {tile.TileIndex} : {GameCommonData.AlphabetToWordMap[tile.LetterIndex]}");
                SetToLastSelectedTile(tile);
                return;
            }

            if (lastSelectedTileIndex == tile.TileIndex) return;

            if (lastSelectedTileIndex != -1)
            {
                if (!tileGraph.CheckIsNodeAdjacent(tile.TileIndex, lastSelectedTileIndex))
                {
                    Debug.Log($"On Selected Tile Is Not Adjacent : {tile.TileIndex} : {GameCommonData.AlphabetToWordMap[tile.LetterIndex]}");
                    return;
                }
            }

            lastSelectedTileIndex = tile.TileIndex;

            LineRenderer.positionCount++;
            Vector3[] positions = new Vector3[LineRenderer.positionCount];
            LineRenderer.GetPositions(positions);
            positions[LineRenderer.positionCount - 1] = transform.InverseTransformPoint(tile.transform.position);
            LineRenderer.SetPositions(positions);

            selectedTilesList.Add(tile.TileIndex);
            Debug.Log($"On Selected : {lastSelectedTileIndex} : {GameCommonData.AlphabetToWordMap[tile.LetterIndex]}");
        }

        private void SetToLastSelectedTile(Tile tile)
        {
            lastSelectedTileIndex = tile.TileIndex;

            int index = selectedTilesList.FindIndex(tile.TileIndex);
            if (index == -1)
            {
                Debug.LogWarning($"Last Selected Tile Does Not Exists!!");
                return;
            }

            LineRenderer.positionCount = index + 1;
            Vector3[] positions = new Vector3[LineRenderer.positionCount];
            LineRenderer.GetPositions(positions);
            LineRenderer.SetPositions(positions);

            selectedTilesList.RemoveFrom(tile.TileIndex);
        }

        protected virtual void CheckSelectedWord()
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
            }

            selectedTilesList.Clear();
            lastSelectedTileIndex = -1;
            LineRenderer.positionCount = 0;
        }

        protected virtual void CalculateScore()
        {
            List<int> tileIndex = selectedTilesList.GetData();

            foreach (int index in tileIndex)
            {
                currentScore += tileList[index].GetTileScorePoints();
            }
        }

        protected virtual void CheckWin()
        {
            SetScoreText();
        }

        protected void LoadSceneAgain()
        {
            GameInstanceManager.LoadLevelMode();
        }

        protected virtual void SetWordCountLeftText()
        {
            GameUIManager.SetWordCountLeftText(CurrentLevelData.wordCount - currentWordCount);
        }

        protected virtual void SetScoreText()
        {
            string totalScore = currentScore.ToString();
            if (CurrentLevelData.totalScore > 0)
                totalScore += " / " + CurrentLevelData.totalScore;

            GameUIManager.SetScoreText(totalScore);
        }

        protected virtual void SetTimeLeftText()
        {
            minutes = (int)currentTimeLeft / 60;
            seconds = (int)currentTimeLeft % 60;
            GameUIManager.SetTimeLeftText($"{minutes} : {seconds}");
        }
    }
}
