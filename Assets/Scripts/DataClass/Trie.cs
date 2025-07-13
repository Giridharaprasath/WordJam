using System;
using System.Collections.Generic;

namespace WordJam
{
    [Serializable]
    public class Trie
    {
        [Serializable]
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

        public List<string> StartsWith(string prefix)
        {
            List<string> res = new();

            TrieNode current = root;
            foreach (char child in prefix)
            {
                if (current.children.ContainsKey(child))
                {
                    current = current.children[child];
                }
                else
                {
                    return res;
                }
            }

            StartsWith(current, prefix, res);
            return res;
        }

        private void StartsWith(TrieNode current, string prefix, List<string> words)
        {
            if (current.isWord)
            {
                words.Add(prefix);
            }

            foreach (char key in current.children.Keys)
            {
                StartsWith(current.children[key], prefix + key, words);
            }
        }
    }
}
