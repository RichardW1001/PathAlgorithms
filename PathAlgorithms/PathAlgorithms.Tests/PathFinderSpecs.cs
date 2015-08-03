using System.Collections.Generic;
using System.Linq;
using ACS.TechTest.Services;
using ACS.TechTest.Tests.Framework;
using NUnit.Framework;

namespace ACS.TechTest.Tests
{
    namespace PathFinderSpecs
    {
        //Reusable stuff
        public class PathFinderSpecBase : SpecBase<PathFinder>
        {
            public class FakeDictionaryProvider : IDictionaryProvider
            {
                private readonly IEnumerable<string> _wordList;

                public FakeDictionaryProvider(IEnumerable<string> wordList)
                {
                    _wordList = wordList;
                }

                public IEnumerable<string> GetDictionary(string endWord)
                {
                    return _wordList;
                }
            }

            public class FakeOutputProvider : IOutputProvider
            {
                public bool ResultsWereOutput { get; set; }

                public void OutputResults(IEnumerable<string> results)
                {
                    ResultsWereOutput = true;
                }
            }

            protected FakeOutputProvider OutputProvider = new FakeOutputProvider();
            protected FakeDictionaryProvider DictionaryProvider;
            protected string StartWord;
            protected string EndWord;

            protected IEnumerable<string> Results;

            protected void GivenWordsAre(IEnumerable<string> words)
            {
                DictionaryProvider = new FakeDictionaryProvider(words);
            }

            protected void GivenStartWordIs(string startWord)
            {
                StartWord = startWord;
            }

            protected void GivenEndWordIs(string endWord)
            {
                EndWord = endWord;
            }

            protected void WhenFindingPath()
            {
                Target = new PathFinder(DictionaryProvider, OutputProvider);
                Results = Target.FindPath(StartWord, EndWord);
            }

            protected void ThenResultsShouldBe(IEnumerable<string> expectedResults)
            {
                if (expectedResults == null)
                {
                    Assert.IsNull(Results);
                }
                else
                {
                    var expectedResultsArray = expectedResults.ToArray();
                    foreach (var expectedResult in expectedResultsArray)
                    {
                        Assert.IsTrue(Results.Contains(expectedResult), string.Format("Results did not contain {0}", expectedResult));
                    }
                    Assert.AreEqual(expectedResultsArray.Count(), Results.Count());
                }
            }

            protected void ThenResultsShouldBeOutput()
            {
                Assert.IsTrue(OutputProvider.ResultsWereOutput);
            }
        }

        /// <summary>
        /// The first test case given in the brief; Spin -> Spot
        /// </summary>
        public class TestCase1 : PathFinderSpecBase
        {
            protected override void Given()
            {
                GivenStartWordIs("Spin");
                GivenEndWordIs("Spot");
                GivenWordsAre(new[]
                {
                    "Spin",
                    "Spit",
                    "Spat",
                    "Spot",
                    "Span"
                });
            }

            protected override void When()
            {
                WhenFindingPath();
            }

            [Then]
            public void ResultsShouldBeCorrect()
            {
                ThenResultsShouldBe(new List<string>
                {
                    "Spin",
                    "Spit",
                    "Spot"
                });
            }

            [Then]
            public void ResultsShouldBeOutput()
            {
                ThenResultsShouldBeOutput();
            }
        }

        /// <summary>
        /// The second test case given in the brief; AAAA -> AAZZ 
        /// </summary>
        public class TestCase2 : PathFinderSpecBase
        {
            protected override void Given()
            {
                GivenStartWordIs("AAAA");
                GivenEndWordIs("AAZZ");
                GivenWordsAre(new[]
                {
                    "ABAA", "AAAA", "ABZA", "ABZZ", "AAZZ"
                });
            }

            protected override void When()
            {
                WhenFindingPath();
            }

            [Then]
            public void ResultsShouldBeCorrect()
            {
                ThenResultsShouldBe(new List<string>
                {
                    "AAAA",
                    "ABAA",
                    "ABZA",
                    "ABZZ",
                    "AAZZ"
                });
            }
        }

        /// <summary>
        /// Ensuring that if there is no route null is returned
        /// </summary>
        public class TestCase3 : PathFinderSpecBase
        {
            protected override void Given()
            {
                GivenStartWordIs("AAAA");
                GivenEndWordIs("ZZZZ");
                GivenWordsAre(new[]
                {
                    "ABAA", "AAAA", "ABZA", "ABZZ", "AAZZ"
                });
            }

            protected override void When()
            {
                WhenFindingPath();
            }

            [Then]
            public void ResultsShouldBeCorrect()
            {
                ThenResultsShouldBe(null);
            }
        }

        /// <summary>
        /// Testing against a bigger dictionary (integration test; unignore to run; make sure dictionary path exists!)
        /// </summary>
        public class TestCase4 : PathFinderSpecBase
        {
            protected override void Given()
            {
                GivenStartWordIs("cat");
                GivenEndWordIs("dog");
            }

            protected override void When()
            {
                Target = new PathFinder(new FileDictionaryProvider("c://words-english.txt"), new FakeOutputProvider());
                Results = Target.FindPath(StartWord, EndWord);
            }

            [Then]
            //[Ignore]
            public void ResultsShouldBeCorrect()
            {
                ThenResultsShouldBe(new List<string>
                {
                    "cat",
                    "cot",
                    "dot",
                    "dog"
                });
            }
        }

        /// <summary>
        /// Making sure start and end word are the same length
        /// </summary>
        public class TestCase5 : PathFinderSpecBase
        {
            protected override void Given()
            {
                GivenStartWordIs("cat");
                GivenEndWordIs("tiger");
                GivenWordsAre(new []{"meow"});
            }

            protected override void When()
            {
                WhenFindingPath();
            }

            [Then]
            public void ThereShouldBeAnError()
            {
                Assert.IsNotNull(Exception);
            }
        }
    }
}