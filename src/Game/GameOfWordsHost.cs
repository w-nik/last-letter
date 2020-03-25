using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace GameOfWords
{
    public class GameOfWordsHost
    {
        private readonly MatchService _service;
     
        public GameOfWordsHost(MatchService service)
        {
            _service = service;
        }

        public void StartAsync()//(CancellationToken cancellationToken)
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

                match ??= new Match(
                    19,
                    new RemotePlayer("User", "http://localhost:53822/player/"),
                    new RemotePlayer("Bot", "http://localhost:53811/bot/"),
                    new State(new WordDictionary(new FileDictionaryProvider(), "extendedDictionary.json")));

                _service.Matches.Add(match.Id, match);

                //Thread.Sleep(TimeSpan.FromMinutes(7));

                match.Play().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Match failed due to unexpected error. {e.Message}");
            }
            //finally
            //{
            //    var text = JsonConvert.SerializeObject(match, settings);
            //    File.WriteAllText(gameSaves, text);
            //}

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            //return Task.CompletedTask;
        }

        public void StopAsync()//(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
