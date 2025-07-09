using System.Collections.Generic;

namespace WordJam
{
    [System.Serializable]
    public class Trie
    {
        [System.Serializable]
        public class TrieNode
        {
            public Dictionary<char, TrieNode> children;
            public bool isWord;

            public TrieNode()
            {
                children = new Dictionary<char, TrieNode>();
                isWord = false;
            }
        }

        private readonly TrieNode root;

        public Trie()
        {
            root = new TrieNode();
        }

        public void Add(string word)
        {
            TrieNode current = root;
            foreach (char child in word)
            {
                var upperChild = char.ToUpper(child);
                if (!current.children.ContainsKey(upperChild))
                {
                    TrieNode tmp = new();
                    current.children.Add(upperChild, tmp);
                }
                current = current.children[upperChild];
            }
            current.isWord = true;
        }

        public bool Contains(string word)
        {
            TrieNode current = root;
            foreach (char child in word)
            {
                var upperChild = char.ToUpper(child);
                if (current.children.ContainsKey(upperChild))
                {
                    current = current.children[upperChild];
                }
                else
                {
                    return false;
                }
            }
            return current.isWord;
        }
    }
}
