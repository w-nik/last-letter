using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IList<string> _gameLog = new List<string>();
        [JsonProperty]
        private readonly State _state;
        [JsonProperty]
        private int turn = 0;

        private Random _dice = new Random();
        private event Action<string> WordAcceptedNotification = s => { };
        private event Action<string> WordRejectionNotification = s => { };

        [JsonConstructor]
        private Match()
        {
        }

        public Match(User user, Bot bot, State state)
        {
            _players.Add(user);
            _players.Add(bot);
            WordAcceptedNotification += user.WordAccepted;
            WordAcceptedNotification += bot.WordAccepted;
            WordAcceptedNotification += state.WordAccepted;

            WordRejectionNotification += user.WordRejected;
            WordRejectionNotification += bot.WordRejected;
            WordRejectionNotification += state.WordRejected;

            _state = state;

            OrderPlayers();
        }

        public IList<string> GameLog => new List<string>(_gameLog);

        public void Play()
        {
            while (true)
            {
                for (; turn < _players.Count; turn++)
                {
                    do
                    {
                        var message = _players[turn].NextWord(_state.LastWord);

                        if (message.Status == Status.Accept)
                        {
                            var verified = _state.Verify(message.Text);

                            if (verified)
                            {
                                _gameLog.Add($"{_players[turn].Name}:\t{message.Text}\t\t[Accepted]");
                                Console.SetCursorPosition(0, Console.CursorTop - 1);
                                Console.WriteLine($"{_players[turn].Name}:\t{message.Text}\t\t[Accepted]");

                                WordAcceptedNotification(message.Text);
                                break;
                            }
                            else
                            {
                                _gameLog.Add($"{_players[turn].Name}:\t{message.Text}\t\t[Rejected]");
                                Console.SetCursorPosition(0, Console.CursorTop - 1);
                                Console.WriteLine($"{_players[turn].Name}:\t{message.Text}\t\t[Rejected]");

                                WordRejectionNotification(message.Text);
                            }
                        }
                        else if (message.Status == Status.GiveUp)
                        {
                            _players[turn].EndGame("You lose...");
                            _gameLog.Add($"Player {_players[turn].Name} lose...");

                            WordAcceptedNotification -= _players[turn].WordAccepted;
                            _players.RemoveAt(turn);
                            break;
                        }
                        else if (message.Status == Status.Reject)
                        {
                            _gameLog.Add($"{_players[turn].Name}:\t{message.Text}\t\t[Rejected]");
                            Console.SetCursorPosition(0, Console.CursorTop - 1);
                            Console.WriteLine($"{_players[turn].Name}:\t{message.Text}\t\t[Rejected]");

                            WordRejectionNotification(message.Text);
                            turn = turn == 0 ? _players.Count - 1 : turn - 1;
                        }

                    } while (true);
                }

                if (_players.Count == 1)
                {
                    _players.First().EndGame("You win!");
                    _gameLog.Add($"Player {_players.First().Name} win!");
                    break;
                }

                turn = 0;
            }
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            _dice = new Random();
            WordAcceptedNotification += _state.WordAccepted;
            WordRejectionNotification += _state.WordRejected;
            _players.ForEach(p => WordAcceptedNotification += p.WordAccepted);
            _players.ForEach(p => WordRejectionNotification += p.WordRejected);
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
