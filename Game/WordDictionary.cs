using System.Collections.Generic;

namespace Game
{
    public class WordDictionary
    {
        private readonly IList<string> _words = new List<string>();

        public WordDictionary()
        {
            InitDictionary();
        }

        public bool Verify(string word)
        {
            return _words.Contains(word);
        }

        private void InitDictionary()
        {
            _words.Add("design");
            _words.Add("friend");
            _words.Add("key");
            _words.Add("network");
            _words.Add("yacht");
        }
    }
}
