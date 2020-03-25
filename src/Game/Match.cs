using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Core;
using Newtonsoft.Json;
using Polly;

namespace GameOfWords
{
    [JsonObject]
    public class Match
    {
        [JsonProperty]
        private readonly List<IRemotePlayer> _players = new List<IRemotePlayer>();
        [JsonProperty]
        private readonly IList<string> _gameLog = new List<string>();
        [JsonProperty]
        private readonly State _state;
        [JsonProperty]
        private readonly PlayersIterator _iterator;

        private Random _dice = new Random();
        private event Func<string, string, Task> WordAcceptedNotification = (p, w) => Task.CompletedTask;
        private event Func<string, string, Task> WordRejectionNotification = (p, w) => Task.CompletedTask;

        [JsonConstructor]
        private Match() { }

        public Match(int id, IRemotePlayer user, IRemotePlayer bot, State state)
        {
            Id = id;
            _iterator = new PlayersIterator(_players);
            _state = state;
            
            _players.Add(bot);
            _players.Add(user);

            OrderPlayers(_players);
            Subscribe();
        }

        public int Id { get; }

        public IList<string> GameLog => new List<string>(_gameLog);

        public async Task Play()
        {
            while (_iterator.MoveNext())
            {
                var player = _iterator.Current;

                var message = player.NextWord(_state.LastWord).Result;

                if (message.Status == Status.Accept)
                {
                    var verified = _state.Verify(message.Text);

                    if (verified)
                    {
                        _gameLog.Add($"{player}:\t{message}\t\t[Accept]");
                        await WordAcceptedNotification(player.Name, message.Text);
                    }
                    else
                    {
                        _gameLog.Add($"{player}:\t{message}\t\t[Reject]");
                        await WordRejectionNotification(player.Name, message.Text);

                        _iterator.Freeze();
                    }
                }
                else if (message.Status == Status.GiveUp)
                {
                    await player.EndGame("You lose...");
                    _gameLog.Add($"Player {player.Name} lose...");

                    WordAcceptedNotification -= player.WordAccepted;
                    _iterator.Remove(player);
                }
                else if (message.Status == Status.Reject)
                {
                    _gameLog.Add($"{player}:\t{message}\t\t[Reject]");
                    await WordRejectionNotification(player.Name, message.Text);

                    _iterator.MoveBack();
                }
            }

            await _iterator.Current.EndGame("You win!");
        }

        public void ResponseFromPlayer(Message message)
        {
            _iterator.Current.HandlePlayerResponse(message);
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            _dice = new Random();
            Subscribe();
        }


        private void Subscribe()
        {
            WordAcceptedNotification += _state.WordAccepted;
            WordRejectionNotification += _state.WordRejected;
            _players.ForEach(p => WordAcceptedNotification += p.WordAccepted);
            _players.ForEach(p => WordRejectionNotification += p.WordRejected);
        }

        private void OrderPlayers(List<IRemotePlayer> players)
        {
            players.Sort(
                (player1, player2) =>
                {
                    if (_dice.Next() % 2 == 0) return -1;
                    else return 1;
                });
        }
    }
}
