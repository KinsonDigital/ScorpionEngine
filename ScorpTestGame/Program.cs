using System;

namespace ScorpTestGame
{
    public static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            using (var game = new TestGame())
            {
                game.Start();
            }
        }
    }
}
