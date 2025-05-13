using System.Data;
using WordFinderApp.Interfaces;

namespace WordFinderApp.Services
{
    public class WordFinder : IWordFinder
    {
        private readonly string[] _matrix;
        private readonly int _matrixSize;

        public WordFinder(IEnumerable<string> matrix)
        {
            if (matrix == null) throw new ArgumentNullException(nameof(matrix));
            _matrix = [.. matrix.Select(row => row.ToUpperInvariant())];
            _matrixSize = _matrix.Length;
            if (_matrix.Any(r => r.Length != _matrixSize)) throw new ArgumentException("Rows must have same length.");
        }
        public IEnumerable<string> Find(IEnumerable<string> wordStream, int numberRankingPositions)
        {
            if (wordStream == null) throw new ArgumentNullException(nameof(wordStream));
            var wordFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//Case-Insensitive.
            var uniqueWords = wordStream
                          .Where(word => !string.IsNullOrWhiteSpace(word))
                          .Select(word => word.ToUpperInvariant())//Normalized
                          .Distinct();//Avoid duplicated words

            foreach (var word in uniqueWords)
            {
                int wordLength = word.Length;
                int count = 0;
                for (int row = 0; row < _matrixSize; row++)
                {
                    for (int column = 0; column < _matrixSize; column++)
                    {
                        if (column <= _matrixSize - wordLength && IsThereHoriazontalMatch(row, column, word)) count++;
                        if (row >= _matrixSize - wordLength && IsThereVerticalMatch(row, column, word)) count++;
                    }
                }
                if (count > 0) wordFrequency[word] = count;
            }

            return [.. wordFrequency.OrderByDescending(keyValuePair => keyValuePair.Value) //Por cantidad de repeticiones
                                    .ThenBy(keyValuePair => keyValuePair.Key) //Desempate por palabra alfabeticamente
                                    .Take(numberRankingPositions) //Tomo las n posiciones pedidas
                                    .Select(keyValuePair => keyValuePair.Key)]; //Devuelvo la lista.
        }

        private bool IsThereHoriazontalMatch(int row, int column, string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (CharsEqual(_matrix[row][column + i], word[i])) return false;
            }
            return true;
        }

        private bool IsThereVerticalMatch(int row, int column, string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (CharsEqual(_matrix[row + i][column], word[i])) return false;
            }
            return true;
        }
        
        private static bool CharsEqual(char a, char b) => a == b;
    }
}
