# BaiHua.Software 开发指南

## 🎯 开发工作流程

### Phase 1: 需求分析

- [ ] 明确功能需求
- [ ] 识别涉及的硬件模块（视觉、运动、IO等）
- [ ] 定义数据流向
- [ ] 确定新增还是修改现有功能

### Phase 2: 设计阶段

```
需求 → 接口设计 → 数据模型 → 数据库设计 → 详细设计
```

**设计清单**：
- [ ] 在 Core 层设计接口
- [ ] 设计数据模型（DTO、Entity）
- [ ] 规划数据库表结构
- [ ] 设计异常处理方案
- [ ] 规划日志记录点

### Phase 3: 开发阶段（自下而上）

```
1. Common → 通用工具
2. Core → 接口定义
3. Infrastructure → 数据访问
4. HAL → 硬件驱动
5. Services → 业务服务
6. BLL → 流程控制
7. UI → 用户界面
```

### Phase 4: 测试阶段

```
单元测试 → 集成测试 → 系统测试 → UAT
```

---

## 📝 开发规范

### 命名规范

#### 文件名

```
// Core 层
IWorkstationService.cs          // 接口
WorkstationStatus.cs            // 枚举
WorkstationDTO.cs               // DTO
Workstation.cs                  // 实体

// Infrastructure 层
FreeSqlRepository.cs            // 实现类
SerilogLogger.cs

// BLL 层
WorkstationBLL.cs
RecipeBLL.cs

// UI 层
MainWindow.xaml / MainWindow.xaml.cs
MainViewModel.cs
DashboardView.xaml / DashboardView.xaml.cs
DashboardViewModel.cs
```

#### 命名规则

```csharp
// 接口（I 前缀）
public interface IWorkstationService { }
public interface IHALDriver { }

// 抽象类（Abstract 或 Base 后缀）
public abstract class DriverBase { }
public abstract class ServiceBase { }

// 实现类（具体名称）
public class WorkstationBLL : IWorkstationService { }
public class MotionDriver : IMotionDriver { }

// 私有字段（_ 前缀）
private readonly IWorkstationService _service;
private string _internalState;

// 常量（UPPER_SNAKE_CASE）
private const int MAX_RETRIES = 3;
private const string CONFIG_FILE = "app.config";

// 属性（PascalCase）
public string Status { get; set; }
public int Timeout { get; set; }

// 方法（PascalCase）
public async Task Start() { }
public void LogError(string message) { }
```

### 代码格式规范

```csharp
// ✅ 正确

// 1. 类声明
public class MyService : IMyService
{
    // 2. 字段（private, readonly 优先）
    private readonly IDependency _dependency;
    private readonly ILogger _logger;
    
    // 3. 属性（auto-property）
    public string Name { get; set; }
    public int Value { get; set; }
    
    // 4. 构造函数
    public MyService(IDependency dependency, ILogger logger)
    {
        _dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    // 5. 公开方法
    public async Task DoWork()
    {
        _logger.LogInformation("Starting work");
        
        try
        {
            await _dependency.Execute();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Work failed");
            throw;
        }
    }
    
    // 6. 私有方法
    private void ValidateInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be empty");
    }
}
```

---

## 🔨 常见开发任务

### 任务 1：新增硬件驱动

**场景**：需要集成一个新的PLC通信协议

**步骤**：

1. **在 Core 层定义接口**
   ```csharp
   // BaiHua.Software.Core/Interfaces/INewPLCDriver.cs
   public interface INewPLCDriver
   {
       /// <summary>
       /// 初始化 PLC 连接
       /// </summary>
       Task Initialize(string host, int port);
       
       /// <summary>
       /// 读取寄存器值
       /// </summary>
       Task<int> ReadRegister(int address);
       
       /// <summary>
       /// 写入寄存器值
       /// </summary>
       Task WriteRegister(int address, int value);
       
       /// <summary>
       /// 断开连接
       /// </summary>
       Task Disconnect();
   }
   ```

2. **在 HAL 层实现驱动**
   ```csharp
   // BaiHua.Software.HAL/Communication/NewPLCDriver.cs
   public class NewPLCDriver : INewPLCDriver
   {
       private readonly ILogger<NewPLCDriver> _logger;
       private TcpClient _client;
       
       public NewPLCDriver(ILogger<NewPLCDriver> logger)
       {
           _logger = logger;
       }
       
       public async Task Initialize(string host, int port)
       {
           try
           {
               _client = new TcpClient();
               await _client.ConnectAsync(host, port);
               _logger.LogInformation($"Connected to PLC at {host}:{port}");
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Failed to initialize PLC connection");
               throw;
           }
       }
       
       public async Task<int> ReadRegister(int address)
       {
           // 实现协议读取逻辑
           _logger.LogDebug($"Reading register at address {address}");
           // ...
           return 0;
       }
       
       public async Task WriteRegister(int address, int value)
       {
           _logger.LogDebug($"Writing {value} to register at {address}");
           // ...
       }
       
       public async Task Disconnect()
       {
           _client?.Close();
           _logger.LogInformation("PLC connection closed");
       }
   }
   ```

3. **在 BLL 层使用驱动**
   ```csharp
   // BaiHua.Software.BLL/PLCControlBLL.cs
   public class PLCControlBLL
   {
       private readonly INewPLCDriver _plc;
       private readonly IAlarmService _alarm;
       
       public PLCControlBLL(INewPLCDriver plc, IAlarmService alarm)
       {
           _plc = plc;
           _alarm = alarm;
       }
       
       public async Task ExecuteCommand(int commandCode)
       {
           try
           {
               await _plc.Initialize("192.168.1.100", 502);
               await _plc.WriteRegister(0x0100, commandCode);
               var result = await _plc.ReadRegister(0x0101);
               
               if (result != 1)
                   await _alarm.RaiseAlarm("PLC command failed");
           }
           finally
           {
               await _plc.Disconnect();
           }
       }
   }
   ```

4. **在 DI 容器注册**
   ```csharp
   // Program.cs
   services.AddSingleton<INewPLCDriver, NewPLCDriver>();
   services.AddScoped<IPLCControlBLL, PLCControlBLL>();
   ```

---

### 任务 2：新增业务服务

**场景**：需要新增数据导出服务

**步骤**：

1. **定义接口**
   ```csharp
   // BaiHua.Software.Core/Interfaces/IExportService.cs
   public interface IExportService
   {
       /// <summary>
       /// 导出生产数据到 Excel
       /// </summary>
       Task ExportToExcel(DateTime startDate, DateTime endDate, string filePath);
       
       /// <summary>
       /// 导出生产数据到 CSV
       /// </summary>
       Task ExportToCSV(DateTime startDate, DateTime endDate, string filePath);
   }
   ```

2. **实现服务**
   ```csharp
   // BaiHua.Software.Services/ExportService.cs
   public class ExportService : IExportService
   {
       private readonly IRepository _repository;
       private readonly ILogger<ExportService> _logger;
       
       public ExportService(IRepository repository, ILogger<ExportService> logger)
       {
           _repository = repository;
           _logger = logger;
       }
       
       public async Task ExportToExcel(DateTime startDate, DateTime endDate, string filePath)
       {
           try
           {
               _logger.LogInformation($"Exporting data from {startDate} to {endDate}");
               
               // 1. 从数据库查询数据
               var data = await _repository.GetProductionData(startDate, endDate);
               
               // 2. 转换为 Excel 格式
               using (var workbook = new XLWorkbook())
               {
                   var worksheet = workbook.Worksheets.Add("Production");
                   // 填充数据...
                   workbook.SaveAs(filePath);
               }
               
               _logger.LogInformation("Export completed successfully");
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Export failed");
               throw;
           }
       }
       
       public async Task ExportToCSV(DateTime startDate, DateTime endDate, string filePath)
       {
           // 实现 CSV 导出...
       }
   }
   ```

3. **注册服务**
   ```csharp
   services.AddScoped<IExportService, ExportService>();
   ```

4. **在 ViewModel 中使用**
   ```csharp
   public class ExportViewModel
   {
       private readonly IExportService _exportService;
       
       public RelayCommand ExportCommand { get; }
       
       public ExportViewModel(IExportService exportService)
       {
           _exportService = exportService;
           ExportCommand = new RelayCommand(ExportData);
       }
       
       private async void ExportData()
       {
           try
           {
               var dialog = new SaveFileDialog { Filter = "Excel files|*.xlsx" };
               if (dialog.ShowDialog() == true)
               {
                   await _exportService.ExportToExcel(
                       StartDate, EndDate, dialog.FileName);
                   MessageBox.Show("Export completed");
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show($"Error: {ex.Message}");
           }
       }
   }
   ```

---

### 任务 3：新增 UI 页面

**场景**：需要新增生产报表页面

**步骤**：

1. **创建 XAML 文件**
   ```xml
   <!-- BaiHua.Software.UI/Views/ReportView.xaml -->
   <UserControl x:Class="BaiHua.Software.UI.Views.ReportView"
                xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
       <Grid>
           <Grid.RowDefinitions>
               <RowDefinition Height="Auto"/>
               <RowDefinition Height="*"/>
           </Grid.RowDefinitions>
           
           <!-- 工具栏 -->
           <StackPanel Orientation="Horizontal" Margin="5">
               <Button Content="生成报表" Command="{Binding GenerateCommand}" 
                       Padding="10,5" Margin="5"/>
               <Button Content="导出" Command="{Binding ExportCommand}" 
                       Padding="10,5" Margin="5"/>
           </StackPanel>
           
           <!-- 数据表格 -->
           <DataGrid Grid.Row="1" ItemsSource="{Binding ReportData}" 
                     IsReadOnly="True" Margin="5"/>
       </Grid>
   </UserControl>
   ```

2. **创建 ViewModel**
   ```csharp
   // BaiHua.Software.UI/ViewModels/ReportViewModel.cs
   public class ReportViewModel : ObservableObject
   {
       private readonly IReportService _reportService;
       private readonly IExportService _exportService;
       
       private ObservableCollection<ReportData> _reportData;
       
       public ObservableCollection<ReportData> ReportData
       {
           get => _reportData;
           set => SetProperty(ref _reportData, value);
       }
       
       public RelayCommand GenerateCommand { get; }
       public RelayCommand ExportCommand { get; }
       
       public ReportViewModel(IReportService reportService, 
                            IExportService exportService)
       {
           _reportService = reportService;
           _exportService = exportService;
           
           GenerateCommand = new RelayCommand(GenerateReport);
           ExportCommand = new RelayCommand(Export);
       }
       
       private async void GenerateReport()
       {
           try
           {
               var data = await _reportService.GenerateReport(
                   DateTime.Today, DateTime.Today);
               ReportData = new ObservableCollection<ReportData>(data);
           }
           catch (Exception ex)
           {
               MessageBox.Show($"Error: {ex.Message}");
           }
       }
       
       private async void Export()
       {
           try
           {
               var dialog = new SaveFileDialog { Filter = "Excel files|*.xlsx" };
               if (dialog.ShowDialog() == true)
               {
                   await _exportService.ExportToExcel(
                       DateTime.Today, DateTime.Today, dialog.FileName);
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show($"Error: {ex.Message}");
           }
       }
   }
   ```

3. **关联 View 和 ViewModel**
   ```csharp
   // BaiHua.Software.UI/Views/ReportView.xaml.cs
   public partial class ReportView : UserControl
   {
       public ReportView(ReportViewModel viewModel)
       {
           InitializeComponent();
           DataContext = viewModel;
       }
   }
   ```

4. **在主窗口注册**
   ```csharp
   // BaiHua.Software.UI/MainWindow.xaml
   <TabControl>
       <TabItem Header="报表">
           <local:ReportView/>
       </TabItem>
   </TabControl>
   ```

---

## 🧪 测试指南

### 单元测试示例

```csharp
// BaiHua.Software.Tests/BLLTests/WorkstationBLLTests.cs
[TestClass]
public class WorkstationBLLTests
{
    private Mock<IHALDriver> _halMock;
    private Mock<IAlarmService> _alarmMock;
    private WorkstationBLL _bll;
    
    [TestInitialize]
    public void Setup()
    {
        _halMock = new Mock<IHALDriver>();
        _alarmMock = new Mock<IAlarmService>();
        _bll = new WorkstationBLL(_halMock.Object, _alarmMock.Object);
    }
    
    [TestMethod]
    public async Task Start_WhenSuccessful_DoesNotRaiseAlarm()
    {
        // Arrange
        _halMock.Setup(x => x.Initialize()).Returns(Task.CompletedTask);
        
        // Act
        await _bll.Start();
        
        // Assert
        _alarmMock.Verify(x => x.RaiseAlarm(It.IsAny<string>()), 
            Times.Never);
    }
    
    [TestMethod]
    public async Task Start_WhenFailed_RaisesAlarm()
    {
        // Arrange
        _halMock.Setup(x => x.Initialize())
            .ThrowsAsync(new Exception("Hardware error"));
        
        // Act & Assert
        await _bll.Start();
        
        _alarmMock.Verify(x => x.RaiseAlarm(It.IsAny<string>()), 
            Times.Once);
    }
}
```

---

## 🚀 发布部署

### 发布清单

- [ ] 代码审查完成
- [ ] 所有测试通过
- [ ] 更新版本号
- [ ] 更新发行说明
- [ ] 生成发布版本
- [ ] 验证包完整性
- [ ] 备份数据库
- [ ] 执行数据迁移（如需要）
- [ ] 部署到生产环境
- [ ] 烟雾测试
- [ ] 监控系统日志

### 发布命令

```powershell
# 清理旧构建
dotnet clean

# 发布版本
dotnet publish -c Release -o ./publish

# 创建安装程序（如使用 WiX）
heat.exe dir ".\publish" -o files.wxs
candle.exe *.wxs
light.exe *.wixobj -o BaiHua.Software.msi
```

---

## 📚 参考资源

- [架构设计文档](./Architecture.md)
- [快速参考指南](./QuickStart.md)
- [模块协作指南](./Module-Integration.md)
- [.NET 最佳实践](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/)
- [C# 编码规范](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

---

**开发效率提示**：
- 使用 Visual Studio 的代码模板快速生成类和接口
- 利用 ReSharper 进行代码分析和重构
- 使用 Git 进行版本控制和分支管理
- 定期进行代码审查

