using System.Collections.Generic;
using System.Linq;
using Windows.Kinect;
using GesturesInput;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Represents the Manager of the Persons in the Kinect Camera view
/// </summary>
[AddComponentMenu("Kinect/Gesture Manager")]
[DisallowMultipleComponent]
public class GestureManager : MonoBehaviour
{

    #region Members
    /// <summary>
    /// Maximum people allowed to be recognized by the Kinect sensor.
    /// </summary>
    [Range(1, 6)]
    public int MaximumPlayers = 1;
    /// <summary>
    /// Delay between gesture recognitions.
    /// </summary>
    [Range(0, 0.5f)]
    public float PauseBetweenGestures;
    /// <summary>
    /// Print messages on console when a gesture recognized.
    /// </summary>
    public bool DebugMode;
    /// <summary>
    /// Ignores the forbiden gestre check.
    /// </summary>
    public bool BypassForbidenGestures;
    /// <summary>
    /// People detected by kinect sensor.
    /// </summary>
    public int AvailablePlayers { get { return _people.Count; } }

    public float RecordingDelay = 0.5f;

    private KinectSensor _sensor;
    private BodyFrameReader _reader;
    private Body[] _bodies;
    private static readonly Dictionary<ulong, Person> _people = new Dictionary<ulong, Person>();
    //private static bool _gotFrame;
    private float _handsPositionUpdateTime;

    #endregion

    #region Monobehaviour Methods

#if UNITY_EDITOR
    /// <summary>
    /// Use Reset() function to check if component can be added to the gameobject.
    /// </summary>
    private void Reset()
    {
        if (FindObjectsOfType<GestureManager>().Length <= 1) return;

        string msg = string.Format("Can't add '{0}' component because it already exists in {1} scene.",
            typeof(GestureManager).Name, SceneManager.GetActiveScene().name);

        EditorUtility.DisplayDialog("Invalid operation", msg, "Ok");
        Invoke("DestroyDuplicateComponent", 0);
    }

    void DestroyDuplicateComponent()
    {
        DestroyImmediate(this);
    }
#endif

    /// <summary>
    /// Initialize and open the Kinect sensor in order to be able to get the body data.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
            Debug.LogWarning(string.Format("Duplicate {0} found destroyed extra Gameobject", GetType()));
        }

        _sensor = KinectSensor.GetDefault();

        if (_sensor == null) return;

        _reader = _sensor.BodyFrameSource.OpenReader();

        if (!_sensor.IsOpen) { _sensor.Open(); }

        _handsPositionUpdateTime = Time.time;

        Debug.Log("Kinect Gesture Manager Initilized!");

    }

    /// <summary>
    /// Update body frame data in each frame.
    /// </summary>
    private void Update()
    {
        if (!_sensor.IsAvailable) return;

        UpdateFrame();
        RecordHandsPosition();
    }

    #endregion

    #region Methods

    private void RecordHandsPosition()
    {
        if (Time.time - _handsPositionUpdateTime >= RecordingDelay)
        {
            _handsPositionUpdateTime = Time.time;
            foreach (var personKeyValuePair in _people)
            {
                personKeyValuePair.Value.RecordHandPosition();
            }
        }
    }

    /// <summary>
    /// Updates the frame, body and people data.
    /// </summary>
    private void UpdateFrame()
    {
        foreach (Person person in _people.Values)
        {
            person.ResetDetectedGestures();
        }

        if (_reader == null) return;
        BodyFrame frame = _reader.AcquireLatestFrame();
        if (frame == null) { return; }

        _bodies = new Body[_sensor.BodyFrameSource.BodyCount];
        frame.GetAndRefreshBodyData(_bodies);

        UpdatePeople(_bodies);
        foreach (Person person in _people.Values)
        {
            person.UpdateGestures();
        }

        frame.Dispose();
        frame = null;
    }

    /// <summary>
    /// Gets the confidence of the current cursor hand.
    /// </summary>
    /// <param name="personId">Person index id in the list of detected people</param>
    /// <returns>The tracking confidence of the cursor hand (LOW - HIGH).</returns>
    public static TrackingConfidence GetCursorHandConfidence(int personId = 1)
    {
        int personIndex = personId - 1;
        if (!IsValidIndex(personIndex)) return TrackingConfidence.Low;

        Person person = _people.ElementAt(personIndex).Value;

        return person.HandLeftPosition.Z < person.HandRightPosition.Z
            ? person.HandLeftConfidence
            : person.HandRightConfidence;

    }

    /// <summary>
    /// Notify when the requested hand state started.
    /// </summary>
    /// <param name="handState">State to be checked.</param>
    /// <param name="hand">Person's hand to detect for the state.</param>
    /// <param name="personId">Person index id in the list of detected people</param>
    /// <returns>Returs true in frame that the requested state starts.</returns>
    public static bool HandStateStart(HandState handState, Person.Hand hand, int personId = 1)
    {
        return GetHandState(hand, personId) != HandState.Unknown && GetHandState(hand, personId) != HandState.NotTracked &&
            GetHandStatePrevious(hand, personId) != handState && GetHandState(hand, personId) == handState;
    }

    /// <summary>
    /// Notify when the requested cursor's hand state starts.
    /// </summary>
    /// <param name="handState">State to be checked.</param>
    /// <param name="personId">Person index id in the list of detected people</param>
    /// <returns>Returs true in frame that the requested state starts.</returns>
    public static bool CursorHandStateStart(HandState handState, int personId = 1)
    {

        return GetCursorHandState(personId) != HandState.Unknown && GetCursorHandState(personId) != HandState.NotTracked &&
            GetCursorHandStatePrevious(personId) != handState && GetCursorHandState(personId) == handState;
    }

    /// <summary>
    /// Notify when the requested hand state ended.
    /// </summary>
    /// <param name="handState">State to be checked.</param>
    /// <param name="hand">Person's hand to detect for the state.</param>
    /// <param name="personId">Person index id in the list of detected people</param>
    /// <returns>Returs true in frame that the requested state stoped.</returns>
    public static bool HandStateEnd(HandState handState, Person.Hand hand, int personId = 1)
    {
        return GetHandState(hand, personId) != HandState.Unknown && GetHandState(hand, personId) != HandState.NotTracked &&
            GetHandStatePrevious(hand, personId) == handState && GetHandState(hand, personId) != handState;
    }

    /// <summary>
    /// Notify when the requested cursor's hand state stoped.
    /// </summary>
    /// <param name="handState">State to be checked.</param>
    /// <param name="personId">Person index id in the list of detected people</param>
    /// <returns>Returs true in frame that the requested state stoped.</returns>
    public static bool CursorHandStateEnd(HandState handState, int personId = 1)
    {
        return GetCursorHandState(personId) != HandState.Unknown && GetCursorHandState(personId) != HandState.NotTracked &&
            GetCursorHandStatePrevious(personId) == handState && GetCursorHandState(personId) != handState;
    }

    /// <summary>
    /// Get the status of a gesture for a specific player.
    /// </summary>
    /// <param name="gestureId">The Id of the gesture to detect.</param>
    /// <param name="personId">the person index of kinect (1-6).</param>
    /// <returns>Returns the status of requested gesture.</returns>
    public static bool GetGestureStatus(GestureId gestureId, int personId = 1)
    {
        int personIndex = personId - 1;
        if (!IsValidIndex(personIndex)) return false;

        return _people.ElementAt(personIndex).Value.GetGesture(gestureId);
    }

    /// <summary>
    /// Gets cursor hand state of current frame.
    /// </summary>
    /// <param name="personId">Person index id in the list of detected people</param>
    /// <returns></returns>
    public static HandState GetCursorHandState(int personId = 1)
    {
        int personIndex = personId - 1;
        if (!IsValidIndex(personIndex)) return HandState.NotTracked;

        Person person = _people.ElementAt(personIndex).Value;

        return person.HandLeftPosition.Z < person.HandRightPosition.Z
            ? _people.ElementAt(personIndex).Value.HandLeftState
            : _people.ElementAt(personIndex).Value.HandRightState;
    }

    /// <summary>
    /// Gets cursor hand state of previous frame.
    /// </summary>
    /// <param name="personId">Person index id in the list of detected people</param>
    /// <returns></returns>
    public static HandState GetCursorHandStatePrevious(int personId = 1)
    {
        int personIndex = personId - 1;
        if (!IsValidIndex(personIndex)) return HandState.NotTracked;

        Person person = _people.ElementAt(personIndex).Value;

        return person.HandLeftPosition.Z < person.HandRightPosition.Z
            ? _people.ElementAt(personIndex).Value.HandLeftStatePrevious
            : _people.ElementAt(personIndex).Value.HandRightStatePrevious;
    }

    /// <summary>
    /// Get the state of person's hand.
    /// </summary>
    /// <param name="hand">Person's hand to detect</param>
    /// <param name="personId">Person index id in the list of detected people</param>
    /// <returns></returns>
    public static HandState GetHandState(Person.Hand hand, int personId = 1)
    {
        int personIndex = personId - 1;
        if (!IsValidIndex(personIndex)) return HandState.NotTracked;

        return hand == Person.Hand.LEFT ?
            _people.ElementAt(personIndex).Value.HandLeftState :
            _people.ElementAt(personIndex).Value.HandRightState;
    }

    /// <summary>
    /// Get the state of person's hand on previous frame.
    /// </summary>
    /// <param name="hand">Person's hand to detect</param>
    /// <param name="personId">Person index id in the list of detected people</param>
    /// <returns></returns>
    public static HandState GetHandStatePrevious(Person.Hand hand, int personId = 1)
    {
        int personIndex = personId - 1;
        if (!IsValidIndex(personIndex)) return HandState.NotTracked;

        return hand == Person.Hand.LEFT ?
            _people.ElementAt(personIndex).Value.HandLeftStatePrevious :
            _people.ElementAt(personIndex).Value.HandRightStatePrevious;
    }

    /// <summary>
    /// Get the hand position in screen space.
    /// </summary>
    /// <param name="personId">Person index id in the list of detected people.</param>
    /// <returns></returns>
    public static Vector2 GetHandPointer(int personId = 1)
    {
        int personIndex = personId - 1;
        if (!IsValidIndex(personIndex)) return Vector2.zero;

        Person person = _people.ElementAt(personIndex).Value;

        //select hand and return result
        return person.HandLeftPosition.Z < person.HandRightPosition.Z ?
            //return left hand result
            person.HandLeftHeightFromSpine < 0.0f ? Vector2.zero :
                person.ScreenPositionLeftHand() :
            //return right hand result
            person.HandRightHeightFromSpine < 0.0f ? Vector2.zero :
                person.ScreenPositionRightHand();
    }

    /// <summary>
    /// Get the hand position in the world space.
    /// </summary>
    /// <param name="personId">Person index id in the list of detected people.</param>
    /// <returns></returns>
    public static Vector3 GetHandPointerWorldPos(int personId = 1)
    {
        //no need for `personIndex` its calculated in `GetHandPointer`
        Vector2 handScreenPosition = GetHandPointer(personId);
        return Camera.main.ScreenToWorldPoint(new Vector3(handScreenPosition.x, handScreenPosition.y, Camera.main.nearClipPlane * 10));
    }

    /// <summary>
    /// Checks if person's index is correct and starts recording hand's position.
    /// </summary>
    /// <param name="personId">Person index id in the list of detected people.</param>
    public static void StartRecordingHandPositions(int personId = 1)
    {
        int personIndex = personId - 1;
        if (!IsValidIndex(personIndex)) return;

        Person person = _people.ElementAt(personIndex).Value;
        person.StartRecordingHandPositions();
    }

    /// <summary>
    /// Checks if person's index is correct and stops recording hand's position.
    /// </summary>
    /// <param name="personId">Person index id in the list of detected people.</param>
    public static void StopRecordingHandPositions(int personId = 1)
    {
        int personIndex = personId - 1;
        if (!IsValidIndex(personIndex)) return;

        Person person = _people.ElementAt(personIndex).Value;
        person.StopRecordingHandPositions(personId);
    }


    /// <summary>
    /// Updates the People data. Removes old data, updates existing and adds new.
    /// </summary>
    /// <param name="bodies">Body array data.</param>
    private void UpdatePeople(Body[] bodies)
    {
        List<ulong> trackedIds = (from body in bodies
                                  where body != null
                                  select body.TrackingId).ToList();

        List<ulong> knownIds = new List<ulong>(_people.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (trackedIds.Contains(trackingId)) continue;

            _people[trackingId].StopRecordingHandPositions(_people.Values.ToList().IndexOf(_people[trackingId]) + 1);
            _people.Remove(trackingId);
            //Debug.Log("Removed: " + trackingId + "\nTotal people: " + _people.Count);
        }

        //Add new people or update existing people body values
        foreach (Body body in bodies)
        {
            if (body == null || body.TrackingId == 0) continue;

            if (!_people.ContainsKey(body.TrackingId))
            {
                //Don't add person if maximum people already reached
                if (_people.Count < MaximumPlayers)
                {
                    //Add new person data
                    _people.Add(body.TrackingId, new Person(body, PauseBetweenGestures, DebugMode, BypassForbidenGestures));
                    //Debug.Log("Added: " + body.TrackingId + "\nTotal people: " + _people.Count);
                }
            }
            else
            {
                //Update Person data
                //Debug.Log("Update: "+body.TrackingId);
                _people[body.TrackingId].UpdateBodyData(body);
            }
        }
    }

    /// <summary>
    /// Used from GestureManager Editor script to reload the settings of the GestureManager.
    /// </summary>
    public void ReloadSettings()
    {
        foreach (Person person in _people.Values)
        {
            person.ReloadSettings(DebugMode, BypassForbidenGestures, PauseBetweenGestures);
        }
    }

    /// <summary>
    /// Checks if the index of the person is valid each moment or in correct range.
    /// </summary>
    /// <param name="personIndex">Person's index in the _people list.</param>
    /// <param name="ignoreGotFrame">Ignore the status of current frame.</param>
    /// <returns></returns>
    private static bool IsValidIndex(int personIndex)
    {
        if (_people.Count == 0) return false;

        if (personIndex < 0 || personIndex > 6)
        {
            Debug.LogWarning("Wrong parameter for person number. Must be between 1-6");
            return false;
        }

        if (personIndex + 1 > _people.Count) return false;

        return true;
    }

    #endregion

    #region Quit Application Actions
    /// <summary>
    /// On application quit closes the reader and disposes the buffers.
    /// </summary>
    void OnApplicationQuit()
    {
        int i = 1;
        foreach (Person person in _people.Values)
        {
            person.StopRecordingHandPositions(i);
            i++;
        }

        if (_reader != null)
        {
            _reader.Dispose();
            _reader = null;
        }

        if (_sensor != null)
        {
            if (_sensor.IsOpen)
            {
                _sensor.Close();
            }

            _sensor = null;
        }
    }

    #endregion
}

#region Editor Extension

#if UNITY_EDITOR
[CustomEditor(typeof(GestureManager))]
public class GestureManagerEditor : Editor
{
    private GestureManager _thisScript;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        _thisScript = (GestureManager)target;

        EditorGUILayout.Space();

        if (EditorApplication.isPlaying && GUILayout.Button("Reload Settings"))
        {
            _thisScript.ReloadSettings();
        }
    }
}
#endif

#endregion
