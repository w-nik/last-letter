using System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            var match = new Match(new User(), new WordDictionary());
            match.PlayGame();

            Console.ReadKey();
        }
    }
}
