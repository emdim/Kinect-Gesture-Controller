using GesturesInput.Gestures;
using System.Collections.Generic;
using System.Linq;
using Windows.Kinect;

namespace GesturesInput
{
    /// <summary>
    /// Represents a gesture controller.
    /// </summary>
    public class GestureController
    {
        #region Members

        /// <summary>
        /// A list of all the gestures the controller is searching for.
        /// </summary>
        private readonly List<Gesture> _gestures = new List<Gesture>();
        private bool _isRunning;
        private GestureId[] _previousGestureForbidenIds = { };
        private bool _bypassForbidenGestures;

        /// <summary>
        /// Get the status of the controller.
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="GestureController" /> with all of the available gesture types.
        /// </summary>
        public GestureController(bool bypassForbidenGestures = false)
        {
            _bypassForbidenGestures = bypassForbidenGestures;
            foreach (GestureId t in System.Enum.GetValues(typeof(GestureId)))
            {
                AddGesture(t);
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GestureController" /> with the specified gesture type.
        /// </summary>
        /// <param name="bypassForbidenGestures">Ignores the forbiden gestre check.</param>
        /// <param name="type">The gesture type to recognize.</param>
        public GestureController(bool bypassForbidenGestures = false, params GestureId[] type)
        {
            _bypassForbidenGestures = bypassForbidenGestures;
            AddGesture(type);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a gesture is recognized.
        /// </summary>
        public event System.EventHandler<GestureEventArgs> GestureRecognized;

        #endregion

        #region Methods

        /// <summary>
        /// Starts the controller.
        /// </summary>
        public virtual void Start()
        {
            _isRunning = true;
        }

        /// <summary>
        /// Stops the controller.
        /// </summary>
        public virtual void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Updates all gestures.
        /// </summary>
        /// <param name="body">The body data to search for gestures.</param>
        public void Update(Body body)
        {
            if (!_isRunning || body == null) return;

            foreach (Gesture gesture in _gestures)
            {
                gesture.Update(body);
            }
        }

        /// <summary>
        /// Adds the specified gesture for recognition.
        /// </summary>
        /// <param name="types">The predefined <see cref="GestureId" />.</param>
        public void AddGesture(params GestureId[] types)
        {
            foreach (GestureId type in types)
            {
                // Check whether the gesure is already added.
                if (_gestures.Any(g => g.GestureType == type)) return;

                IRelativeGestureSegment[] segments = null;
                GestureId[] forbidenGestures = { };

                // DEVELOPERS: If you add a new predefined gesture with a new GestureType,
                // simply add the proper segments to the switch statement here.
                switch (type)
                {
                    case GestureId.JoinedHands:
                        segments = new IRelativeGestureSegment[5];

                        var joinedhandsSegment = new JoinedHandsSegments();
                        for (var i = 0; i < segments.Length; i++)
                        {
                            segments[i] = joinedhandsSegment;
                        }
                        break;
                    case GestureId.WaveRightHand:
                        var waveRightSegment1 = new WaveRightHandSegment1();
                        var waveRightSegment2 = new WaveRightHandSegment2();

                        segments = new IRelativeGestureSegment[]
                        {
                            waveRightSegment1,
                            waveRightSegment2,
                            waveRightSegment1,
                            waveRightSegment2
                        };

                        forbidenGestures = new[]
                        {
                            GestureId.SwipeDownRightHand,
                            GestureId.SwipeDownLeftHand
                        };
                        break;
                    case GestureId.WaveLeftHand:
                        var waveLeftSegment1 = new WaveLeftHandSegment1();
                        var waveLeftSegment2 = new WaveLeftHandSegment2();

                        segments = new IRelativeGestureSegment[]
                        {
                            waveLeftSegment1,
                            waveLeftSegment2,
                            waveLeftSegment1,
                            waveLeftSegment2
                        };

                        forbidenGestures = new[]
                        {
                            GestureId.SwipeDownRightHand,
                            GestureId.SwipeDownLeftHand
                        };
                        break;
                    case GestureId.SwipeUpRightHand:
                        segments = new IRelativeGestureSegment[]
                        {
                            new SwipeUpRightHandSegment1(),
                            new SwipeUpRightHandSegment2(),
                            new SwipeUpRightHandSegment3()
                        };

                        forbidenGestures = new[]
                        {
                            GestureId.SwipeDownRightHand,
                            GestureId.SwipeDownLeftHand
                        };
                        break;
                    case GestureId.SwipeUpLeftHand:
                        segments = new IRelativeGestureSegment[]
                        {
                            new SwipeUpLeftHandSegment1(),
                            new SwipeUpLeftHandSegment2(),
                            new SwipeUpLeftHandSegment3()
                        };

                        forbidenGestures = new[]
                        {
                            GestureId.SwipeDownRightHand,
                            GestureId.SwipeDownLeftHand
                        };
                        break;
                    case GestureId.SwipeDownRightHand:
                        segments = new IRelativeGestureSegment[]
                        {
                            new SwipeDownRightHandSegment1(),
                            new SwipeDownRightHandSegment2(),
                            new SwipeDownRightHandSegment3()
                        };
                        break;
                    case GestureId.SwipeDownLeftHand:
                        segments = new IRelativeGestureSegment[]
                        {
                            new SwipeDownLeftHandSegment1(),
                            new SwipeDownLeftHandSegment2(),
                            new SwipeDownLeftHandSegment3()
                        };
                        break;
                    case GestureId.ZoomIn:
                        segments = new IRelativeGestureSegment[]
                        {
                            new ZoomInSegment1(),
                            new ZoomInSegment2(),
                            new ZoomInSegment3()
                        };
                        break;
                    case GestureId.ZoomOut:
                        segments = new IRelativeGestureSegment[]
                        {
                            new ZoomInSegment3(),
                            new ZoomInSegment2(),
                            new ZoomInSegment1()
                        };
                        break;
                    case GestureId.SwipeLeftWithRightHand:
                        segments = new IRelativeGestureSegment[]
                        {
                            new SwipeLeftWithRightHandSegment1(),
                            new SwipeLeftWithRightHandSegment2(),
                            new SwipeLeftWithRightHandSegment3()
                        };
                        break;
                    case GestureId.SwipeLeftWithLeftHand:
                        segments = new IRelativeGestureSegment[]
                        {
                            new SwipeLeftWithLeftHandSegment1(),
                            new SwipeLeftWithLeftHandSegment2(),
                            new SwipeLeftWithLeftHandSegment3()
                        };
                        break;
                    case GestureId.SwipeRightWithLeftHand:
                        segments = new IRelativeGestureSegment[]
                        {
                            new SwipeRightWithLeftHandSegment1(),
                            new SwipeRightWithLeftHandSegment2(),
                            new SwipeRightWithLeftHandSegment3()
                        };
                        break;
                    case GestureId.SwipeRightWithRightHand:
                        segments = new IRelativeGestureSegment[]
                        {
                            new SwipeRightWithRightHandSegment1(),
                            new SwipeRightWithRightHandSegment2(),
                            new SwipeRightWithRightHandSegment3()
                        };
                        break;
                    default:
                        break;
                }

                if (segments != null)
                {
                    Gesture gesture = new Gesture(type, segments, forbidenGestures);
                    gesture.GestureRecognized += OnGestureRecognized;

                    _gestures.Add(gesture);
                }
            }
        }

        /// <summary>
        /// Used from GestureManager to reload the settings of the GestureController instance.
        /// </summary>
        /// <param name="bypassForbidenGestures">Boolean for bypassing frbiden gestures.</param>
        public void ReloadSettings(bool bypassForbidenGestures)
        {
            _bypassForbidenGestures = bypassForbidenGestures;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the GestureRecognized event of the g control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GestureEventArgs"/> instance containing the event data.</param>
        private void OnGestureRecognized(object sender, GestureEventArgs e)
        {
            if (GestureRecognized != null)
            {
                //UnityEngine.Debug.Log("Previous: " + _previousGestureId);
                //UnityEngine.Debug.Log("Detected: "+e.GestureId);
                //UnityEngine.Debug.Log("Forbiden: ");
                //foreach (var gestureId in e.ForbidenGestures)
                //{
                //    UnityEngine.Debug.Log(gestureId.ToString());
                //}
                //UnityEngine.Debug.Log("============================");
                if (!_previousGestureForbidenIds.Contains(e.GestureId) || _bypassForbidenGestures)
                {
                    GestureRecognized(this, e);
                }
                _previousGestureForbidenIds = e.ForbidenGestures;
            }

            foreach (Gesture gesture in _gestures)
            {
                gesture.Reset();
            }
        }

        #endregion
    }
}
