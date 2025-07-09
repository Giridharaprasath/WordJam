using System;
using System.Collections.Generic;

namespace WordJam
{
    [Serializable]
    public class GridSize
    {
        public int x = 0;
        public int y = 0;

        public GridSize(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }
    }

    [Serializable]
    public class GridData
    {
        public int tileType;
        public string letter;
    }

    [Serializable]
    public class LevelData
    {
        public int bugCount = 0;
        public int wordCount = 0;
        public int timeSec = 0;
        public int totalScore = 0;
        public GridSize gridSize = new();
        public List<GridData> gridData = new();
    }
}
