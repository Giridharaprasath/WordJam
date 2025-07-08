using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordJam
{
    [SerializeField]
    public class GridSize
    {
        public int X = 0;
        public int Y = 0;

        public GridSize(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }
    }

    [SerializeField]
    public class LevelData
    {
        public int BugCount = 0;
        public int WordCount = 0;
        public int TimeSeconds = 0;
        public GridSize GridSize = new();
    }
}
