namespace BaiHua.Software.Core.Application;

public sealed class WorkstationData
{
    public string Title { get; set; } = "工站控制";
    public string CurrentRecipe { get; set; } = string.Empty;
    public string CurrentStatus { get; set; } = string.Empty;
    public bool IsAutoMode { get; set; }
    public double AxisPosition { get; set; }
    public double Temperature { get; set; }
    public double Pressure { get; set; }
    public IReadOnlyList<string> OperationLogs { get; set; } = Array.Empty<string>();
}
