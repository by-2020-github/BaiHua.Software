namespace BaiHua.Software.Core.Application;

public interface IWorkstationAppService
{
    WorkstationData GetWorkstation();
    WorkstationData Start();
    WorkstationData Stop();
    WorkstationData Reset();
    WorkstationData ToggleMode();
}
