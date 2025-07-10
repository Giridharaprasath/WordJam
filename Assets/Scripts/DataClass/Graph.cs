using System;
using System.Collections.Generic;

namespace WordJam
{
    [Serializable]
    public class Graph
    {
        public IDictionary<int, List<int>> Nodes = new Dictionary<int, List<int>>();

        public void Add(int index, List<int> adjacentNodes)
        {
            Nodes.Add(index, adjacentNodes);
        }

        public bool CheckIsNodeAdjacent(int node, int adjacentNode)
        {
            if (Nodes.TryGetValue(node, out List<int> adjacentNodes))
            {
                return adjacentNodes.Contains(adjacentNode);
            }

            return false;
        }
    }
}
