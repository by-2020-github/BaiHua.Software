using BaiHua.Software.Core.Hardware;
using System.Collections.Concurrent;

namespace BaiHua.Software.HAL.Simulation
{
    public class SimIO : IIOCard
    {
        private bool _isConnected;
        private readonly ConcurrentDictionary<int, bool> _outputs = new();
        private readonly ConcurrentDictionary<int, bool> _inputs = new();
        private readonly ConcurrentDictionary<int, double> _analogOutputs = new();
        private readonly ConcurrentDictionary<int, double> _analogInputs = new();

        public bool IsConnected => _isConnected;

        public bool Init(string portName, string connectionParam)
        {
            _isConnected = true;
            return true;
        }

        public void Close()  
        {
            _isConnected = false;
        }

        public bool SetOutput(int index, bool value)
        {
            if (!_isConnected) return false;
            _outputs[index] = value;
            return true;
        }

        public bool GetOutput(int index, out bool value)
        {
            if (!_isConnected) 
            {
                value = false;
                return false;
            }
            value = _outputs.GetValueOrDefault(index, false);
            return true;
        }

        public bool WaitInput(int index, bool level, int timeoutMs)
        {
            return GetInput(index, out bool current) && current == level;
        }
        
        public bool GetInput(int index, out bool value)
        {
            if (!_isConnected)
            {
                value = false;
                 return false;
            }
            value = _inputs.GetValueOrDefault(index, false); // Default false if not set
            return true;
        }

        public bool SetAnalogOutput(int index, double value)
        {
            if (!_isConnected) return false;
            _analogOutputs[index] = value;
            return true;
        }

        public bool GetAnalogOutput(int index, out double value)
        {
            if (!_isConnected)
            {
                value = 0;
                return false;
            }
            value = _analogOutputs.GetValueOrDefault(index, 0);
            return true;
        }

        public bool GetAnalogInput(int index, out double value)
        {
            if (!_isConnected)
            {
                value = 0;
                return false;
            }
            value = _analogInputs.GetValueOrDefault(index, 0);
            return true;
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        public bool Init(string connectionParam)
        {
            throw new NotImplementedException();
        }
    }
}
