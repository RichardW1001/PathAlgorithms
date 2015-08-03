using System.Collections.Generic;
using System.IO;

namespace ACS.TechTest.Services
{
    public class FileOutputProvider : IOutputProvider
    {
        public string FileName { get; set; }

        public FileOutputProvider(string fileName)
        {
            FileName = fileName;
        }

        public void OutputResults(IEnumerable<string> results)
        {
            File.WriteAllLines(FileName, results);
        }
    }
}