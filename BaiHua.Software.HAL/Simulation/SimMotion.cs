using BaiHua.Software.Core.Hardware;
using System.Collections.Concurrent;

namespace BaiHua.Software.HAL.Simulation
{
    public class SimMotion : IMotion
    {
        private bool _isConnected;
        private readonly ConcurrentDictionary<int, double> _positions = new();
        private readonly ConcurrentDictionary<int, bool> _enabled = new();
        private readonly ConcurrentDictionary<int, AxisParam> _params = new();
        private readonly ConcurrentDictionary<int, AxisStatus> _statuses = new();

        public bool IsConnected => _isConnected;

        public bool Init(string connectionParam)
        {
            _isConnected = true;
            return true;
        }

        public void Close()
        {
            _isConnected = false;
        }

        public void Dispose()
        {
            Close();
        }

        public bool EnableAxis(int axisId)
        {
            _enabled[axisId] = true;
            _statuses[axisId] = AxisStatus.Ready;
            return true;
        }

        public bool DisableAxis(int axisId)
        {
            _enabled[axisId] = false;
            _statuses[axisId] = AxisStatus.Disabled;
            return true;
        }

        public bool Home(int axisId)
        {
            if (!_isConnected) return false;
            _positions[axisId] = 0;
            _statuses[axisId] = AxisStatus.Ready;
            return true;
        }

        public bool MoveAbs(int axisId, double position)
        {
            return MoveAbs(axisId, position, GetAxisParam(axisId).Velocity);
        }

        public bool MoveAbs(int axisId, double position, double velocity)
        {
            if (!_isConnected || !IsAxisEnabled(axisId)) return false;
            _statuses[axisId] = AxisStatus.Moving;
            _positions[axisId] = position;
            // Simulate completion
            Task.Delay(100).ContinueWith(t => _statuses[axisId] = AxisStatus.Ready);
            return true;
        }

        public bool MoveRel(int axisId, double distance)
        {
            return MoveRel(axisId, distance, GetAxisParam(axisId).Velocity);
        }

        public bool MoveRel(int axisId, double distance, double velocity)
        {
            if (!_isConnected || !IsAxisEnabled(axisId)) return false;
            double current = _positions.GetOrAdd(axisId, 0);
            _positions[axisId] = current + distance;
            _statuses[axisId] = AxisStatus.Moving;
            Task.Delay(100).ContinueWith(t => _statuses[axisId] = AxisStatus.Ready);
            return true;
        }

        public bool MoveLine(int[] axes, double[] positions, double velocity)
        {
            if (!_isConnected || axes.Length != positions.Length) return false;

            foreach (var axis in axes)
            {
                _statuses[axis] = AxisStatus.Moving;
            }

            for (int i = 0; i < axes.Length; i++)
            {
                _positions[axes[i]] = positions[i];
            }

            Task.Delay(100).ContinueWith(t => 
            {
                foreach (var axis in axes)
                {
                    _statuses[axis] = AxisStatus.Ready;
                }
            });
            return true;
        }

        public bool Stop(int axisId)
        {
            _statuses[axisId] = AxisStatus.Ready;
            return true;
        }

        public bool EmgStop(int axisId)
        {
            _statuses[axisId] = AxisStatus.Error;
            return true;
        }

        public bool StopAll()
        {
            foreach (var key in _statuses.Keys)
            {
                _statuses[key] = AxisStatus.Ready;
            }
            return true;
        }

        public double GetPosition(int axisId)
        {
            return _positions.GetOrAdd(axisId, 0);
        }

        public AxisStatus GetStatus(int axisId)
        {
            return _statuses.GetOrAdd(axisId, AxisStatus.Unknown);
        }

        public bool GetAxisIo(int axisId, int ioIndex)
        {
             return false;
        }

        public bool ClearAlarm(int axisId)
        {
            if (_statuses.TryGetValue(axisId, out var status) && status == AxisStatus.Error)
            {
                 _statuses[axisId] = AxisStatus.Ready;
                 return true;
            }
            return false;
        }

        public void SetAxisParam(int axisId, AxisParam param)
        {
            _params[axisId] = param;
        }

        public AxisParam GetAxisParam(int axisId)
        {
             return _params.GetOrAdd(axisId, new AxisParam());
        }

        private bool IsAxisEnabled(int axisId)
        {
            return _enabled.GetOrAdd(axisId, false);
        }
    }
}
