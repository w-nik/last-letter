using Newtonsoft.Json;

namespace Game
{
    public interface IPlayer
    {
        string Name { get; }

        Message NextWord(string lastWord);

        void WordAccepted(string word);

        void WordRejected(string word);

        void EndGame(string message);
    }
}
