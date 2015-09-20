using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLib.Utils
{
    public class Array
    {
        /// <summary>
        /// Checks whether
        /// </summary>
        /// <param name="array">The array the method loops through</param>
        /// <param name="objectToSearchFor">The object which has to be located in the array</param>
        /// <returns>-1 if wasnt found else the index in the array where the object was found</returns>
        public static int IsInArray(object[] array, object objectToSearchFor)
        {
            for (int i = 0; i < array.Length; i++) if (array[i] == objectToSearchFor) return i;
            return -1;
        }
    }
}
