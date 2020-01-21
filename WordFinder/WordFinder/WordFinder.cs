using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordFinder
{
    public class WordFinder : IWordFinder
    {
        int matrixSize = 64;
        private readonly IEnumerable<string> MatrixEnumerable;

        //All strings contain the same number of characters
        public WordFinder(IEnumerable<string> matrix)
        {
            this.MatrixEnumerable = matrix;
        }

        public Trie _trie { get; private set; }

        //The "Find" method should return the top 10 most repeated words from the word stream found in the matrix
        public IEnumerable<string> Find(IEnumerable<string> dictionary)
        {
            _trie = new Trie(dictionary.ToArray());

            List<string> wordsFound = new List<string>();
            var matrix = new string[matrixSize, matrixSize];

            int colCounter = 0;
            int rowCounter = 0;
            foreach (var letter in MatrixEnumerable)
            {
                matrix[rowCounter, colCounter] = letter;
                colCounter++;
                if (colCounter > matrixSize - 1)
                {
                    //reset column and move row
                    colCounter = 0;
                    rowCounter++;
                }
            }

            //Loop rows and columns from LTR and top to bottom
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    WordsLookup(_trie.Root, matrix, i, j, wordsFound);
                }
            }
            Dictionary<string, int> repeatedWords = new Dictionary<string, int>();
            foreach (var wordFound in wordsFound)
            {
                if (!repeatedWords.ContainsKey(wordFound))
                {
                    repeatedWords.Add(wordFound, 1);
                }
                else
                {
                    var ocurrences = repeatedWords[wordFound];
                    repeatedWords[wordFound] = ocurrences + 1;
                }
            }
            return repeatedWords.OrderByDescending(x => x.Value).Select(x => x.Key).Take(10);
        }

        /// <summary>
        /// Recursive method
        /// </summary>
        /// <param name="n"></param>
        /// <param name="matrix"></param>
        /// <param name="currentRowIndex"></param>
        /// <param name="currentColIndex"></param>
        /// <param name="wordsFound"></param>
       void WordsLookup(Trie.Node n, string[,] matrix, int currentRowIndex, int currentColIndex, List<string> wordsFound)
        {
            if (currentColIndex < matrix.GetLength(1) && currentRowIndex < matrix.GetLength(0))
            {
                foreach (var branch in n.Branches)
                {
                    if (matrix[currentRowIndex, currentColIndex].Contains(branch.Key))
                    {
                        if (branch.Value.IsLeaf)
                        {
                            wordsFound.Add(branch.Value.Word);
                        }
                        WordsLookup(branch.Value, matrix, currentRowIndex, currentColIndex + 1, wordsFound);
                        WordsLookup(branch.Value, matrix, currentRowIndex + 1, currentColIndex, wordsFound);
                    }
                }
            }
        }
    }
}

