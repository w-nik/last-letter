using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Newtonsoft.Json;

namespace GameOfWords
{
    [JsonObject]
    public class WordDictionary
    {
        [JsonProperty]
        private readonly IDictionary<string, IList<string>> _dictionary;

        [JsonConstructor]
        private WordDictionary()
        {
        }

        public WordDictionary(IDictionaryProvider dictProvider, string dictionaryLocation)
        {
            _dictionary = dictProvider.GetDictionary(dictionaryLocation);
        }

        public bool Verify(string word)
        {
            var firstLetter = word.First().ToString().ToUpper();
            if (!_dictionary.ContainsKey(firstLetter))
                return false;

            return _dictionary[firstLetter].Contains(word, StringComparer.OrdinalIgnoreCase);
        }
    }
}
