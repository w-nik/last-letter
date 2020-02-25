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
            Match match = null;

            if (File.Exists("/savedGames.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open("/savedGames.gd", FileMode.Open);
                match = (Match)bf.Deserialize(file);
                file.Close();
            }

            try
            {
                if (match == null)
                {
                    match = new Match(
                        new User(),
                        new Bot(new FileDictionaryProvider(), "shortDictionary.json"),
                        new WordDictionary(new FileDictionaryProvider(), "extendedDictionary.json"));
                }
                else
                {
                    foreach (var text in match.GameLog)
                    {
                        Console.WriteLine(text);
                    }
                }

                match.Play();
            }
            catch (Exception e)
            {
                Console.WriteLine("Match failed due to unexpected error.", e);
            }
            finally
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create("/savedGames.gd");
                bf.Serialize(file, match);
                file.Close();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
