using MonoScorpPlugin;
using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Objects;
using ScorpionEngine.Scene;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysicsDriver;

namespace ScorpTestGame.Scenes
{
    public class MainScene : GameScene
    {
        public VelcroWorld _world;
        public MonoText _helloWorld;
        public MonoTexture _orangeRectTexture;
        public GameObject _orangeObject;

    }
}
