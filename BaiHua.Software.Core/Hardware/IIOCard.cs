namespace BaiHua.Software.Core.Hardware
{
    public interface IIOCard
    {
        bool Init(string portName, string connectionParam); // Or generic connection params
        void Close();
        bool SetOutput(int index, bool value);
        bool GetOutput(int index, out bool value);
        bool WaitInput(int index, bool level, int timeoutMs);
        bool GetInput(int index, out bool value);

        bool SetAnalogOutput(int index, double value);
        bool GetAnalogOutput(int index, out double value);
        bool GetAnalogInput(int index, out double value);

        bool IsConnected { get; }
    }
}
