using FluentAssertions;
using WordFinderApp.Services;
namespace WordFinderApplication.Tests.Services
{
    public class WordSearchUT
    {
        private const int NUMBER_RANKING_POSITIONS = 10;
        private static readonly string[] Matrix =
        [
            "abcdc",
            "fgwio",
            "chill",
            "pqnsd",
            "uxdxy"
        ];

        [Theory]
        [InlineData(new[] { "wind", "snow", "storm" }, new[] { "wind" })]
        [InlineData(new[] { "cold", "wind", "snow", "chill" }, new[] { "cold", "wind", "chill" })]
        [InlineData(new[] { "cold", "cold", "chill", "chill" }, new[] { "chill", "cold" })]
        public void FindTopTenWords_ShouldReturn_ExpectedWords(string[] wordStream, string[] expectedWordMatches)
        {
            WordFinder wordFinder = new WordFinder(Matrix);

            var result = wordFinder.Find(wordStream, NUMBER_RANKING_POSITIONS);

            result.Should().BeEquivalentTo(expectedWordMatches.Select(w => w.ToUpperInvariant()), options => options.WithoutStrictOrdering());
        }

        [Theory]
        [InlineData(new[] { "apple", "banana", "zebra" }, new string[0])]
        public void FindTopTenWords_ShouldReturn_EmptyList(string[] wordStream, string[] expectedWordMatches)
        {
            WordFinder wordFinder = new WordFinder(Matrix);

            var result = wordFinder.Find(wordStream, NUMBER_RANKING_POSITIONS);

            result.Should().BeEquivalentTo(expectedWordMatches.Select(w => w.ToUpperInvariant()), options => options.WithoutStrictOrdering());
        }


        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenMatrixSizeIsBiggerThan64()
        {
            var longRow = new string('B', 65);
            var matrix = Enumerable.Repeat(longRow, 10);

            Action act = () => new WordFinder(matrix);

            act.Should().Throw<ArgumentException>().WithMessage("*Matrix cannot exceed*");
        }

        [Fact]
        public void FindTopTenWords_ShouldRetur_ATenWordsRanked_ByOccurrences()
        {
            var matrix = new[]
            {
                "applebanan",
                "bananaappl",
                "cucumberki",
                "kiwicucumb",
                "mangoorang",
                "grapemelon",
                "melonpeach",
                "peachmelon",
                "lemonapple",
                "bananaoran"
            };
            var wordStream = new[]
            {
                "apple",
                "banana",
                "cucumber",
                "kiwi",
                "nana",
                "each",
                "grape",
                "melon",
                "peach",
                "melon",
                "leban"
            };
            var expectedWordRanking = new[]
            {
                "melon",    //3 times
                "apple",    //2 times
                "banana",   //2 times
                "each",     //2 times
                "nana",     //2 times
                "peach",    //2 times
                "cucumber", //1 time
                "grape",    //1 time
                "kiwi",     //1 time
                "leban"     //1 time
            };

            WordFinder wordFinder = new WordFinder(matrix);
            var result = wordFinder.Find(wordStream, NUMBER_RANKING_POSITIONS);

            result.Should().BeEquivalentTo(expectedWordRanking.Select(w => w.ToUpperInvariant()), options => options.WithStrictOrdering());
        }

        [Theory]
        [MemberData(nameof(LargeWordStreamData))]
        public void FindTopTenWords_ShouldReturn_CHILL_FromLargeStreamData(string[] wordStream, string[] expectedWordMatches)
        {
            WordFinder wordFinder = new WordFinder(Matrix);

            var result = wordFinder.Find(wordStream, NUMBER_RANKING_POSITIONS);

            result.Should().BeEquivalentTo(expectedWordMatches.Select(w => w.ToUpperInvariant()), options => options.WithoutStrictOrdering());
        }
        public static IEnumerable<object[]> LargeWordStreamData =>
        [
            [GenerateLargeWordStreamWords(),new[] { "chill" }]
        ];
        public static string[] GenerateLargeWordStreamWords()
        {
            var list = Enumerable.Repeat("challenge", 100000).ToList();
            list.Add("chill");
            return [.. list];
        }
    }
}