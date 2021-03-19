// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Content
{
    using System.Linq;

    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns true if the character is a letter.
        /// </summary>
        /// <param name="character">The character to check.</param>
        /// <returns>
        ///     <see langword="true"/> if the character is a letter.
        /// </returns>
        public static bool IsLetter(this char character)
            => (character >= 65 && character <= 90) || (character >= 97 && character <= 122);

        /// <summary>
        /// Returns true if the character is a number.
        /// </summary>
        /// <param name="character">The character to check.</param>
        /// <returns>
        ///     <see langword="true"/> if the character is a single number from 0 to 9.
        /// </returns>
        public static bool IsNumber(this char character) => character >= 48 && character <= 57;

        /// <summary>
        /// Gets the first occurance of any number in the string.
        /// </summary>
        /// <param name="value">The string that could possible container a number.</param>
        /// <returns>The index location of the first occurence of a single digit number character.</returns>
        public static int GetFirstOccurentOfNumber(this string value)
        {
            var number = new string(value.Where(IsNumber).ToArray());

            if (string.IsNullOrEmpty(number))
            {
                return -1;
            }

            return int.Parse(number);
        }

        /// <summary>
        /// Returns true if the string has any single digit numbers in them.
        /// </summary>
        /// <param name="value">The string that could possibly contain single digit numbers.</param>
        /// <returns>
        ///     <see langword="true"/> if the string contains any single digit numbers.
        /// </returns>
        public static bool HasNumbers(this string value) => value.Any(IsNumber);

        /// <summary>
        /// Returns true if the string contains only letters and numbers.
        /// </summary>
        /// <param name="value">The string that could possible only contain letters and single digit numbers.</param>
        /// <returns>
        ///     <see langword="true"/> if the string only contains letters and single digit numbers.
        /// </returns>
        public static bool ContainsOnlyLettersAndNumbers(this string value)
        {
            for (var i = 0; i < value.Length; i++)
            {
                // If any of the characters are not a letter or number
                if (!value[i].IsNumber() && !value[i].IsLetter())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
