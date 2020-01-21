using System.Collections.Generic;

namespace WordFinder
{
    public interface IWordFinder
    {
       IEnumerable<string> Find(IEnumerable<string> wordstream);
    }
}