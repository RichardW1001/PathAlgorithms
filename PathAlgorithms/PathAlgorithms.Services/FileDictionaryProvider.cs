using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACS.TechTest.Services
{
    public class FileDictionaryProvider : IDictionaryProvider
    {
        public FileDictionaryProvider(string filePath)
        {
            FilePath = filePath;
        }


        protected IEnumerable<string> Words { get; set; }
        protected string FilePath { get; set; }

        protected void Initialise()
        {
            Words = Words ?? File.ReadAllLines(FilePath);
        }
        
        public IEnumerable<string> GetDictionary(string endWord)
        {
            Initialise();
            return Words.Where(w => w.Length == endWord.Length).ToArray();
        }
    }
}