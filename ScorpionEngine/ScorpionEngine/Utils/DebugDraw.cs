using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScorpionEngine.Utils
{
    internal class DebugDraw
    {
        private GraphicsDevice _graphicsDevice;
        private BasicEffect _basicEffect;
        private SpriteBatch _spriteBatch;


        #region Constructor
        public DebugDraw(GraphicsDevice graphicsDevice, bool enabled = false)
        {

        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets a value indicating if debug drawing should be enabled.
        /// </summary>
        public bool Enabled { get; set; }
        #endregion


        #region Overridden Methods
        public void DrawPolygon(List<Vector2> vertices, int count, Color color)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Draw all of the solid polygons.
        /// </summary>
        /// <param name="vertices">The vertices that make up the shape of the polygon.</param>
        /// <param name="count">The number of vertices to use out of the list of vertices.</param>
        /// <param name="color">The color to draw the lines.</param>
        public  void DrawSolidPolygon(List<Vector2> vertices, int count, Color color)
        {
            throw new NotImplementedException();
        }


        public void DrawCircle(Vector2 center, float radius, Color color)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Draws all circles.
        /// </summary>
        /// <param name="center">The center coordinate of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="axis">The axis of the circle.</param>
        /// <param name="color">The color to draw the circle.</param>
        public void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, Color color)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Draw any joint segments in the engine.
        /// </summary>
        /// <param name="p1">The first point of the joint.</param>
        /// <param name="p2">The second point of the joint.</param>
        /// <param name="color">The color of the joint.</param>
        public void DrawSegment(Vector2 p1, Vector2 p2, Color color)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}