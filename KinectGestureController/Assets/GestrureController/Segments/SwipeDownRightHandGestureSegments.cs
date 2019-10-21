using Windows.Kinect;

namespace GesturesInput.Gestures
{
    public class SwipeDownRightHandSegment1 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand right of right shoulder
            if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
            {
                //hand infront of right shouler
                if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
                {
                    //hand below head and up from elbow
                    if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.Head].Position.Y &&
                        body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
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

    public class SwipeDownRightHandSegment2 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand right of right shoulder
            if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
            {
                //hand infront of right shouler
                if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
                {
                    //hand below elbow
                    if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.ElbowRight].Position.Y)
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

    public class SwipeDownRightHandSegment3 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //hand right of right shoulder
            if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
            {
                //hand infront of right shouler
                if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
                {
                    //hand below right hip
                    if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.HipRight].Position.Y)
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
