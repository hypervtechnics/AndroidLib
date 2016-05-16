using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib
{
    //Taken from https://github.com/regaw-leinad/AndroidLib
    internal static class ExtensionMethods
    {
        internal static bool ContainsIgnoreCase(this string s, string str)
        {
            return s.ToLower().Contains(str.ToLower());
        }

        internal static string ProperCase(this string s)
        {
            string final = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (i == 0 && char.IsLetter(s[i]))
                {
                    final += char.ToUpper(s[i]).ToString();
                }
                else if (char.IsWhiteSpace(s[i - 1]) || (char.IsControl(s[i - 1]) && char.IsLetter(s[i])))
                {
                    final += char.ToUpper(s[i]).ToString();
                }
                else
                {
                    final += char.ToLower(s[i]).ToString();
                }
            }
            return final;
        }
    }

    //Taken from http://www.dotnetperls.com/between-before-after
    static class SubstringExtensions
    {
        /// <summary>
        /// Get string value between [first] a and [next] b.
        /// </summary>
        public static string Between(this string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.IndexOf(b, posA);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        /// <summary>
        /// Get string value after [first] a.
        /// </summary>
        public static string Before(this string value, string a)
        {
            int posA = value.IndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            return value.Substring(0, posA);
        }

        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string After(this string value, string a)
        {
            int posA = value.LastIndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }
    }
}
