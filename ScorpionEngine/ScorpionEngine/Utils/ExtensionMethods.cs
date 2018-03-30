// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery

namespace ScorpionEngine.Utils
{
    public static class ExtensionMethods
    {
        #region Array Searching
        /// <summary>
        /// Searches the array for the given item and returns true if it is found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array to search</param>
        /// <param name="item">The item to search for.</param>
        /// <returns>True if the item has been found in the array.</returns>
        public static bool Has<T>(this T[] array, T item)
        {
            //Search the item, if found, retrun true
            for (var i = 0; i < array.Length; i++)
            {
                //If the current array item matches the item
                if (array[i].Equals(item))
                    return true;
            }

            return false;
        }
        #endregion
    }
}
