using System;
using System.Collections.Generic;
using System.Linq;

namespace ACS.TechTest.Services
{
    public class PathFinder
    {
        private readonly IDictionaryProvider _dictionaryProvider;
        private readonly IOutputProvider _outputProvider;

        public PathFinder(IDictionaryProvider dictionaryProvider, IOutputProvider outputProvider)
        {
            _dictionaryProvider = dictionaryProvider;
            _outputProvider = outputProvider;
        }

        public IEnumerable<string> FindPath(string startWord, string endWord)
        {
            ValidateInputs(startWord, endWord);

            var rankedDictionary = BuildRankedDictionary(startWord, endWord);

            var solution = SearchHelper.ShortestPath(startWord, p => p.Last() == endWord,
                p => GetViableLinks(rankedDictionary, p)).ToArray();

            _outputProvider.OutputResults(solution);

            return solution;
        }

        private static IEnumerable<string> GetViableLinks(IEnumerable<WordData> rankedDictionary, IEnumerable<string> currentSequence)
        {
            var sequenceArray = currentSequence.ToArray();
            return rankedDictionary.
                Select(w => w.Word).
                Except(sequenceArray).
                Where(w => IsLinked(w, sequenceArray.Last()));
        }

        private static void ValidateInputs(string startWord, string endWord)
        {
            if (endWord.Length != startWord.Length)
                throw new InvalidOperationException("Start and end words must be the same length");
        }

        private static int NumberOfLettersDifferent(string a, string b)
        {
            if (a.Length != b.Length) throw new InvalidOperationException("Lengths must match");
            return Enumerable.Range(0, a.Length).Count(i => a[i] != b[i]);
        }

        private static bool IsLinked(string a, string b)
        {
            if (a.Length != b.Length) return false;
            var counter = 0;
            for (var i = 0; i < a.Length; i++)
            {
                if (b[i] != a[i]) counter++;
                if (counter > 1) return false;
            }
            return true;
        }

        private IEnumerable<WordData> BuildRankedDictionary(string startWord, string endWord)
        {
            return _dictionaryProvider.GetDictionary(endWord).
                Select(w => new WordData
                {
                    Word = w,
                    DistanceFromEnd = NumberOfLettersDifferent(w, endWord),
                    DistanceFromStart = NumberOfLettersDifferent(w, startWord)
                }).
                OrderBy(w => w.DistanceFromEnd).
                ThenByDescending(w => w.DistanceFromStart).
                ThenByDescending(w => w.Word).
                ToList();
        }

        protected struct WordData
        {
            public string Word { get; set; }
            public int DistanceFromStart { get; set; }
            public int DistanceFromEnd { get; set; }
        }
    }
}