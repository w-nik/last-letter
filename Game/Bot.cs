using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class Bot : IPlayer
    {
        private readonly IList<string> _words = new List<string>();

        private string _lastWord = "";

        public Bot()
        {
            InitDictionary();
        }

        public string NextWord()
        {
            string word = "";

            if (string.IsNullOrEmpty(_lastWord))
            {
                var rand = new Random().Next(0, _words.Count);
                word = _words[rand];
            }
            else
            {
                word = _words.FirstOrDefault(w => w.StartsWith(_lastWord.Last()));
            }

            return word;
        }

        public void WordAccepted(string word)
        {
            _words.Remove(word);
            _lastWord = word;
        }

        public void WordRejected(string word)
        {
            _words.Remove(word);
        }

        public void EndGame(string message)
        {
        }

        private void InitDictionary()
        {
            _words.Add("friend");
            _words.Add("key");
            _words.Add("network");
        }
    }
}
