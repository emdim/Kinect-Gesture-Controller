using System.Collections.Generic;
using System.IO;
using Windows.Kinect;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GesturesInput
{
    public class Person
    {
        #region Enumerators

        /// <summary>
        /// Hand Id.
        /// </summary>
        public enum Hand
        {
            LEFT,
            RIGHT
        }

        #endregion

        #region Accesors

        /// <summary>
        /// Skeleton's tracking Id.
        /// </summary>
        public ulong TrackingId
        {
            get { return _body.TrackingId; }
        }

        /// <summary>
        /// State of the left hand.
        /// </summary>
        public HandState HandLeftState
        {
            get { return _body.HandLeftState; }
        }

        /// <summary>
        /// Tracking confidence of left hand (LOW - HIGH).
        /// </summary>
        public TrackingConfidence HandLeftConfidence
        {
            get { return _body.HandLeftConfidence; }
        }

        /// <summary>
        /// State of the right hand.
        /// </summary>
        public HandState HandRightState
        {
            get { return _body.HandRightState; }
        }

        /// <summary>
        /// Tracking confidence of right hand (LOW - HIGH).
        /// </summary>
        public TrackingConfidence HandRightConfidence
        {
            get { return _body.HandRightConfidence; }
        }

        /// <summary>
        /// Kinect positon of Left Hand.
        /// </summary>
        public CameraSpacePoint HandLeftPosition
        {
            get { return _body.Joints[JointType.HandLeft].Position; }
        }

        /// <summary>
        /// Kinect positon of Right Hand.
        /// </summary>
        public CameraSpacePoint HandRightPosition
        {
            get { return _body.Joints[JointType.HandRight].Position; }
        }

        /// <summary>
        /// Distance between left hand and spine base.
        /// </summary>
        public float HandLeftHeightFromSpine
        {
            get { return _body.Joints[JointType.HandLeft].Position.Y - _body.Joints[JointType.SpineBase].Position.Y; }
        }

        /// <summary>
        /// Distance between right hand and spine base.
        /// </summary>
        public float HandRightHeightFromSpine
        {
            get { return _body.Joints[JointType.HandRight].Position.Y - _body.Joints[JointType.SpineBase].Position.Y; }
        }

        /// <summary>
        /// Distance between Left Hand and spine base
        /// </summary>
        public float LeftHandDistanceFromSpineBase
        {
            get
            {
                return 0.5f + _body.Joints[JointType.HandLeft].Position.X -
                       _body.Joints[JointType.SpineBase].Position.X;
            }
        }

        /// <summary>
        /// Distance between Right Hand and spine base
        /// </summary>
        public float RightHandDistanceFromSpineBase
        {
            get
            {
                return 0.5f + _body.Joints[JointType.HandRight].Position.X +
                       _body.Joints[JointType.SpineBase].Position.X;
            }
        }

        #endregion

        #region Members

        /// <summary>
        /// State of the left hand on previous frame.
        /// </summary>
        public HandState HandLeftStatePrevious { get; private set; }

        /// <summary>
        /// State of the right hand on previous frame.
        /// </summary>
        public HandState HandRightStatePrevious { get; private set; }

        private Body _body;
        private readonly GestureController _gestureController;
        private readonly bool[] _gesturesStatus;
        private float _pauseBetweenGestures;
        private bool _debugMode;
        private bool _bypassForbidenGestures;
        private float _timePaused;
        private bool _isRecording;
        private List<Vector2Int> _handPositions = new List<Vector2Int>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new person object for managing the gestures.
        /// </summary>
        /// <param name="body">The body data.</param>
        /// <param name="debugMode">enable / diable the debug print for gesture recognition.</param>
        public Person(Body body, float pauseBetweenGestures = 0.0f, bool debugMode = false,
            bool bypassForbidenGestures = false)
        {
            _body = body;
            _debugMode = debugMode;
            _pauseBetweenGestures = pauseBetweenGestures;
            _bypassForbidenGestures = bypassForbidenGestures;
            _timePaused = Time.time;

            _gesturesStatus = new bool[System.Enum.GetValues(typeof(GestureId)).Length];
            _gestureController = new GestureController(_bypassForbidenGestures);
            _gestureController.GestureRecognized += OnGestureRecognized;
            _gestureController.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update Person's Body data.
        /// </summary>
        /// <param name="body">The new body data.</param>
        public void UpdateBodyData(Body body)
        {
            if (_body.HandLeftState != HandState.NotTracked && _body.HandLeftState != HandState.Unknown)
            {
                HandLeftStatePrevious = _body.HandLeftState;
            }

            if (_body.HandRightState != HandState.NotTracked && _body.HandRightState != HandState.Unknown)
            {
                HandRightStatePrevious = _body.HandRightState;
            }

            _body = body;
        }

        /// <summary>
        /// Update the gesture data.
        /// </summary>
        public void UpdateGestures()
        {
            if (_body == null) return;
            if (!_body.IsTracked) return;

            if (Time.time - _timePaused < _pauseBetweenGestures)
            {
                //Debug.Log("paused detection");
                return;
            }

            _gestureController.Update(_body);
        }

        public void ResetDetectedGestures()
        {
            System.Array.Clear(_gesturesStatus, 0, _gesturesStatus.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gestureId">The id of the gesture to check for.</param>
        /// <returns></returns>
        public bool GetGesture(GestureId gestureId)
        {
            return _gesturesStatus[(int) gestureId];
        }

        /// <summary>
        /// Used from GestureManager to reload the settings of the Person instance.
        /// </summary>
        /// <param name="debgMode">Boolean for printing debug messages.</param>
        /// <param name="bypassForbidenGestures">Boolean for bypassing frbiden gestures.</param>
        /// <param name="pauseBetweenGestures">Time to pause the next gesture detection.</param>
        public void ReloadSettings(bool debgMode, bool bypassForbidenGestures, float pauseBetweenGestures)
        {
            _debugMode = debgMode;
            _bypassForbidenGestures = bypassForbidenGestures;
            _pauseBetweenGestures = pauseBetweenGestures;

            _gestureController.ReloadSettings(_bypassForbidenGestures);
        }

        /// <summary>
        /// Starts hand position recording
        /// and clears the list of Vector 2 elements.
        /// </summary>
        public void StartRecordingHandPositions()
        {
            if (_isRecording) return;

            _isRecording = true;
            _handPositions.Clear();
        }

        /// <summary>
        /// Stops hand position recording for this person 
        /// and the List of Vector2 elements is saved in a csv file.
        /// </summary>
        /// <param name="personId">The id of current person to append in csv file name.</param>
        public void StopRecordingHandPositions(int personId)
        {
            if (!_isRecording) return;

            _isRecording = false;

            string path = GetPath(personId);
            CsvReadWrite.SaveCsv(_handPositions, path);
        }

        /// <summary>
        /// Adds the current hand's position in a List of Vector2 elements,
        /// when _isRecording value is true.
        /// </summary>
        public void RecordHandPosition()
        {
            if (!_isRecording) return;

            _handPositions.Add(
                HandLeftPosition.Z < HandRightPosition.Z ?
                ScreenPositionLeftHand() : 
                ScreenPositionRightHand());
        }

        /// <summary>
        /// Calculates and returns the position of left hand on screen.
        /// </summary>
        /// <returns>A Vector2 which represents the x and y coordinates of the left hand on the screen.</returns>
        public Vector2Int ScreenPositionLeftHand()
        {
            return new Vector2Int((int)(LeftHandDistanceFromSpineBase * Screen.width),
                (int)(HandLeftHeightFromSpine * Screen.height));
        }

        /// <summary>
        /// Calculates and returns the position of right hand on screen.
        /// </summary>
        /// <returns>A Vector2 which represents the x and y coordinates of the right hand on the screen.</returns>
        public Vector2Int ScreenPositionRightHand()
        {
            return new Vector2Int((int)(RightHandDistanceFromSpineBase * Screen.width),
                (int)(HandRightHeightFromSpine * Screen.height));
        }

        // Following method is used to retrive the relative path as device platform
        private static string GetPath(int playerId)
        {
#if UNITY_ANDROID
        return Application.dataPath + DateTime.Now.ToString("yyyy-M-dd HH-mm-ss") + ".csv";
#elif UNITY_IPHONE
        return Application.dataPath + DateTime.Now.ToString("yyyy-M-dd HH-mm-ss") + ".csv";
#else
            if (!Directory.Exists(string.Format("{0}/CSV/", Application.streamingAssetsPath)))
            {
                Directory.CreateDirectory(string.Format("{0}/CSV/", Application.streamingAssetsPath));
            }
            return string.Format("{0}/CSV/Player{1} ", Application.streamingAssetsPath, playerId) + System.DateTime.Now.ToString("yyyy-M-dd HH-mm-ss") + ".csv";
#endif
        }

        #endregion

        #region Event Handlers
        /// <summary>
        /// Actions when a gesture detected.
        /// </summary>
        /// <param name="sender">Sender of the current event.</param>
        /// <param name="e">Event data.</param>
        private void OnGestureRecognized(object sender, GestureEventArgs e)
        {
            if (_debugMode) Debug.Log("Gesture tracked: " + e.GestureId + " from body: " + e.TrackingId);
            _gesturesStatus[(int)e.GestureId] = true;
            _timePaused = Time.time;
        }

        #endregion
    }
}
