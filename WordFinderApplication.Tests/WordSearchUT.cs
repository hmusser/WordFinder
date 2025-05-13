using FluentAssertions;
using WordFinderApp.Interfaces;
using WordFinderApp.Services;
namespace WordFinderApplication.Tests
{
    public class WordSearchUT
    {

        [Fact]
        public void ShouldFindAHorizontallyWord()
        {
            var testMatrix = new List<string>
            {
                "COLD",
                "WIND",
                "SNOW",
                "CHILL"
            };
            
            IWordFinder wordFinder = new WordFinder(testMatrix);

            var wordStream = new List<string> { "COLD" };

            var result = wordFinder.Find(wordStream, 10);

            result.Should().ContainSingle().Which.Should().Be("COLD");
        }
    }
}