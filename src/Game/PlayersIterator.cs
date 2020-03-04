using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game
{
    [JsonObject]
    public class PlayersIterator
    {
        [JsonProperty]
        private readonly IList<IPlayer> _players;
        [JsonProperty]
        private int _turn = -1;

        [JsonConstructor]
        private PlayersIterator() { }

        public PlayersIterator(IList<IPlayer> players)
        {
            _players = players;
        }

        public bool MoveNext()
        {
            if (++_turn >= _players.Count) _turn = 0;
            Current = _players[_turn];

            return _players.Count > 1;
        }

        public bool MoveBack()
        {
            _turn -= 2;
            if (_turn < 0) _turn = _players.Count + _turn;

            return _players.Count > 1;
        }

        public void Freeze()
        {
            _turn = _turn == 0 ? _players.Count - 1 : _turn - 1;
        }

        public IPlayer Current { get; private set; }

        public bool Remove(IPlayer player)
        {
            return _players.Remove(player);
        }
    }

}
