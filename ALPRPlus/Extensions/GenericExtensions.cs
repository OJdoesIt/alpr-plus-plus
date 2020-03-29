using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Plugins.ALPRPlus.Extensions
{
    internal static class GenericExtensions
    {
        internal static bool IsBetween<T>(this T item, T start, T end, bool inclusive = true)
        {
            if (inclusive)
            {
                return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
            }
            else
            {
                return Comparer<T>.Default.Compare(item, start) > 0
                && Comparer<T>.Default.Compare(item, end) < 0;
            }
        }
    }
}
