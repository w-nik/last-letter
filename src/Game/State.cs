using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameOfWords
{
    [JsonObject]
    public class State
    {
        [JsonProperty]
        private readonly WordDictionary _dictionary;
        [JsonProperty]
        private readonly List<string> _acceptedWords = new List<string>();
        [JsonProperty]
        private readonly List<string> _rejectedWords = new List<string>();

        [JsonConstructor]
        private State() { }

        public State(WordDictionary dictionary)
        {
            this._dictionary = dictionary;
        }

        public string LastWord => _acceptedWords.LastOrDefault();

        public bool Verify(string word)
        {
            var verified = _dictionary.Verify(word);
            var notUsed = !_acceptedWords.Contains(word) && !_rejectedWords.Contains(word);

            return verified && notUsed;
        }

        public Task WordAccepted(string player, string word)
        {
            _acceptedWords.Add(word);

            return Task.CompletedTask;
        }

        public Task WordRejected(string player, string word)
        {
            _rejectedWords.Add(word);

            if (_acceptedWords.Any() && _acceptedWords.Last() == word)
                _acceptedWords.Remove(word);

            return Task.CompletedTask;
        }
    }
}
