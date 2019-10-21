using Windows.Kinect;

namespace GesturesInput.Gestures
{
    /// <summary>
    /// The first part of a <see cref="GestureId.WaveLeftHand"/> gesture.
    /// </summary>
    public class WaveLeftHandSegment1 : IRelativeGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult CheckGesture(Body body)
        {
            // Hand above elbow
            if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ElbowLeft].Position.Y)
            {
                // Hand Left of elbow
                if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ElbowLeft].Position.X)
                {
                    return GesturePartResult.Succeeded;
                }

                // Hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return GesturePartResult.Pausing;
            }

            // Hand dropped - no gesture fails
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The second part of a <see cref="GestureId.WaveLeftHand"/> gesture.
    /// </summary>
    public class WaveLeftHandSegment2 : IRelativeGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult CheckGesture(Body body)
        {
            // Hand above elbow
            if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ElbowLeft].Position.Y)
            {
                // Hand left of elbow
                if (body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ElbowLeft].Position.X)
                {
                    return GesturePartResult.Succeeded;
                }

                // Hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return GesturePartResult.Pausing;
            }

            // Hand dropped - no gesture fails
            return GesturePartResult.Failed;
        }
    }
}
