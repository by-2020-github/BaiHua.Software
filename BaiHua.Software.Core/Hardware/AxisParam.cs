namespace BaiHua.Software.Core.Hardware
{
    public class AxisParam
    {
        /// <summary>
        /// 轴名称
        /// </summary>
        public string AxisName { get; set; } = "Axis";

        /// <summary>
        /// 脉冲当量 (Pulse/Unit)
        /// </summary>
        public double PulseEquivalent { get; set; } = 1000.0;

        /// <summary>
        /// 起始速度
        /// </summary>
        public double StartVelocity { get; set; } = 0;

        /// <summary>
        /// 运行速度
        /// </summary>
        public double Velocity { get; set; } = 100.0;

        /// <summary>
        /// 加速度
        /// </summary>
        public double Acceleration { get; set; } = 500.0;

        /// <summary>
        /// 减速度
        /// </summary>
        public double Deceleration { get; set; } = 500.0;

        /// <summary>
        /// 回零速度 (高速)
        /// </summary>
        public double HomeVelocityHigh { get; set; } = 50.0;

        /// <summary>
        /// 回零速度 (低速)
        /// </summary>
        public double HomeVelocityLow { get; set; } = 5.0;

        /// <summary>
        /// 回零模式 (0:原点, 1:极限+原点, 2:...)
        /// </summary>
        public int HomeMode { get; set; } = 0;

        /// <summary>
        /// 回零方向 (1:正向, -1:负向)
        /// </summary>
        public int HomeDir { get; set; } = 1;

        /// <summary>
        /// 软限位正
        /// </summary>
        public double SoftLimitPositive { get; set; } = 10000.0;

        /// <summary>
        /// 软限位负
        /// </summary>
        public double SoftLimitNegative { get; set; } = -10000.0;

        /// <summary>
        /// 是否启用软限位
        /// </summary>
        public bool EnableSoftLimit { get; set; } = false;
    }
}
