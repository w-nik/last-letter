using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Game
{
    [JsonObject]
    public class Bot : IPlayer
    {
        [JsonProperty]
        private IDictionary<string, IList<string>> _words;

        [JsonConstructor]
        private Bot() { }

        public Bot(IDictionaryProvider dictProvider, string dictLocation)
        {
            _words = dictProvider.GetDictionary(dictLocation);
        }

        [JsonProperty]
        public string Name => this.GetType().Name;
        
        public Message NextWord(string lastWord)
        {
            string word = "";

            if (string.IsNullOrEmpty(lastWord))
            {
                if (_words.Count > 0)
                {
                    var randKeyIndex = new Random().Next(0, _words.Keys.Count);
                    if (randKeyIndex > 0) randKeyIndex--;
                    var key = _words.Keys.ToList()[randKeyIndex];
                    var randWord = new Random().Next(0, _words[key].Count);
                    if (randWord > 0) randWord--;
                    word = _words[key][randWord];
                }
            }
            else
            {
                var lastLetter = lastWord.Last().ToString().ToUpper();

                if (_words.ContainsKey(lastLetter))
                {
                    word = _words[lastLetter].FirstOrDefault();
                }
            }

            Console.WriteLine($"Bot:\t{word}");

            if(string.IsNullOrEmpty(word))
                return new Message { Status = Status.GiveUp, Text = string.Empty };

            return new Message { Status = Status.Accept, Text = word };
        }

        public void WordAccepted(string word)
        {
            RemoveWordFromDictionary(word);
        }

        public void WordRejected(string word)
        {
            RemoveWordFromDictionary(word);
        }

        public void EndGame(string message)
        {
        }

        private void RemoveWordFromDictionary(string word)
        {
            if (!string.IsNullOrEmpty(word))
            {
                var firstLetter = word.First().ToString().ToUpper();

                if (_words.ContainsKey(firstLetter) && _words[firstLetter].Contains(word))
                {
                    _words[firstLetter].Remove(word);
                    if (_words[firstLetter].Count == 0) _words.Remove(firstLetter);
                }
            }
        }
    }
}
