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
        private readonly IList<string> _gameLog = new List<string>();
        [JsonProperty]
        private readonly State _state;
        [JsonProperty]
        private readonly IBidirectionalEnumerable<IPlayer> _playersEnum;

        private Random _dice = new Random();
        private event Action<string> WordAcceptedNotification = s => { };
        private event Action<string> WordRejectionNotification = s => { };

        [JsonConstructor]
        private Match() { }

        public Match(User user, Bot bot, State state)
        {
            _players.Add(bot);
            _players.Add(user);

            OrderPlayers(_players);

            _playersEnum = new PlayersEnumerable(_players);

            WordAcceptedNotification += user.WordAccepted;
            WordAcceptedNotification += bot.WordAccepted;
            WordAcceptedNotification += state.WordAccepted;

            WordRejectionNotification += user.WordRejected;
            WordRejectionNotification += bot.WordRejected;
            WordRejectionNotification += state.WordRejected;

            _state = state;

        }

        public IList<string> GameLog => new List<string>(_gameLog);

        public void Play()
        {
            foreach (IPlayer player in _playersEnum)
            {
                var message = player.NextWord(_state.LastWord);

                if (message.Status == Status.Accept)
                {
                    var verified = _state.Verify(message.Text);

                    if (verified)
                    {
                        LogLine(player.Name, message.Text, true);
                        WordAcceptedNotification(message.Text);
                    }
                    else
                    {
                        LogLine(player.Name, message.Text, false);
                        WordRejectionNotification(message.Text);

                        _playersEnum.GetEnumerator().Freeze();
                    }
                }
                else if (message.Status == Status.GiveUp)
                {
                    player.EndGame("You lose...");
                    _gameLog.Add($"Player {player.Name} lose...");

                    WordAcceptedNotification -= player.WordAccepted;
                    _playersEnum.Remove(player);
                }
                else if (message.Status == Status.Reject)
                {
                    LogLine(player.Name, message.Text, false);
                    WordRejectionNotification(message.Text);

                    _playersEnum.GetEnumerator().MoveBack();
                }
            }

            _playersEnum.GetEnumerator().Current.EndGame("You win!");
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

        private void OrderPlayers(List<IPlayer> players)
        {
            players.Sort(
                (player1, player2) =>
                {
                    if (_dice.Next() % 2 == 0) return -1;
                    else return 1;
                });
        }

        private void LogLine(string player, string message, bool isAccepted)
        {
            var resolution = isAccepted ? "[Accepted]" : "[Rejected]";

            _gameLog.Add($"{player}:\t{message}\t\t{resolution}");
            
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine($"{player}:\t{message}\t\t{resolution}");
        }
    }
}
