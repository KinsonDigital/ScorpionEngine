﻿using KDScorpionCore.Plugins;

namespace KDScorpionCore
{
    //TODO: Add docs
    public interface IText : IPlugin
    {
        int Width { get; }

        int Height { get; }

        string Text { get; set; }

        byte[] Color { get; set; }
    }
}
