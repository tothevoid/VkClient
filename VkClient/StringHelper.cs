using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VkClient
{
    public static class StringExtension
    {
        /// <summary>  
        /// Unions input strings into one.
        /// </summary>  
        /// <param name="Words list"></param>

        public static string Concat(this List<string> words)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < words.Count; i++)
                sb.Append(words[i]);
            return sb.ToString();
        }

        /// <summary>  
        /// Unions input strings into one.
        /// </summary>  
        /// <param name="Words array"></param>

        public static string Concat(this string[] words)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
                sb.Append(words[i]);
            return sb.ToString();
        }

        public static string Concat<T>(this T words, char separator)
            where T : List<string>
        {
            var sb = new StringBuilder();
            foreach (var x in words)
                sb.Append(x).Append(separator);
            return sb.ToString().Substring(0,sb.Length-1);
        }
        


    }

    

}
