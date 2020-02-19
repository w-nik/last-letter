using System;
using System.Collections.Generic;

namespace Game
{
    public class Match
    {
        private readonly List<IPlayer> _players = new List<IPlayer>();
        private readonly Random _dice;
        private readonly WordDictionary _dictionary;
        private readonly List<string> _words = new List<string>();
        
        private event Action<string> WordAcceptedNotification = s => {};

        public Match(User user, WordDictionary dictionary)
        {
            var bot = new Bot();
            _players.Add(user);
            _players.Add(bot);
            WordAcceptedNotification += user.WordAccepted;
            WordAcceptedNotification += bot.WordAccepted;

            _dice = new Random();
            _dictionary = dictionary;
        }

        public void PlayGame()
        {
            OrderPlayers();

            while (true)
            {
                for (int i = 0; i < _players.Count; i++)
                {
                    do
                    {
                        var word = _players[i].NextWord();

                        if (string.IsNullOrEmpty(word))
                        {
                            _players[i].EndGame("You lose...");
                            WordAcceptedNotification -= _players[i].WordAccepted;
                            _players.RemoveAt(i);
                            break;
                        }

                        if (_dictionary.Verify(word) && !_words.Contains(word))
                        {
                            _words.Add(word);
                            WordAcceptedNotification(word);
                            break;
                        }
                        else
                        {
                            _players[i].WordRejected(word);
                        }

                    } while (true);
                }

                if (_players.Count == 1)
                {
                    _players[0].EndGame("You win!");
                    break;
                }
            }
        }

        private void OrderPlayers()
        {
            _players.Sort(
                (player1, player2) =>
                {
                    if (_dice.Next() % 2 == 0) return -1;
                    else return 1;
                });
        }
    }
}
