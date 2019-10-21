using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class MultiSourceManager : MonoBehaviour {
    public int ColorWidth { get; private set; }
    public int ColorHeight { get; private set; }
    
    private KinectSensor _sensor;
    private MultiSourceFrameReader _reader;
    private Texture2D _colorTexture;
    private ushort[] _depthData;
    private byte[] _colorData;

    public Texture2D GetColorTexture()
    {
        return _colorTexture;
    }
    
    public ushort[] GetDepthData()
    {
        return _depthData;
    }

    void Start () 
    {
        _sensor = KinectSensor.GetDefault();
        
        if (_sensor != null) 
        {
            _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth);
            
            var colorFrameDesc = _sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            ColorWidth = colorFrameDesc.Width;
            ColorHeight = colorFrameDesc.Height;
            
            _colorTexture = new Texture2D(colorFrameDesc.Width, colorFrameDesc.Height, TextureFormat.RGBA32, false);
            _colorData = new byte[colorFrameDesc.BytesPerPixel * colorFrameDesc.LengthInPixels];
            
            var depthFrameDesc = _sensor.DepthFrameSource.FrameDescription;
            _depthData = new ushort[depthFrameDesc.LengthInPixels];
            
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
                var colorFrame = frame.ColorFrameReference.AcquireFrame();
                if (colorFrame != null)
                {
                    var depthFrame = frame.DepthFrameReference.AcquireFrame();
                    if (depthFrame != null)
                    {
                        colorFrame.CopyConvertedFrameDataToArray(_colorData, ColorImageFormat.Rgba);
                        _colorTexture.LoadRawTextureData(_colorData);
                        _colorTexture.Apply();
                        
                        depthFrame.CopyFrameDataToArray(_depthData);
                        
                        depthFrame.Dispose();
                        depthFrame = null;
                    }
                
                    colorFrame.Dispose();
                    colorFrame = null;
                }
                
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
