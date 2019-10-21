using Windows.Kinect;

namespace GesturesInput.Gestures
{
    public class SwipeDownLeftHandSegment1 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand Left of Left shoulder
            if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
            {
                //hand infront of Left shouler
                if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
                {
                    //hand below head and up from elbow
                    if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.Head].Position.Y &&
                        body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ElbowLeft].Position.Y)
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

    public class SwipeDownLeftHandSegment2 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand Left of Left shoulder
            if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
            {
                //hand infront of Left shouler
                if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
                {
                    //hand below elbow
                    if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.ElbowLeft].Position.Y)
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

    public class SwipeDownLeftHandSegment3 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand Left of Left shoulder
            if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
            {
                //hand infront of Left shouler
                if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
                {
                    //hand below Left hip
                    if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.HipLeft].Position.Y)
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
