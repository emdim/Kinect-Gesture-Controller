using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class ColorSourceManager : MonoBehaviour 
{
    public int ColorWidth { get; private set; }
    public int ColorHeight { get; private set; }

    private KinectSensor _sensor;
    private ColorFrameReader _reader;
    private Texture2D _texture;
    private byte[] _data;
    
    public Texture2D GetColorTexture()
    {
        return _texture;
    }
    
    void Start()
    {
        _sensor = KinectSensor.GetDefault();
        
        if (_sensor != null) 
        {
            _reader = _sensor.ColorFrameSource.OpenReader();
            
            var frameDesc = _sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            ColorWidth = frameDesc.Width;
            ColorHeight = frameDesc.Height;
            
            _texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);
            _data = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];
            
            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }
        }
    }
    
    void Update () 
    {
        if (_reader != null) 
        {
            var frame = _reader.AcquireLatestFrame();
            
            if (frame != null)
            {
                frame.CopyConvertedFrameDataToArray(_data, ColorImageFormat.Rgba);
                _texture.LoadRawTextureData(_data);
                _texture.Apply();
                
                frame.Dispose();
                frame = null;
            }
        }
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
