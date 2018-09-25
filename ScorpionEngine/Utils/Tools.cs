using ScorpionEngine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Utils
{
    /// <summary>
    /// Provides basic tools to make things easier internally in the engine.
    /// </summary>
    public static class Tools
    {
        #region Public Methods
        /// <summary>
        /// Gets the minimum value of the given enumeration.
        /// </summary>
        /// <param name="enumType">The enum to process.</param>
        /// <returns></returns>
        public static int GetEnumMin(Type enumType)
        {
            var values = Enum.GetValues(enumType);

            //Get the smallest item out of all of the values
            return values.Cast<int>().Concat(new[] { int.MaxValue }).Min();
        }
        #endregion
    }
}
