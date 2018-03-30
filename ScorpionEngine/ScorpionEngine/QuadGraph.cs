namespace ScorpionEngine
{
    /// <summary>
    /// Holds and calculates the quad position of a location compared to another location.
    /// https://www.mathsisfun.com/definitions/quadrant-graph-.html
    /// </summary>
    public struct QuadGraph
    {
        #region Props
        /// <summary>
        /// Gets the position of the quad graph in world space.
        /// </summary>
        public Vector GraphPosition { get; private set; }

        /// <summary>
        /// Gets the point in world space that is in 1 of the 4 quadrants.
        /// </summary>
        public Vector GraphPoint { get; private set; }

        /// <summary>
        /// Gets the angle in degrees that is pointing to 1 of the 4 quadrants.
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets the quad that the GraphPoint resides in.
        /// </summary>
        public short PointQuad { get; private set; }

        /// <summary>
        /// Gets the quad that the Angle points to.
        /// </summary>
        public short AngleQuad { get; private set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the position of the quad graph and the single point's location in the graph.
        /// </summary>
        /// <param name="graphPosition">The position of the graph's center point in the game world.</param>
        /// <param name="graphPoint">A single point in space in one of the 4 quadrants of the quad graph.</param>
        /// <param name="angle">An angle in degrees at the graph's position that can point to one out of 4 of the graph quadrants.</param>
        public void Update(Vector graphPosition, Vector graphPoint, float angle)
        {
            GraphPosition = graphPosition;
            GraphPoint = graphPoint;
            Angle = angle;

            PointQuad = CalcGraphPointQuad();
            AngleQuad = CalcAngleQuad();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Returns the quad location from 1 to 4 that the graph point lives in.
        /// </summary>
        private short CalcGraphPointQuad()
        {
            //Mouse quad using position
            if (GraphPoint.X > GraphPosition.X && GraphPoint.Y < GraphPosition.Y)//QUAD 1
                return 1;

            if (GraphPoint.X > GraphPosition.X && GraphPoint.Y > GraphPosition.Y)//QUAD 2
                return 2;

            if (GraphPoint.X < GraphPosition.X && GraphPoint.Y > GraphPosition.Y)//QUAD 3
                return 3;

            if (GraphPoint.X < GraphPosition.X && GraphPoint.Y < GraphPosition.Y)//QUAD4
                return 4;

            return -1;
        }


        /// <summary>
        /// Returns the quad location form 1 to 4 that the angle is pointing to.
        /// </summary>
        private short CalcAngleQuad()
        {
            short quad = -1;

            while (quad == -1)
            {
                if (Angle >= 0 && Angle <= 90) //QUAD 1
                {
                    quad = 1;
                }
                else if (Angle >= 91 && Angle <= 180) //QUAD 2
                {
                    quad = 2;
                }
                else if (Angle >= 181 && Angle <= 270) //QUAD 3
                {
                    quad = 3;
                }
                else if (Angle >= 271 && Angle <= 360) //QUAD 4
                {
                    quad = 4;
                }
                else //Increase the bodyAngleDeg by 1 degree to nudge it into a quad
                {
                    Angle += 1;
                }

                Angle = Angle > 360 ? 0.00001f : Angle;
            }

            return quad;
        }
        #endregion
    }
}