using System.Data;
using WordFinderApp.Interfaces;

namespace WordFinderApp.Services
{
    public class WordFinder : IWordFinder
    {
        private readonly string[] _matrix;
        private readonly int _matrixSize;
        private const int MATRIX_MAX_SIZE = 64;
        public WordFinder(IEnumerable<string> matrix)
        {
            if (matrix == null) throw new ArgumentNullException(nameof(matrix));
            if (matrix.Count() > MATRIX_MAX_SIZE || matrix.Any(row => row.Length > MATRIX_MAX_SIZE))
            { 
                throw new ArgumentException($"Matrix cannot exceed {MATRIX_MAX_SIZE} dimensions."); 
            }
            _matrix = [.. matrix.Select(row => row.ToUpperInvariant())];
            _matrixSize = _matrix.Length;
            if (_matrix.Any(r => r.Length != _matrixSize)) throw new ArgumentException("Rows must have same length.");
        }
        public IEnumerable<string> Find(IEnumerable<string> wordStream, int numberRankingPositions)
        {
            if (wordStream == null) throw new ArgumentNullException(nameof(wordStream));
            var wordFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var uniqueWords = wordStream
                          .Where(word => !string.IsNullOrWhiteSpace(word))
                          .Select(word => word.ToUpperInvariant())
                          .Distinct();

            foreach (var word in uniqueWords)
            {
                int wordLength = word.Length;
                int count = 0;
                for (int row = 0; row < _matrixSize; row++)
                {
                    for (int column = 0; column < _matrixSize; column++)
                    {
                        if (column <= _matrixSize - wordLength && IsThereHoriazontalMatch(row, column, word)) count++;
                        if (row <= _matrixSize - wordLength && IsThereVerticalMatch(row, column, word)) count++;
                    }
                }
                if (count > 0) wordFrequency[word] = count;
            }

            return [.. wordFrequency.OrderByDescending(keyValuePair => keyValuePair.Value)
                                    .ThenBy(keyValuePair => keyValuePair.Key) 
                                    .Take(numberRankingPositions) 
                                    .Select(keyValuePair => keyValuePair.Key)]; 
        }

        private bool IsThereHoriazontalMatch(int row, int column, string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (!CharsEqual(_matrix[row][column + i], word[i])) return false;
            }
            return true;
        }

        private bool IsThereVerticalMatch(int row, int column, string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (!CharsEqual(_matrix[row + i][column], word[i])) return false;
            }
            return true;
        }
        
        private static bool CharsEqual(char a, char b) => a == b;
    }
}
