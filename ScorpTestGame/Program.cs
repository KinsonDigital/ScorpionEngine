// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable IDE0060 // Remove unused parameter
namespace ScorpTestGame
{
    using System;
    using System.Drawing;
    using System.IO;
    using KDScorpionEngine.Content;
    using Newtonsoft.Json;

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
