using System;
using System.Runtime.Serialization;

namespace Game
{
    [Serializable]
    public class User : IPlayer
    {
        public string NextWord()
        {
            Console.Write("Your turn: ");
            var word = Console.ReadLine();

            if("throw".Equals(word))
                throw new ArgumentException();

            return word;
        }

        public void WordAccepted(string word)
        {
            Console.WriteLine(word);
        }

        public void WordRejected(string word)
        {
            Console.WriteLine($"Your word [{word}] rejected. Try another one");
        }

        public void EndGame(string message)
        {
            Console.WriteLine(message);
        }
    }
}
