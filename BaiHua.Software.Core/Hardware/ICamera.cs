using System;

namespace BaiHua.Software.Core.Hardware
{
    public enum TriggerMode
    {
        Off,      // 连续采集
        On        // 触发采集
    }

    public enum TriggerSource
    {
        Software, // 软触发
        Line0,    // 硬触发 Line0
        Line1,    // 硬触发 Line1
        Line2,    // 硬触发 Line2
        Line3     // 硬触发 Line3
    }

    public class CameraImageEventArgs : EventArgs
    {
        public object Image { get; }
        public DateTime Timestamp { get; }

        public CameraImageEventArgs(object image)
        {
            Image = image;
            Timestamp = DateTime.Now;
        }
    }

    public interface ICamera : IDisposable
    {
        bool IsConnected { get; }
        bool IsGrabbing { get; }

        bool Open();
        void Close();

        /// <summary>
        /// 开始采集
        /// </summary>
        bool StartGrabbing();

        /// <summary>
        /// 停止采集
        /// </summary>
        bool StopGrabbing();

        /// <summary>
        /// 执行软触发
        /// </summary>
        bool ExecuteSoftwareTrigger();

        /// <summary>
        /// 单帧获取（同步）
        /// </summary>
        bool GrabOne(out object image, int timeoutMs = 2000);

        // --- 参数设置 ---

        void SetTriggerMode(TriggerMode mode);
        TriggerMode GetTriggerMode();

        void SetTriggerSource(TriggerSource source);
        TriggerSource GetTriggerSource();

        void SetExposureTime(double value);
        double GetExposureTime();

        void SetGain(double value);
        double GetGain();

        /// <summary>
        /// 图像采集事件
        /// </summary>
        event EventHandler<CameraImageEventArgs>? ImageGrabbed;
    }
}
