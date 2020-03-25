using System.Collections.Generic;

namespace Core
{
    public interface IDictionaryProvider
    {
        IDictionary<string, IList<string>> GetDictionary(string location);
    }
}
