using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScorpionEngine.Tests
{
    public static class AssertExt
    {
        public static void NotThrown<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T)
            {
                Assert.True(false, $"Expected the exception {typeof(T).Name} to not be thrown.");
            }
        }
    }
}
