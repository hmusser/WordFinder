namespace WordFinderApp.Interfaces
{
    public interface IWordFinder
    {
        IEnumerable<string> Find(IEnumerable<string> wordStream, int numberRankingPositions);
    }
}
