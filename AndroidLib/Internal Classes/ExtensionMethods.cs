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
}
