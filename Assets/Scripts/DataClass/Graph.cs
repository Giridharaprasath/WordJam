using System.Collections.Generic;

namespace WordJam
{
    [System.Serializable]
    public class Graph
    {
        [System.Serializable]
        public class Node
        {
            public int Index;
            public List<int> AdjacentNodex = new();
        }

        public IDictionary<int, List<int>> Nodes = new Dictionary<int, List<int>>();
        public List<Node> LNodes = new();

        public void Add(int index, List<int> adjacentNodes)
        {
            LNodes.Add(new Node()
            {
                Index = index,
                AdjacentNodex = adjacentNodes
            });
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

        public bool GetIsNodeAdjacent(int node, int adjacentNode)
        {
            Node currentNode = LNodes.Find(x => x.Index == node);
            return currentNode.AdjacentNodex.Contains(adjacentNode);
        }
    }
}
