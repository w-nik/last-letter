using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace User
{
    public class PlayerService : IPlayerService
    {
        private int _matchId = -1;

        public Message NextWord(string lastWord)
        {
            Console.Write("User:\t");

            var message = GetMessageFromUser(lastWord);

            return message;
        }

        public void WordAccepted(string word)
        {
            Console.WriteLine($"{word}\t\t[Accepted]");
        }

        public void WordRejected(string word)
        {
            Console.WriteLine($"{word}\t\t[Rejected]");
        }

        public void EndGame(string message)
        {
            Console.WriteLine(message);
        }

        public bool VerifyMatch(int matchId)
        {
            if (_matchId < 0)
                _matchId = matchId;

            return _matchId == matchId;
        }

        private Message GetMessageFromUser(string lastWord)
        {
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

            return message;
        }
    }
}
