using BaiHua.Software.Core.Hardware;
using OpenCvSharp;

namespace BaiHua.Software.HAL.Simulation
{
    public class SimCamera : ICamera
    {
        private bool _isConnected;
        private bool _isGrabbing;
        private TriggerMode _triggerMode = TriggerMode.Off;
        private TriggerSource _triggerSource = TriggerSource.Software;
        private double _exposure = 10000;
        private double _gain = 0;
        private Mat? _lastImage;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _grabTask;
        private AutoResetEvent _triggerEvent = new AutoResetEvent(false);

        public event EventHandler<CameraImageEventArgs>? ImageGrabbed;

        public bool IsConnected => _isConnected;
        public bool IsGrabbing => _isGrabbing;

        public bool Open()
        {
            _isConnected = true;
            return true;
        }

        public void Close()
        {
            StopGrabbing();
            _isConnected = false;
        }

        public bool StartGrabbing()
        {
            if (!_isConnected) return false;
            if (_isGrabbing) return true;

            _isGrabbing = true;
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            _grabTask = Task.Run(() => GrabLoop(token), token);
            return true;
        }

        public bool StopGrabbing()
        {
            if (!_isGrabbing) return true;

            _cancellationTokenSource?.Cancel();
            try
            {
                _grabTask?.Wait(2000); // Wait for task to finish
            }
            catch {}

            _isGrabbing = false;
            return true;
        }

        private void GrabLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                bool shouldGrab = false;

                if (_triggerMode == TriggerMode.Off)
                {
                    // Continuous
                    shouldGrab = true;
                    Thread.Sleep(33); // ~30 fps simulation
                }
                else
                {
                    // Trigger Mode
                    if (_triggerSource == TriggerSource.Software)
                    {
                        // Wait for signal
                        if (_triggerEvent.WaitOne(100)) // Poll with timeout to check cancellation
                        {
                            shouldGrab = true;
                        }
                    }
                    else
                    {
                        // Hardware trigger simulation (random or manual via debug?)
                        // For validation, we just wait.
                        Thread.Sleep(100); 
                    }
                }

                if (shouldGrab)
                {
                    GenerateAndFireImage();
                }
            }
        }

        private void GenerateAndFireImage()
        {
             // Create a dummy image
            var img = new Mat(480, 640, MatType.CV_8UC3, new Scalar(0, 0, 0));
            // Draw something
            Cv2.PutText(img, $"Sim: {DateTime.Now:HH:mm:ss.fff}", new OpenCvSharp.Point(50, 50), 
                HersheyFonts.HersheySimplex, 1.0, new Scalar(0, 255, 0), 2);

            // Fire event
            ImageGrabbed?.Invoke(this, new CameraImageEventArgs(img));

            _lastImage?.Dispose();
            _lastImage = img; // Keep reference if needed for other purposes, but event args has it
        }

        public bool ExecuteSoftwareTrigger()
        {
            if (!_isConnected || !_isGrabbing) return false;
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

            // Generate one immediately
            var img = new Mat(480, 640, MatType.CV_8UC3, new Scalar(50, 50, 50));
            Cv2.PutText(img, "GrabOne", new OpenCvSharp.Point(50, 100), HersheyFonts.HersheySimplex, 1.0, new Scalar(0, 0, 255));
            image = img;
            return true;
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
        }

        public double GetExposureTime()
        {
            return _exposure;
        }

        public void SetGain(double value)
        {
             _gain = value;
        }

        public double GetGain()
        {
            return _gain;
        }

        public void Dispose()
        {
            StopGrabbing();
            if (_lastImage != null && !_lastImage.IsDisposed)
            {
                _lastImage.Dispose();
                _lastImage = null;
            }
            _triggerEvent.Dispose();
        }

    }
}
