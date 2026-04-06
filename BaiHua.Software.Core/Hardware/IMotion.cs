namespace BaiHua.Software.Core.Hardware
{

    /// <summary>
    /// 轴运动状态
    /// </summary>
    public enum AxisStatus
    {
        Unknown,
        Disabled,
        Ready,
        Moving,
        Homing,
        Stopping,
        Error
    }

    public interface IMotion : IDisposable
    {
        bool IsConnected { get; }

        /// <summary>
        /// 初始化卡 (参数可以是串口号，IP地址，或者卡号等)
        /// </summary>
        bool Init(string connectionParam);

        /// <summary>
        /// 关闭卡
        /// </summary>
        void Close();


        /// <summary>
        /// 使能轴
        /// </summary>
        bool EnableAxis(int axisId);

        /// <summary>
        /// 失能轴
        /// </summary>
        bool DisableAxis(int axisId);

        /// <summary>
        /// 设置轴参数
        /// </summary>
        void SetAxisParam(int axisId, AxisParam param);

        /// <summary>
        /// 获取轴参数
        /// </summary>
        AxisParam GetAxisParam(int axisId);

        /// <summary>
        /// 回零
        /// </summary>
        bool Home(int axisId);

        /// <summary>
        /// 绝对运动 (使用预设参数)
        /// </summary>
        bool MoveAbs(int axisId, double position);

        /// <summary>
        /// 绝对运动 (指定速度)
        /// </summary>
        bool MoveAbs(int axisId, double position, double velocity);

        /// <summary>
        /// 相对运动 (使用预设参数)
        /// </summary>
        bool MoveRel(int axisId, double distance);

        /// <summary>
        /// 相对运动 (指定速度)
        /// </summary>
        bool MoveRel(int axisId, double distance, double velocity);

        /// <summary>
        /// 多轴线性插补
        /// </summary>
        /// <param name="axes">轴数组</param>
        /// <param name="positions">目标位置数组 (与轴一一对应)</param>
        /// <param name="velocity">插补速度</param>
        /// <returns></returns>
        bool MoveLine(int[] axes, double[] positions, double velocity);

        /// <summary>
        /// 停止单轴
        /// </summary>
        bool Stop(int axisId);

        /// <summary>
        /// 急停单轴
        /// </summary>
        bool EmgStop(int axisId);

        /// <summary>
        /// 停止所有轴
        /// </summary>
        bool StopAll();

        /// <summary>
        /// 获取轴位置
        /// </summary>
        double GetPosition(int axisId);

        /// <summary>
        /// 获取轴状态
        /// </summary>
        AxisStatus GetStatus(int axisId);

        /// <summary>
        /// 获取轴IO信号 (0: 正限位, 1: 负限位, 2: 原点, 3: 报警)
        /// </summary>
        bool GetAxisIo(int axisId, int ioIndex);

        /// <summary>
        /// 清除报警
        /// </summary>
        bool ClearAlarm(int axisId);
    }

}
