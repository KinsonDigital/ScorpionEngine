using ScorpionCore;
using MonoScorpPlugin;
using ScorpionEngine.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Config
{
    public static class DriverLoader
    {
        public static void Init()
        {
            //TODO: Load the assembly here
        }


        public static IContentLoader GetContentLoaderPlugin()
        {
            return Activator.CreateInstance<MonoContentLoader>();
        }
    }
}
