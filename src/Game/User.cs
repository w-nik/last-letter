using System;
using System.Threading.Tasks;
using Core;
using Newtonsoft.Json;

namespace GameOfWords
{
    [Obsolete]
    [JsonObject]
    public class User : IRemotePlayer
    {
        public Task<Message> NextWord(string lastWord)
        {
            Console.Write("User:\t");

            Message message = null;
            var word = string.Empty;

            do
            {
                word = Console.ReadLine();
            } while (string.IsNullOrEmpty(word));

            if ("sudo give-up".Equals(word))
                message = new Message { Status = Status.GiveUp, Text = string.Empty };
            else if ("sudo reject".Equals(word))
                message = new Message { Status = Status.Reject, Text = lastWord };
            else if ("sudo null".Equals(word))
                throw null;
            else
                message = new Message { Status = Status.Accept, Text = word };

            return Task.FromResult(message);
        }

        public void HandlePlayerResponse(Message message)
        {
            throw new NotImplementedException();
        }

        [JsonProperty]
        public string Name => this.GetType().Name;

        public Task WordAccepted(string player, string word)
        {
            if (player == this.Name)
                Console.SetCursorPosition(0, Console.CursorTop - 1);

            Console.WriteLine($"{player}:\t{word}\t\t[Accepted]");

            return Task.CompletedTask;
        }

        public Task WordRejected(string player, string word)
        {
            if (player == this.Name)
                Console.SetCursorPosition(0, Console.CursorTop - 1);

            Console.WriteLine($"{player}:\t{word}\t\t[Rejected]");

            return Task.CompletedTask;
        }

        public Task EndGame(string message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}
