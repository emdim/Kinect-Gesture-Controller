using Windows.Kinect;

namespace GesturesInput.Gestures
{
    public class ZoomInSegment1 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //both hands in front of elbows
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z &&
                body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
            {
                //both hands up of spine base and down of spine shoulder
                if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y &&
                    body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y &&
                    body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    //both hands between shoulders
                    if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X &&
                        body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderLeft].Position.X &&
                        body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderRight].Position.X &&
                        body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderLeft].Position.X)
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

    public class ZoomInSegment2 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //both hands in front of elbows
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z &&
                body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
            {
                //both hands up of spine base and down of spine shoulder
                if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y &&
                    body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y &&
                    body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    //both hands outside of shoulders
                    if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X &&
                        body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
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

    public class ZoomInSegment3 : IRelativeGestureSegment
    {
        public GesturePartResult CheckGesture(Body body)
        {
            //both hands in front of elbows
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z &&
                body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
            {
                //both hands up of spine base and down of spine shoulder
                if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y &&
                    body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y &&
                    body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    //both hands outside of elbows
                    if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ElbowRight].Position.X &&
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
