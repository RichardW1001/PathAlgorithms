using System;
using System.Collections.Generic;
using System.Diagnostics;
using ACS.TechTest.Services;

namespace ACS.TechTest.ConsoleClient
{
    class Program
    {
        private static string _dictionaryFile;
        private static string _resultsFile;
        private static string _startWord;
        private static string _endWord;

        static void Main()
        {
            WriteHeader();

            GetDictionaryPath();
            GetResultsPath();
            GetStartWord();
            GetEndWord();

            FindPaths();

            Finish();
        }

        private static void Finish()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void FindPaths()
        {
            Console.WriteLine("Press enter to find shortest path...");
            Console.ReadLine();
            
            var sw = new Stopwatch();
            sw.Start();

            var dictionaryProvider = new FileDictionaryProvider(_dictionaryFile);
            var outputProvider = new FileOutputProvider(_resultsFile);

            var pathFinder = new PathFinder(dictionaryProvider, outputProvider);

            IEnumerable<string> results = new string[]{};
            try
            {
                results = pathFinder.FindPath(_startWord, _endWord);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error: {0}", e.Message);
            }

            sw.Stop();

            Console.WriteLine("That took {0} milliseconds", sw.ElapsedMilliseconds);
            Console.WriteLine();
            Console.WriteLine("Results were: ");
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        private static void GetEndWord()
        {
            _endWord = null;
            while (string.IsNullOrEmpty(_endWord))
            {
                Console.WriteLine("Choose a ending word:");
                _endWord = Console.ReadLine();
            }
            Console.WriteLine("Ending word is: {0}", _endWord);
        }

        private static void GetStartWord()
        {
            _startWord = null;
            while (string.IsNullOrEmpty(_startWord))
            {
                Console.WriteLine("Choose a starting word:");
                _startWord = Console.ReadLine();
            }
            Console.WriteLine("Starting word is: {0}", _startWord);
        }

        private static void GetResultsPath()
        {
            while (string.IsNullOrEmpty(_resultsFile))
            {
                Console.WriteLine("Please specify where to write results to:");
                _resultsFile = Console.ReadLine();
            }
            Console.WriteLine("Results will be saved to: {0}", _resultsFile);
        }

        private static void GetDictionaryPath()
        {
            while (string.IsNullOrEmpty(_dictionaryFile))
            {
                Console.WriteLine("Please specify the location of the dictionary file:");
                _dictionaryFile = Console.ReadLine();
            }
            Console.WriteLine("Dictionary file found: {0}", _dictionaryFile);
        }

        protected static void WriteHeader()
        {
            Console.WriteLine();
            Console.WriteLine("This is a little program to get from one word to another, changing one letter at a time.");
            Console.WriteLine("This console client is deliberately simple");
            Console.WriteLine();
        }

    }
}
