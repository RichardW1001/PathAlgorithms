using System.Collections.Generic;

namespace ACS.TechTest.Services
{
    public interface IDictionaryProvider
    {
        IEnumerable<string> GetDictionary(string endWord);
    }
}
