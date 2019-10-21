using Windows.Kinect;

namespace GesturesInput.Gestures
{
    public class SwipeLeftWithLeftHandSegment1 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //left hand infront of left elbow
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
            {
                //left hand between spine base and spine shoulder
                if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
                {
                    //left hand at the right of spine mid
                    if (body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.SpineMid].Position.X)
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

    public class SwipeLeftWithLeftHandSegment2 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //right hand infront of right elbow
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
            {
                //right hand between spine base and spine shoulder
                if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
                {
                    //left hand between shouder left and shoulder right
                    if (body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderLeft].Position.X &&
                        body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderRight].Position.X)
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

    public class SwipeLeftWithLeftHandSegment3 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //right hand infront of right elbow
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
            {
                //right hand between spine base and spine shoulder
                if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
                {
                    //left hand at the left of left shoulder and left of left elbow
                    if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderRight].Position.X &&
                        body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ElbowLeft].Position.X)
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
