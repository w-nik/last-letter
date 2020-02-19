using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Game.UnitTests
{
    public class BotTests
    {
        [Fact]
        public void FirstMove_NextWord_ReturnsWordFromDictionary()
        {
            // Arrange
            var dict = new List<string> {"hello", "world"};
            var bot = new Bot(dict);

            // Act
            var word = bot.NextWord();

            // Assert
            string.IsNullOrEmpty(word).Should().BeFalse();
        }

        [Fact]
        public void WordRejected_ReturnsEmptyString()
        {
            // Arrange
            var rejected = "hello";
            var dict = new List<string> {rejected};
            var bot = new Bot(dict);

            // Act
            bot.WordRejected(rejected);
            var word = bot.NextWord();
            
            // Assert
            word.Should().Be(string.Empty);
        }

        [Fact]
        public void NextWord_ReturnsWordByLastLetter()
        {
            // Arrange
            var expectedWord = "ahoy";
            var dict = new List<string> { "hello", expectedWord, "hi", "morning" };
            var bot = new Bot(dict);

            // Act
            bot.WordAccepted("lambda");
            var word = bot.NextWord();

            // Assert
            word.Should().Be(expectedWord);
        }
    }
}
