using System.Collections.Generic;

namespace ACS.TechTest.Services
{
    public interface IOutputProvider
    {
        void OutputResults(IEnumerable<string> results);
    }
}