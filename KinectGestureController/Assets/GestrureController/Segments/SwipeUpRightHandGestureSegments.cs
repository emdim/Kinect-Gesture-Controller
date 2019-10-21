using Windows.Kinect;

namespace GesturesInput.Gestures
{
    public class SwipeUpRightHandSegment1 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand right of right shoulder
            if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
            {
                //hand between spine base and head
                if (body.Joints[JointType.SpineBase].Position.Y < body.Joints[JointType.HandRight].Position.Y &&
                    body.Joints[JointType.Head].Position.Y > body.Joints[JointType.HandRight].Position.Y)
                {
                    //hand infront of elbow
                    if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
                    {
                        return GesturePartResult.Succeeded;
                    }
                    return GesturePartResult.Pausing;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }

    public class SwipeUpRightHandSegment2 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand right of right shoulder
            if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
            {
                //hand infront of right shouler
                if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
                {
                    //hand above of right shoulder
                    if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ShoulderRight].Position.Y)
                    {
                        return GesturePartResult.Succeeded;
                    }
                    return GesturePartResult.Pausing;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }

    public class SwipeUpRightHandSegment3 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand right of right shoulder
            if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
            {
                //hand infront of right shouler
                if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
                {
                    //hand above head
                    if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.Head].Position.Y)
                    {
                        return GesturePartResult.Succeeded;
                    }
                    return GesturePartResult.Pausing;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }
}
