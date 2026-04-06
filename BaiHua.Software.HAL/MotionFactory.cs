using BaiHua.Software.Core.Hardware;
using BaiHua.Software.HAL.Simulation;

namespace BaiHua.Software.HAL
{
    public class MotionFactory
    {
        public enum MotionType
        {
            Simulation,
            Leisai, // Example: LeiSai Motion Card
            Googol, // Example: Googol Motion Card
            Inovance
        }

        public static IMotion Create(MotionType type)
        {
            switch (type)
            {
                case MotionType.Simulation:
                    return new SimMotion();
                // Add real implementations here
                default:
                    throw new ArgumentException($"Motion type {type} not supported.");
            }
        }
        
        public static IMotion CreateAndInit(MotionType type, string connectionParam)
        {
            var motion = Create(type);
            motion.Init(connectionParam);
            return motion;
        }
    }
}
