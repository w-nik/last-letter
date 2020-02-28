using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Game
{
    [JsonObject]
    public class Match
    {
        [JsonProperty]
        private readonly List<IPlayer> _players = new List<IPlayer>();
        [JsonProperty]
        private readonly WordDictionary _dictionary;
        [JsonProperty]
        private readonly ISet<string> _usedWords = new HashSet<string>();
        [JsonProperty]
        private int turn = 0;

        private Random _dice = new Random();
        private event Action<string> WordAcceptedNotification = s => { };

        [JsonProperty]
        public IList<string> GameLog { get; } = new List<string>();

        [JsonConstructor]
        private Match()
        {
        }

        public Match(User user, Bot bot, WordDictionary dictionary)
        {
            _players.Add(user);
            _players.Add(bot);
            WordAcceptedNotification += user.WordAccepted;
            WordAcceptedNotification += bot.WordAccepted;

            _dictionary = dictionary;

            OrderPlayers();
        }

        public void Play()
        {
            while (true)
            {
                for (; turn < _players.Count; turn++)
                {
                    do
                    {
                        var word = _players[turn].NextWord();

                        if (string.IsNullOrEmpty(word))
                        {
                            _players[turn].EndGame("You lose...");
                            GameLog.Add($"Player {_players[turn].GetType().Name} lose...");
                            WordAcceptedNotification -= _players[turn].WordAccepted;
                            _players.RemoveAt(turn);
                            break;
                        }

                        if (_dictionary.Verify(word) && !_usedWords.Contains(word))
                        {
                            _usedWords.Add(word);
                            GameLog.Add(word);
                            WordAcceptedNotification(word);
                            break;
                        }
                        else
                        {
                            GameLog.Add($"{word} - rejected");
                            _players[turn].WordRejected(word);
                        }

                    } while (true);
                }

                if (_players.Count == 1)
                {
                    _players[0].EndGame("You win!");
                    GameLog.Add($"Player {_players[turn].GetType().Name} win!");
                    break;
                }

                turn = 0;
            }
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            _dice = new Random();
            _players.ForEach(p => WordAcceptedNotification += p.WordAccepted);
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
