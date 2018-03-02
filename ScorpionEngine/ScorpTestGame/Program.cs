using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using ScorpionEngine;

namespace ScorpTestGame
{
#if WINDOWS || LINUX

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
                //Set the source of the game content
                Engine.ContentRootDir = Environment.CurrentDirectory + "\\Content";

                if (Directory.Exists(Engine.ContentRootDir))
                {
                    game.Start(); //Start the game
                }
                else//Content directory does not exist, so that means no access to game content.  Notify user and end the game.
                {
                    MessageBox.Show("No Content To Load", "Scorpion Engine");
                }
            }
        }
    }

#endif
}