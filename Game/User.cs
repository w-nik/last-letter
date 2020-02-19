using System;

namespace Game
{
    public class User : IPlayer
    {
        public string NextWord()
        {
            Console.Write("Your turn: ");
            var word = Console.ReadLine();
            return word;
        }

        public void WordAccepted(string word)
        {
            Console.WriteLine(word);
        }

        public void WordRejected(string word)
        {
            Console.WriteLine("Word rejected. Try another one");
        }

        public void EndGame(string message)
        {
            Console.WriteLine(message);
        }
    }
}
