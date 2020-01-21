using System.Collections.Generic;

namespace WordFinder
{
    public class Trie
    {
        public class Node
        {
            public string Word { get; set; }
            public bool IsLeaf { get; set; }
            public Dictionary<char, Node> Branches = new Dictionary<char, Node>();
        }
        public Node Root = new Node();
        public Trie(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i];
                var node = Root;
                for (int j = 0; j < word.Length; j++)
                {
                    var letter = word[j];
                    if (!node.Branches.TryGetValue(letter, out Node next))
                    {
                        next = new Node();
                        if (j + 1 == word.Length)
                        {
                            next.Word = word;
                            next.IsLeaf = true;
                        }
                        node.Branches.Add(letter, next);
                    }
                    node = next;
                }
            }
        }
    }
}

