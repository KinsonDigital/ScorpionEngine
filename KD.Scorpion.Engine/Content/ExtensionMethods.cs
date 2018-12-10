﻿using System.Linq;

namespace KDScorpionEngine.Content
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns true if the character is a letter.
        /// </summary>
        public static bool IsLetter(this char item)
        {
            return (item >= 65 && item <= 90 || item >= 97 && item <= 122);
        }


        /// <summary>
        /// Returns true if the character is a number.
        /// </summary>
        /// <returns></returns>
        public static bool IsNumber(this char item)
        {
            return (item >= 48 && item <= 57);
        }


        /// <summary>
        /// Gets the first occurance of any number in the string.
        /// </summary>
        /// <returns></returns>
        public static int GetFirstOccurentOfNumber(this string item)
        {
            var number = new string(item.Where(IsNumber).ToArray());

            if (string.IsNullOrEmpty(number)) return -1;


            return int.Parse(number);
        }


        /// <summary>
        /// Returns true if the string has any number digits in them.
        /// </summary>
        /// <returns></returns>
        public static bool HasNumbers(this string item)
        {
            return item.Any(IsNumber);
        }


        /// <summary>
        /// Returns true if the string contains only letters and numbers.
        /// </summary>
        /// <returns></returns>
        public static bool ContainsOnlyLettersAndNumbers(this string item)
        {
            for (var i = 0; i < item.Length; i++)
            {
                //If any of the characters are not a letter or number
                if (!item[i].IsNumber() && !item[i].IsLetter())
                    return false;
            }


            return true;
        }
    }
}