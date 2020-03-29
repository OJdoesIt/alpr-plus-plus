using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Plugins.ALPRPlus.Extensions
{
    internal static class EnumExtensions
    {
        internal static string ToFriendlyString(this Enum e)
        {
            return e.ToString().Replace("_", " ");
        }
    }
}
