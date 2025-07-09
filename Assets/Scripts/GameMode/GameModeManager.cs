using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WordJam
{
    public abstract class GameModeManager : MonoBehaviour
    {
        internal GameInstanceManager GameInstanceManager;

        [Header("Game Mode Settings")]
        public GameModeEnum CurrentGameMode;
        public LevelData CurrentLevelData;

        [Header("Grid Settings")]
        public GridLayoutGroup GridLayoutGroup;
        public GameObject GridParentPrefab;

        [SerializeField]
        private List<GameObject> gridParentList;

        [Header("Tile Settings")]
        public Tile TilePrefab;
        [SerializeField]
        private List<Tile> tileList;

        [Header("Line Renderer")]
        public LineRenderer LineRenderer;

        private LinkedList<Tile> SelectedTiles = new();
        public List<string> TileIndexList = new();
        [SerializeField]
        private Graph TileGraph = new();
        private int LastSelectedTileIndex = -1;

        private int totalGridCount = 0;

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
        }

        internal virtual void SpawnGridParent()
        {
            for (int i = 0; i < totalGridCount; i++)
            {
                GameObject gridParent = Instantiate(GridParentPrefab, GridLayoutGroup.transform);
                gridParent.name = $"GridParent_{i}";
                gridParentList.Add(gridParent);
            }
        }

        internal virtual void SpawnTiles()
        {
            List<GridData> gridData = new(CurrentLevelData.gridData);

            for (int i = 0; i < totalGridCount; i++)
            {
                Tile tile = Instantiate(TilePrefab, gridParentList[i].transform);
                tile.name = $"Tile_{i}";
                tile.SetTileText(gridData[i].letter);
                tile.TileIndex = i;
                tile.OnTileSelected += OnTileSelected;

                tileList.Add(tile);
            }
        }

        internal virtual void CreateTileGraph()
        {
            for (int i = 0; i < totalGridCount; i++)
            {
                TileGraph.Add(i, GetAdjacentElements(i, CurrentLevelData.gridSize.x, CurrentLevelData.gridSize.y));
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

        private void OnTileSelected(Tile tile)
        {
            if (tile.IsSelected)
            {
                // TODO : ADD LOGIC WHEN SELECTING AGAIN
                return;
            }

            if (SelectedTiles.Contains(tile))
            {
                Debug.Log($"Tile Already Selected : {tile.TileIndex} : {GameCommonData.AlphabetToWordMap[tile.LetterIndex]}");
                return;
            }

            if (LastSelectedTileIndex == tile.TileIndex) return;

            if (LastSelectedTileIndex != -1)
            {
                if (!TileGraph.GetIsNodeAdjacent(tile.TileIndex, LastSelectedTileIndex))
                {
                    Debug.Log($"On Selected Tile Is Not Adjacent : {tile.TileIndex} : {GameCommonData.AlphabetToWordMap[tile.LetterIndex]}");
                    return;
                }
            }

            LastSelectedTileIndex = tile.TileIndex;
            // tile.IsSelected = true;

            LineRenderer.positionCount++;
            Vector3[] positions = new Vector3[LineRenderer.positionCount];
            LineRenderer.GetPositions(positions);
            positions[LineRenderer.positionCount - 1] = transform.InverseTransformPoint(tile.transform.position);
            LineRenderer.SetPositions(positions);

            SelectedTiles.AddLast(tile);
            TileIndexList.Add(GameCommonData.AlphabetToWordMap[tile.LetterIndex]);
            Debug.Log($"On Selected : {LastSelectedTileIndex} : {GameCommonData.AlphabetToWordMap[tile.LetterIndex]}");
        }

        [ContextMenu("Check Selected")]
        public void Editor_CheckSelected()
        {
            StringBuilder stringBuilder = new();
            Debug.Log($"Linked List Count : {SelectedTiles.Count}");
            var node = SelectedTiles.First;
            while (node != null)
            {
                string alphabet = GameCommonData.AlphabetToWordMap[node.Value.LetterIndex];
                stringBuilder.Append(alphabet);
                Debug.Log($"Linked List Index : {alphabet}");
                node = node.Next;
            }

            string ToCheckWord = stringBuilder.ToString();
            SelectedTiles.Clear();
            TileIndexList.Clear();
            LastSelectedTileIndex = -1;

            LineRenderer.positionCount = 0;

            if (GameInstanceManager == null) return;

            bool exists = GameInstanceManager.AllWordsTrie.Contains(ToCheckWord);
            Debug.Log($"Does the word '{ToCheckWord}' exist? {exists}");
        }
    }
}
