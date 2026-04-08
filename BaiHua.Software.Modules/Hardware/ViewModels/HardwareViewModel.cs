using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace BaiHua.Software.Modules.Hardware.ViewModels;

public class HardwareViewModel : BindableBase
{
    public string Title { get; } = "硬件接入";

    public string Summary { get; } = "统一管理现场设备连接状态与驱动扩展入口。";

    public int OnlineEndpointCount => HardwareEndpoints.Count(endpoint => endpoint.Status == "已连接");

    public int PendingEndpointCount => HardwareEndpoints.Count(endpoint => endpoint.Status == "待接入");

    public string IntegrationSummary => $"已连接 {OnlineEndpointCount} 个设备，待接入 {PendingEndpointCount} 个设备。";

    public ObservableCollection<string> NextSteps { get; } =
    [
        "补充真实驱动适配层并绑定连接状态。",
        "增加设备参数配置、手动测试和告警联动。",
        "接入硬件诊断日志与通信质量统计。"
    ];

    public ObservableCollection<HardwareEndpointItem> HardwareEndpoints { get; } =
    [
        new() { Name = "PLC 控制器", Category = "控制", Driver = "FluentModbus", Address = "192.168.0.10:502", Status = "已连接", Description = "负责产线工艺联锁、配方下发与状态采集。" },
        new() { Name = "视觉相机", Category = "视觉", Driver = "GigE Vision", Address = "Camera-01", Status = "待接入", Description = "用于定位、检测和图像采集。" },
        new() { Name = "运动控制", Category = "运控", Driver = "EtherCAT", Address = "AxisGroup-A", Status = "待接入", Description = "管理多轴插补、点位与安全联动。" },
        new() { Name = "IO 模块", Category = "IO", Driver = "Remote IO", Address = "Station-02", Status = "已连接", Description = "采集按钮、气缸、传感器等离散量信号。" }
    ];
}

public sealed class HardwareEndpointItem
{
    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Driver { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
