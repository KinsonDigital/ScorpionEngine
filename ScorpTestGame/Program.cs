// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable IDE0060 // Remove unused parameter
namespace ScorpTestGame
{
    using System;

    public static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            using var game = new MainGame();

            game.RunAsync().Wait();
        }
    }
}
