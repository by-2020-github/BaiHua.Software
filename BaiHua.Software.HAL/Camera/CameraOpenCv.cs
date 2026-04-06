using BaiHua.Software.Core.Hardware;
using OpenCvSharp;
using System.Threading;
using System.Threading.Tasks;

namespace BaiHua.Software.HAL.Camera
{
    public class CameraOpenCv : ICamera
    {
        private VideoCapture? _capture;
        private bool _isConnected;
        private bool _isGrabbing;
        private int _deviceId;

        private TriggerMode _triggerMode = TriggerMode.Off;
        private TriggerSource _triggerSource = TriggerSource.Software;
        private double _exposure = -10; // Default auto or unknown
        private double _gain = -1;

        private CancellationTokenSource? _cts;
        private Task? _grabTask;
        private AutoResetEvent _triggerEvent = new AutoResetEvent(false);
        private readonly object _lock = new object();

        public event EventHandler<CameraImageEventArgs>? ImageGrabbed;

        public bool IsConnected => _isConnected;
        public bool IsGrabbing => _isGrabbing;

        public CameraOpenCv(int deviceId = 0)
        {
            _deviceId = deviceId;
        }

        public bool Open()
        {
            if (_isConnected) return true;
            try
            {
                lock (_lock)
                {
                    _capture = new VideoCapture(_deviceId);
                    if (_capture.IsOpened())
                    {
                        _isConnected = true;
                        return true;
                    }
                }
            }
            catch
            {
                // Log error
            }
            return false;
        }

        public void Close()
        {
            StopGrabbing();
            lock (_lock)
            {
                if (_capture != null)
                {
                    _capture.Release();
                    _capture.Dispose();
                    _capture = null;
                }
            }
            _isConnected = false;
        }

        public bool StartGrabbing()
        {
            if (!_isConnected || _capture == null) return false;
            if (_isGrabbing) return true;

            _isGrabbing = true;
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            _grabTask = Task.Run(() => GrabLoop(token), token);
            return true;
        }

        public bool StopGrabbing()
        {
            if (!_isGrabbing) return true;

            _cts?.Cancel();
            try
            {
                _grabTask?.Wait(2000);
            }
            catch { }

            _isGrabbing = false;
            return true;
        }

        private void GrabLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                bool trigger = false;
                if (_triggerMode == TriggerMode.Off)
                {
                    trigger = true;
                }
                else
                {
                    if (_triggerSource == TriggerSource.Software)
                    {
                        if (_triggerEvent.WaitOne(10)) trigger = true;
                    }
                    else
                    {
                        // Hardware trigger simulation (not supported by standard webcam)
                        Thread.Sleep(10);
                    }
                }

                if (trigger)
                {
                    try
                    {
                        Mat? frame = null;
                        lock (_lock)
                        {
                            if (_capture != null && _capture.IsOpened())
                            {
                                var temp = new Mat();
                                if (_capture.Read(temp) && !temp.Empty())
                                {
                                    frame = temp;
                                }
                                else
                                {
                                    temp.Dispose();
                                }
                            }
                        }

                        if (frame != null)
                        {
                            ImageGrabbed?.Invoke(this, new CameraImageEventArgs(frame));
                            // Ensure the frame is managed by receiver or GC. 
                            // Current implementation: receiver gets a Mat.
                        }
                    }
                    catch { }
                }
                else
                {
                    if (_triggerMode == TriggerMode.Off) Thread.Sleep(10);
                }
            }
        }

        public bool ExecuteSoftwareTrigger()
        {
            if (!_isGrabbing) return false;
            if (_triggerMode == TriggerMode.On && _triggerSource == TriggerSource.Software)
            {
                _triggerEvent.Set();
                return true;
            }
            return false;
        }

        public bool GrabOne(out object image, int timeoutMs = 2000)
        {
            image = null!;
            if (!_isConnected) return false;

            lock (_lock)
            {
                if (_capture != null && _capture.IsOpened())
                {
                    var frame = new Mat();
                    if (_capture.Read(frame) && !frame.Empty())
                    {
                        image = frame;
                        return true;
                    }
                    frame.Dispose();
                }
            }
            return false;
        }

        public void SetTriggerMode(TriggerMode mode)
        {
            _triggerMode = mode;
        }

        public TriggerMode GetTriggerMode()
        {
            return _triggerMode;
        }

        public void SetTriggerSource(TriggerSource source)
        {
            _triggerSource = source;
        }

        public TriggerSource GetTriggerSource()
        {
            return _triggerSource;
        }

        public void SetExposureTime(double value)
        {
            _exposure = value;
            if (_isConnected && _capture != null)
            {
                _capture.Set(VideoCaptureProperties.Exposure, value);
            }
        }

        public double GetExposureTime()
        {
            if (_isConnected && _capture != null)
            {
                return _capture.Get(VideoCaptureProperties.Exposure);
            }
            return _exposure;
        }

        public void SetGain(double value)
        {
            _gain = value;
            if (_isConnected && _capture != null)
            {
                _capture.Set(VideoCaptureProperties.Gain, value);
            }
        }

        public double GetGain()
        {
            if (_isConnected && _capture != null)
            {
                return _capture.Get(VideoCaptureProperties.Gain);
            }
            return _gain;
        }

        public void Dispose()
        {
            Close();
            _triggerEvent.Dispose();
        }
    }
}
