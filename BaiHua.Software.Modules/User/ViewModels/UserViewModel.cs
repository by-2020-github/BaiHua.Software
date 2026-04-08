using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace BaiHua.Software.Modules.User.ViewModels;

public class UserViewModel : BindableBase
{
    public string Title { get; } = "用户中心";

    public string Summary { get; } = "统一承载登录认证、角色授权与审计功能。";

    public string CurrentUser { get; } = Environment.UserName;

    public string CurrentRole { get; } = "Administrator";

    public string CurrentShift { get; } = DateTime.Now.Hour < 18 ? "白班" : "夜班";

    public string LastLoginTime { get; } = DateTime.Now.AddMinutes(-18).ToString("yyyy-MM-dd HH:mm:ss");

    public string SecuritySummary { get; } = "当前模块用于承载认证、权限矩阵和审计入口，可继续接入真实账户体系。";

    public ObservableCollection<string> PermissionGroups { get; } =
    [
        "设备操作权限",
        "配方与参数修改权限",
        "硬件调试权限",
        "报表与追溯查询权限"
    ];

    public ObservableCollection<string> AuditItems { get; } =
    [
        "最近登录：本机账户已进入系统。",
        "最近授权变更：暂无未审核变更。",
        "最近审计检查：日志功能已启用。"
    ];

    public ObservableCollection<UserFeatureItem> FeatureItems { get; } =
    [
        new() { Title = "登录 / 注销入口", Description = "统一接入本地账户、AD 或第三方认证源。", Stage = "规划中" },
        new() { Title = "角色与权限矩阵配置", Description = "按模块、功能点、按钮级别进行授权控制。", Stage = "规划中" },
        new() { Title = "操作审计与登录日志查询", Description = "记录关键动作、登录来源和审批记录。", Stage = "规划中" },
        new() { Title = "用户组与工艺权限映射", Description = "将工艺段、设备、产品线权限映射到角色。", Stage = "规划中" }
    ];
}

public sealed class UserFeatureItem
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Stage { get; set; } = string.Empty;
}
