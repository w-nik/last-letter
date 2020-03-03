using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace Game.UnitTests
{
    public class BotTests
    {
        private readonly Mock<IDictionaryProvider> _providerMock = new Mock<IDictionaryProvider>();

        [Fact]
        public void Ctor_PassesDictionaryLocationIntoProvider()
        {
            // Arrange
            var location = "dictionaryLocation";
            var dict = new Dictionary<string, IList<string>>
            {
                {"W", new List<string> {"world"}}
            };
            _providerMock.Setup(p => p.GetDictionary(It.IsAny<string>())).Returns(dict);

            // Act
            var bot = new Bot(_providerMock.Object, location);

            // Assert
            _providerMock.Verify(p => p.GetDictionary(location));
        }

        [Fact]
        public void FirstMove_NextWord_ReturnsWordFromDictionary()
        {
            // Arrange
            var dict = new Dictionary<string, IList<string>>
            {
                { "H", new List<string> { "hello" } },
                { "W", new List<string> { "world" } }
            };
            _providerMock.Setup(p => p.GetDictionary(It.IsAny<string>())).Returns(dict);
            var bot = new Bot(_providerMock.Object, "dictionaryLocation");

            // Act
            var message = bot.NextWord("");

            // Assert
            string.IsNullOrEmpty(message.Text).Should().BeFalse();
            message.Status.Should().Be(Status.Accept);
        }

        [Fact]
        public void WordRejected_ReturnsEmptyString()
        {
            // Arrange
            var rejected = "hello";
            var dict = new Dictionary<string, IList<string>>
            {
                { "H", new List<string> { rejected } },
            };
            _providerMock.Setup(p => p.GetDictionary(It.IsAny<string>())).Returns(dict);

            var bot = new Bot(_providerMock.Object, "dictionaryLocation");

            // Act
            bot.WordRejected(rejected);
            var message = bot.NextWord("foo");
            
            // Assert
            message.Text.Should().Be(string.Empty);
            message.Status.Should().Be(Status.GiveUp);
        }

        [Fact]
        public void NextWord_ReturnsWordByLastLetter()
        {
            // Arrange
            var expectedWord = "ahoy";
            var dict = new Dictionary<string, IList<string>>
            {
                { "A", new List<string> { expectedWord } },
                { "H", new List<string> { "hello", "hi" } },
                { "M", new List<string> { "morning" } },
                { "W", new List<string> { "world" } }
            };
            _providerMock.Setup(p => p.GetDictionary(It.IsAny<string>())).Returns(dict);
            var bot = new Bot(_providerMock.Object, "dictionaryLocation");

            // Act
            bot.WordAccepted("lambda");
            var message = bot.NextWord("");

            // Assert
            message.Text.Should().Be(expectedWord);
            message.Status.Should().Be(Status.Accept);
        }
    }
}
