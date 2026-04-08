using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using BaiHua.Software.Core.Application;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AppModules = BaiHua.Software.Modules;

namespace BaiHua.Software.UI;

/// <summary>
/// 主窗口 ViewModel - 管理主窗口的状态和命令
/// </summary>
public class MainWindowViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;
    private readonly AppModules.IModuleManager _moduleManager;
    private readonly AppModules.IModuleNavigationRegistry _moduleNavigationRegistry;
    private readonly IRuntimeStateService _runtimeStateService;
    private readonly List<AppModules.ModuleExport> _allModules = [];

    private string _title = "BaiHua.Software - 上位机通用框架";
    private ObservableCollection<AppModules.ModuleExport> _filteredModules;
    private AppModules.ModuleExport? _selectedModule;
    private string _filterText = string.Empty;
    private string _currentModuleTitle = "欢迎";
    private string _currentModuleDescription = "请选择底部模块开始操作。";
    private string _deviceStatusText = "设备状态加载中...";
    private string _workstationStatusText = "工位状态加载中...";
    private string _currentUserText = $"当前用户：{Environment.UserName}";
    private string _lastRefreshText = "更新时间：--";
    private ObservableCollection<ModuleNavigationGroup> _navigationGroups;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public ObservableCollection<AppModules.ModuleExport> FilteredModules
    {
        get => _filteredModules;
        set => SetProperty(ref _filteredModules, value);
    }

    public AppModules.ModuleExport? SelectedModule
    {
        get => _selectedModule;
        set
        {
            if (SetProperty(ref _selectedModule, value) && value is not null)
            {
                OpenModule(value);
            }
        }
    }

    public ObservableCollection<ModuleNavigationGroup> NavigationGroups
    {
        get => _navigationGroups;
        set => SetProperty(ref _navigationGroups, value);
    }

    public string FilterText
    {
        get => _filterText;
        set
        {
            if (SetProperty(ref _filterText, value))
            {
                ApplyFilter();
                ClearSearchCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string CurrentModuleTitle
    {
        get => _currentModuleTitle;
        set => SetProperty(ref _currentModuleTitle, value);
    }

    public string CurrentModuleDescription
    {
        get => _currentModuleDescription;
        set => SetProperty(ref _currentModuleDescription, value);
    }

    public string DeviceStatusText
    {
        get => _deviceStatusText;
        set => SetProperty(ref _deviceStatusText, value);
    }

    public string WorkstationStatusText
    {
        get => _workstationStatusText;
        set => SetProperty(ref _workstationStatusText, value);
    }

    public string CurrentUserText
    {
        get => _currentUserText;
        set => SetProperty(ref _currentUserText, value);
    }

    public string LastRefreshText
    {
        get => _lastRefreshText;
        set => SetProperty(ref _lastRefreshText, value);
    }

    public string ModuleCountText => $"模块：{FilteredModules.Count}/{_allModules.Count}";

    public DelegateCommand ExitApplicationCommand { get; }
    public DelegateCommand MinimizeWindowCommand { get; }
    public DelegateCommand ClearSearchCommand { get; }

    public MainWindowViewModel(
        IRegionManager regionManager,
        AppModules.IModuleManager moduleManager,
        AppModules.IModuleNavigationRegistry moduleNavigationRegistry,
        IRuntimeStateService runtimeStateService)
    {
        _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
        _moduleManager = moduleManager ?? throw new ArgumentNullException(nameof(moduleManager));
        _moduleNavigationRegistry = moduleNavigationRegistry ?? throw new ArgumentNullException(nameof(moduleNavigationRegistry));
        _runtimeStateService = runtimeStateService ?? throw new ArgumentNullException(nameof(runtimeStateService));

        _filteredModules = new ObservableCollection<AppModules.ModuleExport>();
        _navigationGroups = new ObservableCollection<ModuleNavigationGroup>();

        ExitApplicationCommand = new DelegateCommand(OnExitApplication);
        MinimizeWindowCommand = new DelegateCommand(OnMinimizeWindow);
        ClearSearchCommand = new DelegateCommand(OnClearSearch, CanClearSearch);

        LoadModules();
        RefreshShellStatus();
    }

    /// <summary>
    /// 加载所有模块
    /// </summary>
    private void LoadModules()
    {
        _allModules.Clear();
        _allModules.AddRange(_moduleManager.GetAllModules().Where(m => m.IsVisible));
        ApplyFilter();

        if (FilteredModules.Any())
        {
            SelectedModule = FilteredModules[0];
            return;
        }

        RefreshShellStatus();
    }

    private void ApplyFilter()
    {
        var keyword = FilterText?.Trim();
        var modules = string.IsNullOrWhiteSpace(keyword)
            ? _allModules
            : _allModules.Where(m => m.DisplayName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                || m.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                || m.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

        FilteredModules = new ObservableCollection<AppModules.ModuleExport>(modules);
        BuildNavigationGroups();
        RaisePropertyChanged(nameof(ModuleCountText));
    }

    private void BuildNavigationGroups()
    {
        var categoryDefinitions = _moduleNavigationRegistry.GetCategories();

        var groupedModules = FilteredModules
            .GroupBy(module => _moduleNavigationRegistry.GetCategoryOrDefault(module.CategoryKey).Key,
                StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.OrderBy(module => module.Order).ToList(), StringComparer.OrdinalIgnoreCase);

        NavigationGroups = new ObservableCollection<ModuleNavigationGroup>(
            categoryDefinitions.Select(definition => new ModuleNavigationGroup
            {
                Key = definition.Key,
                Title = definition.Title,
                Description = definition.Description,
                IconGlyph = definition.IconGlyph,
                Modules = new ObservableCollection<AppModules.ModuleExport>(
                    groupedModules.TryGetValue(definition.Key, out var modules)
                        ? modules
                        : [])
            }));
    }

    private void OpenModule(AppModules.ModuleExport module)
    {
        _moduleManager.LoadModule(module.Name);

        if (module.ViewType is not null)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
                _regionManager.RequestNavigate(module.Region, module.ViewType.Name));
        }

        CurrentModuleTitle = module.DisplayName;
        CurrentModuleDescription = module.Description;
        RefreshShellStatus();
    }

    private bool CanClearSearch() => !string.IsNullOrWhiteSpace(FilterText);

    private void OnClearSearch()
    {
        FilterText = string.Empty;
    }

    private void RefreshShellStatus()
    {
        var dashboard = _runtimeStateService.GetDashboardData();
        var workstation = _runtimeStateService.GetWorkstationData();

        DeviceStatusText = $"设备状态：在线 {dashboard.OnlineDeviceCount} 台 / 离线 {dashboard.OfflineDeviceCount} 台";
        WorkstationStatusText = $"工位状态：{workstation.CurrentStatus} · {(workstation.IsAutoMode ? "自动" : "手动")}模式";
        CurrentUserText = $"当前用户：{Environment.UserName}";
        LastRefreshText = $"更新时间：{dashboard.LastRefreshTime:yyyy-MM-dd HH:mm:ss}";
    }

    /// <summary>
    /// 退出应用
    /// </summary>
    private void OnMinimizeWindow()
    {
        if (Application.Current.MainWindow is not null)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }

    private void OnExitApplication()
    {
        Application.Current.Shutdown();
    }
}

public sealed class ModuleNavigationGroup
{
    public string Key { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string IconGlyph { get; set; } = string.Empty;

    public ObservableCollection<AppModules.ModuleExport> Modules { get; set; } = [];

    public bool HasModules => Modules.Count > 0;
}
