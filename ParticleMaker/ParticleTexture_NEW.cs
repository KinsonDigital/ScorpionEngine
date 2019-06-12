﻿using SDL2;
using System;
using System.Drawing;

namespace ParticleMaker
{
    public class ParticleTexture_NEW
    {
        public ParticleTexture_NEW(IntPtr texturePtr, int width, int height)
        {
            TexturePointer = texturePtr;
            Width = width;
            Height = height;
        }

        public string Name { get; set; }

        public IntPtr TexturePointer { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Color Color { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public override int GetHashCode() => Name.GetHashCode();
    }
}
