using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
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
    private readonly List<AppModules.ModuleExport> _allModules = [];
    private readonly List<string> _navigationHistory = [];

    private string _title = "BaiHua.Software - 上位机通用框架";
    private ObservableCollection<AppModules.ModuleExport> _filteredModules;
    private AppModules.ModuleExport? _selectedModule;
    private string _filterText = string.Empty;
    private string _currentModuleTitle = "欢迎";
    private string _currentModuleDescription = "请选择左侧模块开始操作。";
    private int _navigationHistoryIndex = -1;
    private bool _recordSelectionInHistory = true;

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
                OpenModule(value, _recordSelectionInHistory);
            }
        }
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

    public string NavigationStatus => _navigationHistoryIndex >= 0
        ? $"导航记录：{_navigationHistoryIndex + 1}/{_navigationHistory.Count}"
        : "导航记录：0/0";

    public string ModuleCountText => $"模块：{FilteredModules.Count}/{_allModules.Count}";

    public DelegateCommand ExitApplicationCommand { get; }
    public DelegateCommand NavigateHomeCommand { get; }
    public DelegateCommand NavigatePreviousCommand { get; }
    public DelegateCommand NavigateNextCommand { get; }
    public DelegateCommand ClearSearchCommand { get; }

    public MainWindowViewModel(IRegionManager regionManager, AppModules.IModuleManager moduleManager)
    {
        _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
        _moduleManager = moduleManager ?? throw new ArgumentNullException(nameof(moduleManager));

        _filteredModules = new ObservableCollection<AppModules.ModuleExport>();

        ExitApplicationCommand = new DelegateCommand(OnExitApplication);
        NavigateHomeCommand = new DelegateCommand(OnNavigateHome, CanNavigateHome);
        NavigatePreviousCommand = new DelegateCommand(OnNavigatePrevious, CanNavigatePrevious);
        NavigateNextCommand = new DelegateCommand(OnNavigateNext, CanNavigateNext);
        ClearSearchCommand = new DelegateCommand(OnClearSearch, CanClearSearch);

        LoadModules();
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
        }
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
        RaisePropertyChanged(nameof(ModuleCountText));
        UpdateNavigationCommands();
    }

    private void OpenModule(AppModules.ModuleExport module, bool addToHistory)
    {
        _moduleManager.LoadModule(module.Name);

        if (module.ViewType is not null)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
                _regionManager.RequestNavigate(module.Region, module.ViewType.Name));
        }

        CurrentModuleTitle = module.DisplayName;
        CurrentModuleDescription = module.Description;

        if (addToHistory)
        {
            RecordHistory(module.Name);
        }

        RaisePropertyChanged(nameof(NavigationStatus));
        UpdateNavigationCommands();
    }

    private void RecordHistory(string moduleName)
    {
        if (_navigationHistoryIndex >= 0
            && _navigationHistoryIndex < _navigationHistory.Count
            && _navigationHistory[_navigationHistoryIndex] == moduleName)
        {
            return;
        }

        if (_navigationHistoryIndex < _navigationHistory.Count - 1)
        {
            _navigationHistory.RemoveRange(_navigationHistoryIndex + 1, _navigationHistory.Count - _navigationHistoryIndex - 1);
        }

        _navigationHistory.Add(moduleName);
        _navigationHistoryIndex = _navigationHistory.Count - 1;
    }

    private bool CanNavigateHome() => _allModules.Any();

    private void OnNavigateHome()
    {
        var target = FilteredModules.FirstOrDefault() ?? _allModules.FirstOrDefault();
        if (target is not null)
        {
            SelectedModule = target;
        }
    }

    private bool CanNavigatePrevious() => _navigationHistoryIndex > 0;

    private void OnNavigatePrevious()
    {
        if (!CanNavigatePrevious())
        {
            return;
        }

        _navigationHistoryIndex--;
        NavigateByHistory(_navigationHistory[_navigationHistoryIndex]);
    }

    private bool CanNavigateNext() => _navigationHistoryIndex >= 0 && _navigationHistoryIndex < _navigationHistory.Count - 1;

    private void OnNavigateNext()
    {
        if (!CanNavigateNext())
        {
            return;
        }

        _navigationHistoryIndex++;
        NavigateByHistory(_navigationHistory[_navigationHistoryIndex]);
    }

    private void NavigateByHistory(string moduleName)
    {
        var target = _allModules.FirstOrDefault(m => m.Name == moduleName);
        if (target is null)
        {
            return;
        }

        _recordSelectionInHistory = false;
        try
        {
            SelectedModule = target;
        }
        finally
        {
            _recordSelectionInHistory = true;
        }
    }

    private bool CanClearSearch() => !string.IsNullOrWhiteSpace(FilterText);

    private void OnClearSearch()
    {
        FilterText = string.Empty;
    }

    private void UpdateNavigationCommands()
    {
        NavigateHomeCommand.RaiseCanExecuteChanged();
        NavigatePreviousCommand.RaiseCanExecuteChanged();
        NavigateNextCommand.RaiseCanExecuteChanged();
        ClearSearchCommand.RaiseCanExecuteChanged();
        RaisePropertyChanged(nameof(NavigationStatus));
        RaisePropertyChanged(nameof(ModuleCountText));
    }

    /// <summary>
    /// 退出应用
    /// </summary>
    private void OnExitApplication()
    {
        Application.Current.Shutdown();
    }
}
