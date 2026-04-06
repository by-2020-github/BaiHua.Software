using BaiHua.Software.Core.Hardware;
using BaiHua.Software.HAL.Camera;
using BaiHua.Software.HAL.Simulation;

namespace BaiHua.Software.HAL
{
    public class HALContext
    {
        public enum HardwareMode
        {
            Real,
            Simulation
        }

        public ICamera Camera { get; private set; }
        public IMotion Motion { get; private set; }
        public IIOCard IOCard { get; private set; }

        public HALContext(HardwareMode mode)
        {
            if (mode == HardwareMode.Simulation)
            {
                Camera = new SimCamera();
                Motion = new SimMotion();
                IOCard = new SimIO();
            }
            else
            {
                Camera = new CameraOpenCv(0);
                Motion = new SimMotion(); // Fallback to Sim until real implemented
                IOCard = new SimIO();
            }
        }
        
        public void Initialize()
        {
             Camera.Open();
             Motion.Init("COM1,9600"); 
             IOCard.Init("192.168.1.5", "502");
        }

        public void Shutdown()
        {
             Camera.Close();
             Motion.Close();
             IOCard.Close();
        }
    }
}
