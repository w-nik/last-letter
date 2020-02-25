using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public interface IDictionaryProvider
    {
        IDictionary<string, IList<string>> GetDictionary(string location);
    }
}
