using System.Data;
using WordFinderApp.Interfaces;

namespace WordFinderApp.Services
{
    public class WordFinder : IWordFinder
    {
        private readonly char[,] _matrix;
        private readonly int _matrixSize;

        public WordFinder(IEnumerable<string>  matrix)
        {
            var matrixLines = matrix.ToArray();
            _matrixSize = matrixLines.Length;
            _matrix = new char[_matrixSize, _matrixSize];
            //Set each data position of the matrix
            for (int currentRow = 0; currentRow < _matrixSize; currentRow++)
            {
                for (int currentColumn = 0; currentColumn < _matrixSize; currentColumn++)
                {
                    _matrix[currentRow, currentColumn] = matrixLines[currentRow][ currentColumn];
                }
            }
        }
        public IEnumerable<string> Find(IEnumerable<string> wordStream, int numberRankingPositions)
        {
          var wordFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//Case-Insensitive.
          var uniqueWords = wordStream.Distinct(StringComparer.OrdinalIgnoreCase);//Avoid duplicated words
        
            foreach ( var word in uniqueWords )
            {
                int count = CountWordInMatrix(word);
                if(count>0)
                {
                    wordFrequency[word] = count;    
                }
            }

            return wordFrequency.OrderByDescending(keyValuePair => keyValuePair.Value) //Por cantidad de repeticiones
                                .ThenBy(keyValuePair => keyValuePair.Key) //Desempate por palabra alfabeticamente
                                .Take(numberRankingPositions) //Tomo las n posiciones pedidas
                                .Select(keyValuePair => keyValuePair.Key); //Devuelvo la lista.
        }

        public int CountWordInMatrix(string word)
        {
            var wordLength = word.Length;
            int count = 0;
            for (int currentRow = 0; currentRow < _matrixSize; currentRow++)
            {
                for (int currentColumn = 0; currentColumn < _matrixSize; currentColumn++)
                {
                  //  if (currentColumn <= _matrixSize - wordLength && IsThereHorizontalMatch(currentRow, currentColumn, word)) count++;
                  //  if (currentRow <= _matrixSize - wordLength && && IsThereVerticalMatch(currentRow, currentColumn, word)) count++;
                }
            }
            return count;
        }

        public bool IsThereHoriazontalMatch(int row, int column, string word)
        {
            for(int i = 0; i < word.Length; i++ )
            {
                //if (CharsEqual(_matrix[row, column + i], word[i])) return false;
            }
            return true;
        }


    }
}
