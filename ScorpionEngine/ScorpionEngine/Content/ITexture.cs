﻿using ScorpionEngine.Core;

namespace ScorpionEngine.Content
{
    public interface ITexture
    {
        int Width { get; set; }

        int Height { get; set; }

        T GetTexture<T>() where T : class;
    }
}