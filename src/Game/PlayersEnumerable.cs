using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game
{
    [JsonObject]
    public class PlayersEnumerable : IBidirectionalEnumerable<IPlayer>
    {
        [JsonProperty]
        private readonly IList<IPlayer> _players;
        [JsonProperty]
        private readonly IBidirectionalEnumerator<IPlayer> _enumerator;

        [JsonConstructor]
        private PlayersEnumerable() { }

        public IList<IPlayer> Items => _players;

        public PlayersEnumerable(IList<IPlayer> players)
        {
            _players = new List<IPlayer>(players);
            _enumerator = new PlayersEnumerator(_players);
        }

        public bool Remove(IPlayer player)
        {
            return _players.Remove(player);
        }

        IBidirectionalEnumerator<IPlayer> IBidirectionalEnumerable<IPlayer>.GetEnumerator() => _enumerator;

        IEnumerator<IPlayer> IEnumerable<IPlayer>.GetEnumerator() => _enumerator;

        IEnumerator IEnumerable.GetEnumerator() => _enumerator;
    }

}
