﻿using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace GameOfWords
{
    public class FileDictionaryProvider: IDictionaryProvider
    {
        public IDictionary<string, IList<string>> GetDictionary(string location)
        {
            string text = string.Empty;

            try
            {
                text = System.IO.File.ReadAllText(location);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to find dictionary.", e);
                throw;
            }

            return JsonConvert.DeserializeObject<IDictionary<string, IList<string>>>(text);
        }
    }
}
