using KDScorpionCore;
using KDScorpionCore.Plugins;
using MonoScorpPlugin;
using System;
using System.Collections.Generic;
using VelcroPhysicsPlugin;

namespace PluginSystem
{
    public class MonoPluginFactory : IPluginFactory
    {
        #region Methods
        public IRenderer CreateRenderer() => new MonoRenderer();


        public IContentLoader CreateContentLoader() => new MonoContentLoader();


        public IEngineCore CreateEngineCore() => new MonoEngineCore();


        public IKeyboard CreateKeyboard() => new MonoKeyboard();


        public IMouse CreateMouse() => new MonoMouse();


        public IDebugDraw CreateDebugDraw() => new MonoDebugDraw();


        public IPhysicsBody CreatePhysicsBody() => new VelcroBody();


        public IPhysicsBody CreatePhysicsBody(params object[] paramItems)
        {
            var paramTypeList = new Dictionary<int, Type>()
            {
                { 0, typeof(float[]) },
                { 1, typeof(float[]) },
                { 2, typeof(float) },
                { 3, typeof(float) },
                { 4, typeof(float) },
                { 5, typeof(float) },
                { 6, typeof(float) },
                { 7, typeof(float) },
                { 8, typeof(bool) }
            };

            if (paramItems.Length < paramTypeList.Count)
                throw new ArgumentException($"The param must have at least {paramTypeList.Count} parameters", nameof(paramItems));

            for (int i = 0; i < paramItems.Length; i++)
            {

                //If the current paremter is not the correct type
                if (paramItems[i].GetType() != paramTypeList[i])
                {
                    var typeSections = paramTypeList[i].ToString().Contains(".") ?
                        paramTypeList[i].ToString().Split('.') :
                        new string[0];

                    if (typeSections.Length > 0)
                        throw new ArgumentException($"Param number {i+ 1} is not the correct type.  The param must be of type '{typeSections[typeSections.Length - 1]}'.");
                }
            }


            return new VelcroBody((float[])paramItems[0], (float[])paramItems[1], (float)paramItems[2], (float)paramItems[3], (float)paramItems[4], (float)paramItems[5], (float)paramItems[6], (float)paramItems[7], (bool)paramItems[8]);
        }


        public IPhysicsWorld CreatePhysicsWorld() => new VelcroWorld();


        public IPhysicsWorld CreatePhysicsWorld(params object[] paramItems)
        {
            if (paramItems.Length < 2)
                throw new ArgumentException("The param must have at least 2 parameters of type 'float'.", nameof(paramItems));

            if (paramItems[0].GetType() != typeof(float))
                throw new ArgumentException("The first param must be of type 'float'");

            if (paramItems[1].GetType() != typeof(float))
                throw new ArgumentException("The second param must be of type 'float'");


            return new VelcroWorld((float)paramItems[0], (float)paramItems[1]);
        }
        #endregion
    }
}
