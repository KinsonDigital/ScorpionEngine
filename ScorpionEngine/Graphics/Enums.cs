using System;
using System.Collections.Generic;
using System.Text;

namespace KDScorpionEngine.Graphics
{
    public enum TextureType
    {
        Single,
        Atlas,
    }

    public enum AnimateDirection
    {
        Forward,
        Reverse,
        None,
    }

    public enum AnimateState
    {
        Playing,
        Paused,
    }
}
