using System;
using Newtonsoft.Json;

namespace Game
{
    [JsonObject]
    public class User : IPlayer
    {
        public Message NextWord(string lastWord)
        {
            Console.Write("User:\t");

            var word = string.Empty;

            do
            {
                word = Console.ReadLine();
            } while (string.IsNullOrEmpty(word));

            if ("sudo give-up".Equals(word))
                return new Message { Status = Status.GiveUp, Text = string.Empty };
            else if ("sudo reject".Equals(word))
                return new Message { Status = Status.Reject, Text = lastWord };
            else if ("sudo null".Equals(word))
                throw null;

            return new Message { Status = Status.Accept, Text = word };
        }

        [JsonProperty]
        public string Name => this.GetType().Name;

        public void WordAccepted(string word)
        {
        }

        public void WordRejected(string word)
        {
        }

        public void EndGame(string message)
        {
            Console.WriteLine(message);
        }
    }
}
