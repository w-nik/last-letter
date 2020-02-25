using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Game
{
    [Serializable]
    public class Bot : IPlayer
    {
        private IDictionary<string, IList<string>> _words;
        private string _lastWord = "";

        public Bot(IDictionaryProvider dictProvider, string dictLocation)
        {
            _words = dictProvider.GetDictionary(dictLocation);
        }

        public string NextWord()
        {
            string word = "";

            if (string.IsNullOrEmpty(_lastWord))
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
                var lastLetter = _lastWord.Last().ToString().ToUpper();

                if (_words.ContainsKey(lastLetter))
                {
                    word = _words[lastLetter].FirstOrDefault();
                }
            }

            return word;
        }

        public void WordAccepted(string word)
        {
            RemoveWordFromDictionary(word);

            _lastWord = word;
        }

        public void WordRejected(string word)
        {
            Console.WriteLine($"Bot's word [{word}] rejected.");
            RemoveWordFromDictionary(word);
        }

        public void EndGame(string message)
        {
        }

        private void RemoveWordFromDictionary(string word)
        {
            var firstLetter = word.First().ToString().ToUpper();

            if (_words.ContainsKey(firstLetter))
            {
                _words[firstLetter].Remove(word);
                if (_words[firstLetter].Count == 0) _words.Remove(firstLetter);
            }
        }
    }
}
