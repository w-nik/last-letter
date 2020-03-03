using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Game
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

        private bool _accepted;

        [JsonConstructor]
        private State()
        {
        }

        public State(WordDictionary dictionary)
        {
            this._dictionary = dictionary;
        }

        public string LastWord => _acceptedWords.LastOrDefault();

        public bool Verify(string word)
        {
            var verified = _dictionary.Verify(word);
            var notUsed = !_acceptedWords.Contains(word) && !_rejectedWords.Contains(word);

            //if (!verified)
            //    Console.WriteLine("[Rejected] No such word");

            //if (!notUsed)
            //    Console.WriteLine("[Rejected] Word already used");

            _accepted = verified && notUsed;

            return _accepted;
        }

        public void WordAccepted(string word)
        {
            _acceptedWords.Add(word);

            //Console.WriteLine("[Accepted] By dictionary");
        }

        public void WordRejected(string word)
        {
            _rejectedWords.Add(word);

            if (_acceptedWords.Last() == word)
                _acceptedWords.Remove(word);

            //if (_accepted)
            //    Console.WriteLine("[Rejected] By user");
        }
    }
}
