using Windows.Kinect;

namespace GesturesInput.Gestures
{
    /// <summary>
    /// The first part of a <see cref="GestureId.WaveRightHand"/> gesture.
    /// </summary>
    public class WaveRightHandSegment1 : IRelativeGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult CheckGesture(Body body)
        {
            // Hand above elbow
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
            {
                // Hand right of elbow
                if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ElbowRight].Position.X)
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
    /// The second part of a <see cref="GestureId.WaveRightHand"/> gesture.
    /// </summary>
    public class WaveRightHandSegment2 : IRelativeGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult CheckGesture(Body body)
        {
            // Hand above elbow
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
            {
                // Hand left of elbow
                if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ElbowRight].Position.X)
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
