using System;

namespace GesturesInput
{
    /// <summary>
    /// The gesture event arguments.
    /// </summary>
    public class GestureEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the gesture type.
        /// </summary>
        public GestureId GestureId { get; private set; }

        /// <summary>
        /// Gets the skeleton tracking ID for the gesture.
        /// </summary>
        public ulong TrackingId { get; private set; }

        /// <summary>
        /// Gestures that are forbiden to execute next
        /// </summary>
        public GestureId[] ForbidenGestures { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="GestureEventArgs"/>.
        /// </summary>
        public GestureEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GestureEventArgs"/>.
        /// </summary>
        /// <param name="type">The gesture type.</param>
        /// <param name="trackingID">The tracking ID.</param>
        public GestureEventArgs(GestureId type, GestureId[] forbidenGestures, ulong trackingID)
        {
            GestureId = type;
            ForbidenGestures = forbidenGestures;
            TrackingId = trackingID;
        }

        #endregion
    }
}
