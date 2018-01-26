using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFML.Shared.Utility
{
    public class StringUtil
    {
        /// <summary>Checks if the string is null.</summary>
        /// <param name="input">The input string.</param>
        /// <returns>A boolean value: [true] if the string is null, else [false].</returns>
        public static bool IsNull(string input)
        {
            return input == null ? true : false;
        }

        /// <param name="input">The input string.</param>
        /// <returns>If input string is null, returns null. Else, the string in upper case.</returns>
        public static string ToUpper(string input)
        {
            return IsNull(input) ? null : input.ToUpper();
        }
    }
}