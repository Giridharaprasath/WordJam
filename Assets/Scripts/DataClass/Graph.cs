using System;
using System.Collections.Generic;

namespace WordJam
{
    /// <summary>
    /// Graph class representing a collection of nodes and their adjacent nodes.
    /// This class provides methods to add nodes, check adjacency, retrieve adjacent nodes,
    /// and perform depth-first search (DFS) traversal.
    /// This class is used to manage the relationships between the tiles in the game.
    /// Each tile is represented as a node, and the connections between tiles are represented as edges.
    /// </summary>
    [Serializable]
    public class Graph
    {
        private IDictionary<int, List<int>> Nodes = new Dictionary<int, List<int>>();

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

        public List<int> GetAdjacentNodes(int node)
        {
            return Nodes[node];
        }

        public List<int> GetDFSPath(int node)
        {
            List<int> path = new();
            HashSet<int> visited = new();
            Stack<int> stack = new();

            stack.Push(node);
            while (stack.Count > 0)
            {
                int currentNode = stack.Pop();
                if (!visited.Contains(currentNode))
                {
                    visited.Add(currentNode);
                    path.Add(currentNode);

                    if (Nodes.TryGetValue(currentNode, out List<int> adjacentNodes))
                    {
                        foreach (int adjacent in adjacentNodes)
                        {
                            if (!visited.Contains(adjacent))
                            {
                                stack.Push(adjacent);
                            }
                        }
                    }
                }
            }
            
            return path;
        }
    }
}
