using System;
using System.Collections.Generic;
using System.Linq;

namespace ACS.TechTest.Services
{
    public static class SearchHelper
    {
        public static IEnumerable<T> ShortestPath<T>(T startNode, Func<IEnumerable<T>, bool> matchCondition, Func<IEnumerable<T>, IEnumerable<T>> childNodes)
        {
            var solutionPaths = new List<List<T>>();
            var candidatePaths = new Queue<List<T>>();

            candidatePaths.Enqueue(new List<T> { startNode });

            var betterSolutionsPossible = true;
            while (betterSolutionsPossible)
            {
                var candidatePath = candidatePaths.Dequeue();

                foreach (var newCandidate in childNodes(candidatePath).Select(child => new List<T>(candidatePath) { child }))
                {
                    if (matchCondition(newCandidate)) solutionPaths.Add(newCandidate);
                    else candidatePaths.Enqueue(newCandidate);
                }

                betterSolutionsPossible = candidatePaths.Any() && 
                    (!solutionPaths.Any() || candidatePaths.Any(c => c.Count() + 1 < solutionPaths.Min(p => p.Count())));
            }

            return solutionPaths.OrderBy(s => s.Count()).FirstOrDefault();
        }
    }
}