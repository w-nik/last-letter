using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameSaves = Directory.GetCurrentDirectory() + "\\" + "lastLetterSaves.json";

            Match match = null;

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            if (File.Exists(gameSaves))
            {
                var text = File.ReadAllText(gameSaves);
                match = JsonConvert.DeserializeObject<Match>(text, settings);
            }

            try
            {
                if (match != null)
                {
                    foreach (var text in match.GameLog)
                        Console.WriteLine(text);
                }

                match = match ?? new Match(
                            new User(),
                            new Bot(new FileDictionaryProvider(), "shortDictionary.json"),
                            new State(new WordDictionary(new FileDictionaryProvider(), "extendedDictionary.json")));

                match.Play();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Match failed due to unexpected error. {e.Message}");
            }
            finally
            {
                var text = JsonConvert.SerializeObject(match, settings);
                File.WriteAllText(gameSaves, text);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
