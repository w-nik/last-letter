using System.Threading.Tasks;
using Core;

namespace GameOfWords
{
    public interface IRemotePlayer
    {
        string Name { get; }

        Task<Message> NextWord(string lastWord);

        void HandlePlayerResponse(Message message);
        
        Task WordAccepted(string player, string word);

        Task WordRejected(string player, string word);

        Task EndGame(string message);
    }
}
