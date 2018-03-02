namespace ScorpionEngine
{
    /// <summary>
    /// Represents a point in space that another entity can attach its position to.
    /// </summary>
    public class AnchorPoint
    {
        #region Properties

        /// <summary>
        /// Gets or sets the source entity ID.
        /// </summary>
        public int SourceID { get; set; }

        /// <summary>
        /// Gets or sets the destination entity ID.
        /// </summary>
        public int DestID { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an anchor point from the source entity to the destination entity.  The source entity will follow the destination entity.
        /// </summary>
        /// <param name="sourceID">The ID of the source entity that attaches to the destination entity.</param>
        /// <param name="destID">The ID of the destination entity that attaches to the source entity.</param>
        public AnchorPoint(int sourceID, int destID)
        {
            SourceID = sourceID;
            DestID = destID;
        }

        #endregion
    }
}