using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game
{
    [JsonObject]
    public class PlayersEnumerator : IBidirectionalEnumerator<IPlayer>
    {
        [JsonProperty]
        private readonly IList<IPlayer> _players;
        [JsonProperty]
        private int _turn = -1;

        [JsonConstructor]
        private PlayersEnumerator() { }

        public PlayersEnumerator(IList<IPlayer> players)
        {
            _players = players;
        }

        public bool Freeze()
        {
            _turn = _turn == 0 ? _players.Count - 1 : _turn - 1;

            return true;
        }

        public bool MoveBack()
        {
            _turn -= 2;
            if (_turn < 0) _turn = _players.Count + _turn;

            return true;
        }

        public bool MoveNext()
        {
            if (++_turn >= _players.Count) _turn = 0;
            Current = _players[_turn];

            return _players.Count > 1;
        }

        public void Reset()
        {
            _turn = -1;
        }

        object IEnumerator.Current => Current;

        public IPlayer Current { get; private set; }

        public void Dispose()
        {
        }
    }

}
