using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delbert.Infrastructure
{
    public static class StringExtensions
    {
        public static bool EndsWithAny(this string str, IEnumerable<string> phrases)
        {
            return phrases.Any(str.EndsWith);
        }
    }
}
