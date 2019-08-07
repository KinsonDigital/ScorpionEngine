using System;

namespace ScorpTestGame
{
    public static class Program
    {
        [STAThread]
#pragma warning disable IDE0060 // Remove unused parameter
        private static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            using (var game = new MainGame())
            {
                game.Start();
            }
        }
    }
}
