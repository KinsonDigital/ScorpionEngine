using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticalMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var main = new Main())
            {
                main.Start();
            }
        }
    }
}
