using System;
using UnityEngine;
using Windows.Kinect;
using GesturesInput;

public class BodySourceManager : MonoBehaviour 
{
    private KinectSensor _sensor;
    private BodyFrameReader _reader;
    private Body[] _data;

    public Body[] GetData()
    {
        return _data;
    }
    

    void Start () 
    {
        _sensor = KinectSensor.GetDefault();

        if (_sensor != null)
        {
            _reader = _sensor.BodyFrameSource.OpenReader();
            
            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }

            _reader.FrameArrived += FrameArrived;
        }
    }

    private void FrameArrived(object sender, BodyFrameArrivedEventArgs e)
    {
        if (_reader != null)
        {
            var frame = _reader.AcquireLatestFrame();
            if (frame != null)
            {
                if (_data == null)
                {
                    _data = new Body[_sensor.BodyFrameSource.BodyCount];
                }

                frame.GetAndRefreshBodyData(_data);

                frame.Dispose();
                frame = null;
            }
        }
    }

    void Update()
    {
        //if(GestureManager.GetGesture(GestureId.WaveRight))
        //{
        //    Debug.Log("Someone waved!");
        //}
    }

    void OnApplicationQuit()
    {
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
}
