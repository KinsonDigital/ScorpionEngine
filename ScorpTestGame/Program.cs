using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using ScorpionEngine;

namespace ScorpTestGame
{
//#if WINDOWS || LINUX

    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //Create a new game
            using (var game = new TestGame())
            {
                game.Start(); //Start the game
            }
        }
    }

//#endif
}