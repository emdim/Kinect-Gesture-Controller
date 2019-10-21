using Windows.Kinect;

namespace GesturesInput.Gestures
{
    public class SwipeRightWithRightHandSegment1 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //right hand infront of right elbow
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
            {
                //right hand between spine base and spine shoulder
                if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
                {
                    //right hand at the left of spine mid
                    if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.SpineMid].Position.X)
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

    public class SwipeRightWithRightHandSegment2 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //right hand infront of right elbow
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
            {
                //right hand between spine base and spine shoulder
                if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
                {
                    //right hand between shouder left and shoulder right
                    if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderLeft].Position.X &&
                        body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X)
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

    public class SwipeRightWithRightHandSegment3 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //right hand infront of right elbow
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
            {
                //right hand between spine base and spine shoulder
                if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
                {
                    //right hand at the right of right shoulder and right of right elbow
                    if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X &&
                        body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ElbowRight].Position.X)
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
