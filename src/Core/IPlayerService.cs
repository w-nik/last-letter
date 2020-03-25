using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IPlayerService
    {
        Message NextWord(string lastWord);

        void WordAccepted(string word);

        void WordRejected(string word);

        void EndGame(string message);

        bool VerifyMatch(int matchId);
    }
}
