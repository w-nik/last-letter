namespace Game
{
    public interface IPlayer
    {
        string NextWord();

        void WordAccepted(string word);

        void WordRejected(string word);

        void EndGame(string message);
    }
}
