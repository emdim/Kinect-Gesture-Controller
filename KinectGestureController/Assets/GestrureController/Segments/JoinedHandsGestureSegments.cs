using Windows.Kinect;

namespace GesturesInput.Gestures
{
    class JoinedHandsSegments : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            if ((body.Joints[JointType.ElbowLeft].Position.Z > body.Joints[JointType.HandLeft].Position.Z) &&
                (body.Joints[JointType.ElbowRight].Position.Z > body.Joints[JointType.HandRight].Position.Z))
            {
                if ((body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y) &&
                    (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y) &&
                    (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y) &&
                    (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y))
                {
                    if ((body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderLeft].Position.X) &&
                        (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X))
                    {
                        if (body.Joints[JointType.HandRight].Position.X - body.Joints[JointType.HandLeft].Position.X < 0)
                        {
                            return GesturePartResult.Succeeded;
                        }
                        return GesturePartResult.Pausing;
                    }
                    return GesturePartResult.Failed;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }
}
