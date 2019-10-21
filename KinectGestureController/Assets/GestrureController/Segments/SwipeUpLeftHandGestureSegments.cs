using Windows.Kinect;

namespace GesturesInput.Gestures
{
    public class SwipeUpLeftHandSegment1 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand Left of Left shoulder
            if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
            {
                //hand between spine base and head
                if (body.Joints[JointType.SpineBase].Position.Y < body.Joints[JointType.HandLeft].Position.Y &&
                    body.Joints[JointType.Head].Position.Y > body.Joints[JointType.HandLeft].Position.Y)
                {
                    //hand infront of elbow
                    if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
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

    public class SwipeUpLeftHandSegment2 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand Left of Left shoulder
            if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
            {
                //hand infront of Left shouler
                if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
                {
                    //hand above of Left shoulder
                    if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ShoulderLeft].Position.Y)
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

    public class SwipeUpLeftHandSegment3 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand Left of Left shoulder
            if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
            {
                //hand infront of Left shouler
                if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
                {
                    //hand above head
                    if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.Head].Position.Y)
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
