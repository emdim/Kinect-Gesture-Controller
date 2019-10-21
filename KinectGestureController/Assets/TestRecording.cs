using System.Collections;
using System.Collections.Generic;
using GesturesInput;
using UnityEngine;

public class TestRecording : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    private bool _recording;
    // Update is called once per frame
    void Update()
    {
        if (!_recording && GestureManager.GetGestureStatus(GestureId.WaveRightHand))
        {
            Debug.Log("Start recording");
            GestureManager.StartRecordingHandPositions();
            _recording = true;
        }

        if (_recording && GestureManager.GetGestureStatus(GestureId.WaveLeftHand))
        {
            Debug.Log("Stop recording");
            GestureManager.StopRecordingHandPositions();
            _recording = false;
        }
    }
}
