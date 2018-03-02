using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using ScorpionEngine;
using ScorpionEngine.Objects;
using ScorpionEngine.Utils;
using Microsoft.Xna.Framework;

namespace ScorpionEngine
{
    /// <summary>
    /// Creates a new instance of a game world.
    /// </summary>
    public abstract class World
    {
        #region Events
        /// <summary>
        /// Occurs when an obj is removed from the world.
        /// </summary>
        public static event EventHandler OnEntityRemoved;
        #endregion

        #region Fields
        private static readonly List<int> _idNumbers = new List<int>();//Generated ID numbers that are assigned to game objects.
        private static readonly List<GameObject> _gameObjects = new List<GameObject>();//The list of entites that have been added to the world
        private readonly List<ObjectPool> _pools;//The list of obj pools that have been added to the world
        private readonly List<AnchorPoint> _anchors;//The anchor points of the obj
        private readonly Dictionary<int, Texture2D> _backgrounds = new Dictionary<int, Texture2D>();
        private static readonly List<SpriteFont> _debugTexts = new List<SpriteFont>(); 
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a game world.
        /// </summary>
        protected World(Vector gravity)
        {
            _pools = new List<ObjectPool>();
            _anchors = new List<AnchorPoint>();
        }
        #endregion

        #region Props
        /// <summary>
        /// Gets the list of entities.
        /// </summary>
        public static List<GameObject> GameObjects
        {
            get
            {
                return _gameObjects;
            }
        }

        /// <summary>
        /// Gets the pools that have been added to the world.
        /// </summary>
        public IEnumerable<ObjectPool> Pools
        {
            get
            {
                return _pools;
            }
        }

        #region Internal Props
        /// <summary>
        /// Gets or sets a value indicating if debug drawing is enabled.
        /// </summary>
        public static bool DebugDrawEnabled
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion
        #endregion

        #region Public Methods
        /// <summary>
        /// Draws the debug data.
        /// </summary>
        internal static void DrawDebugData()
        {
            //TODO: DRAW DEBUG DATA
        }

        /// <summary>
        /// Adds the given obj pool to the world.  Duplicate pool names in a world are not aloud.
        /// </summary>
        /// <param name="pool">The pool to add to the world.</param>
        public void AddPool(ObjectPool pool)
        {
            //If the pool's name doesn't exist already.
            if (_pools.Exists(item => item.Name == pool.Name))
                throw new Exception("The world already contains a pool with the name " + pool.Name);

            _pools.Add(pool);
        }

        /// <summary>
        /// Generates a new ID that can be used to assign to a game object.
        /// </summary>
        /// <returns></returns>
        public static int GenerateID()
        {
            //If there are no entities, just create an ID of 1
            var newID = (_idNumbers.Count <= 0) ? 1 : _idNumbers.Max(item => item) + 1;//The new id to assign

            _idNumbers.Add(newID);

            return newID;
        }
        #endregion

        #region Game Loop Methods
        /// <summary>
        /// Used to be overriden by inherited world classes to initialize objects.
        /// </summary>
        public virtual void OnInit()
        {
        }

        /// <summary>
        /// Used to be overriden  by inherited world classes to load content.
        /// </summary>
        public virtual void OnLoadContent()
        {
        }

        /// <summary>
        /// Used to be overriden by inhertied world classes to update objects and the game.
        /// Also calls the update method for all of the entites added to the world.
        /// </summary>
        /// <param name="engineTime">The time elapsed since the last frame.</param>
        public virtual void OnUpdate(EngineTime engineTime)
        {
            //Call the update method for each obj
            foreach (var obj in _gameObjects)
            {
                obj.OnUpdate(engineTime);
            }

            //Call the update for all of the obj pools in the current game world
            foreach (var obj in _pools)
            {
                obj.OnUpdate(engineTime);
            }

            //TODO: Update the physics engine
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Add the given obj to the game world.  GameObjects with the same name not aloud.
        /// </summary>
        public static void AddGameObj(GameObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException();

            _gameObjects.Add(obj);//Add the new obj to the obj list
        }

        /// <summary>
        /// Removes the ID that matches the given ID number.
        /// </summary>
        /// <param name="id">The id number of the obj to remove.</param>
        public static void RemoveGameObj(int id)
        {
            for (var i = 0; i < _gameObjects.Count; i++)
            {
                //If the id matches
                if (_gameObjects[i].ID == id)
                {
                    _gameObjects.RemoveAt(i);//Remove the obj

                    //Remove the ID from the id numbers list
                    _idNumbers.Remove(id);

                    if (OnEntityRemoved != null)
                        OnEntityRemoved.Invoke(null, new EventArgs());
                    break;
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks to see if an obj with the given name already exists.
        /// </summary>
        /// <param name="name">The name of the obj to check for.</param>
        /// <returns></returns>
        private static bool EntityExists(int id)
        {
            //Checks each obj
            for (var i = 0; i < _gameObjects.Count; i++)
            {
                //If the obj's name matches the given name
                if (_gameObjects[i].ID == id)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}