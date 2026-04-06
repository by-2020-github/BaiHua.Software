@echo off
:: 设置控制台编码为 UTF-8，防止中文乱码
chcp 65001 >nul
echo ---------------------------------------------------
echo 开始创建 BaiHua.Software 上位机通用框架
echo [修复版]：修正 WPF 框架版本参数
echo ---------------------------------------------------

set SLN_NAME=BaiHua.Software

echo.
echo [1/6] 正在创建分层项目...
dotnet new classlib -n %SLN_NAME%.Core -f net8.0
dotnet new classlib -n %SLN_NAME%.Common -f net8.0
dotnet new classlib -n %SLN_NAME%.Infrastructure -f net8.0
dotnet new classlib -n %SLN_NAME%.HAL -f net8.0
dotnet new classlib -n %SLN_NAME%.BLL -f net8.0
dotnet new classlib -n %SLN_NAME%.Services -f net8.0
:: UI 层使用 WPF (修复：只写 net8.0，模板会自动处理 windows 平台标识)
dotnet new wpf -n %SLN_NAME%.UI -f net8.0

echo.
echo [2/6] 正在生成 %SLN_NAME%.slnx 解决方案文件...
:: 直接写入极简的 .slnx XML 结构
echo ^<Solution^> > %SLN_NAME%.slnx
echo   ^<Project Path="%SLN_NAME%.Core\%SLN_NAME%.Core.csproj" /^> >> %SLN_NAME%.slnx
echo   ^<Project Path="%SLN_NAME%.Common\%SLN_NAME%.Common.csproj" /^> >> %SLN_NAME%.slnx
echo   ^<Project Path="%SLN_NAME%.Infrastructure\%SLN_NAME%.Infrastructure.csproj" /^> >> %SLN_NAME%.slnx
echo   ^<Project Path="%SLN_NAME%.HAL\%SLN_NAME%.HAL.csproj" /^> >> %SLN_NAME%.slnx
echo   ^<Project Path="%SLN_NAME%.BLL\%SLN_NAME%.BLL.csproj" /^> >> %SLN_NAME%.slnx
echo   ^<Project Path="%SLN_NAME%.Services\%SLN_NAME%.Services.csproj" /^> >> %SLN_NAME%.slnx
echo   ^<Project Path="%SLN_NAME%.UI\%SLN_NAME%.UI.csproj" /^> >> %SLN_NAME%.slnx
echo ^</Solution^> >> %SLN_NAME%.slnx

echo.
echo [3/6] 正在配置项目引用关系...
:: 基础依赖
dotnet add %SLN_NAME%.Common\%SLN_NAME%.Common.csproj reference %SLN_NAME%.Core\%SLN_NAME%.Core.csproj
dotnet add %SLN_NAME%.Infrastructure\%SLN_NAME%.Infrastructure.csproj reference %SLN_NAME%.Core\%SLN_NAME%.Core.csproj %SLN_NAME%.Common\%SLN_NAME%.Common.csproj
dotnet add %SLN_NAME%.HAL\%SLN_NAME%.HAL.csproj reference %SLN_NAME%.Core\%SLN_NAME%.Core.csproj %SLN_NAME%.Common\%SLN_NAME%.Common.csproj
dotnet add %SLN_NAME%.Services\%SLN_NAME%.Services.csproj reference %SLN_NAME%.Core\%SLN_NAME%.Core.csproj %SLN_NAME%.Common\%SLN_NAME%.Common.csproj
:: 业务逻辑层依赖
dotnet add %SLN_NAME%.BLL\%SLN_NAME%.BLL.csproj reference %SLN_NAME%.Core\%SLN_NAME%.Core.csproj %SLN_NAME%.Services\%SLN_NAME%.Services.csproj %SLN_NAME%.HAL\%SLN_NAME%.HAL.csproj
:: UI层依赖
dotnet add %SLN_NAME%.UI\%SLN_NAME%.UI.csproj reference %SLN_NAME%.BLL\%SLN_NAME%.BLL.csproj %SLN_NAME%.Infrastructure\%SLN_NAME%.Infrastructure.csproj

echo.
echo [4/6] 正在安装 NuGet 核心依赖包...
:: UI 层依赖 (MVVM 与 DI)
dotnet add %SLN_NAME%.UI\%SLN_NAME%.UI.csproj package CommunityToolkit.Mvvm
dotnet add %SLN_NAME%.UI\%SLN_NAME%.UI.csproj package Microsoft.Extensions.Hosting
dotnet add %SLN_NAME%.UI\%SLN_NAME%.UI.csproj package Microsoft.Extensions.DependencyInjection

:: 基础设施层依赖 (ORM 与 日志)
dotnet add %SLN_NAME%.Infrastructure\%SLN_NAME%.Infrastructure.csproj package FreeSql
dotnet add %SLN_NAME%.Infrastructure\%SLN_NAME%.Infrastructure.csproj package FreeSql.Provider.Sqlite
dotnet add %SLN_NAME%.Infrastructure\%SLN_NAME%.Infrastructure.csproj package Serilog
dotnet add %SLN_NAME%.Infrastructure\%SLN_NAME%.Infrastructure.csproj package Serilog.Sinks.Console
dotnet add %SLN_NAME%.Infrastructure\%SLN_NAME%.Infrastructure.csproj package Serilog.Sinks.File

:: HAL 硬件抽象层依赖 (视觉与通信)
dotnet add %SLN_NAME%.HAL\%SLN_NAME%.HAL.csproj package OpenCvSharp4
dotnet add %SLN_NAME%.HAL\%SLN_NAME%.HAL.csproj package OpenCvSharp4.runtime.win
dotnet add %SLN_NAME%.HAL\%SLN_NAME%.HAL.csproj package FluentModbus

:: 通用工具层
dotnet add %SLN_NAME%.Common\%SLN_NAME%.Common.csproj package Newtonsoft.Json

echo.
echo [5/6] 正在清理默认类文件...
del %SLN_NAME%.Core\Class1.cs /q
del %SLN_NAME%.Common\Class1.cs /q
del %SLN_NAME%.Infrastructure\Class1.cs /q
del %SLN_NAME%.HAL\Class1.cs /q
del %SLN_NAME%.BLL\Class1.cs /q
del %SLN_NAME%.Services\Class1.cs /q

echo.
echo [6/6] 正在创建目录结构与说明文档 (.md)...

:: Core
echo # Core (核心层) > %SLN_NAME%.Core\Readme.md
echo 存放所有接口定义(Interfaces)、全局枚举(Enums)、数据传输对象(DTO)和领域模型。 >> %SLN_NAME%.Core\Readme.md
echo 设计原则：最底层的契约，没有任何外部依赖，其他所有层都可以安全引用它。 >> %SLN_NAME%.Core\Readme.md

:: Common
echo # Common (通用工具层) > %SLN_NAME%.Common\Readme.md
echo 存放全局通用的帮助类，如 Json序列化(Newtonsoft.Json)、加密解密、文件读写、扩展方法等。 >> %SLN_NAME%.Common\Readme.md

:: Infrastructure
echo # Infrastructure (基础设施层) > %SLN_NAME%.Infrastructure\Readme.md
echo 提供系统基础服务能力的实现，主要包含： >> %SLN_NAME%.Infrastructure\Readme.md
echo 1. 基于 FreeSql 的数据库访问仓储实现。 >> %SLN_NAME%.Infrastructure\Readme.md
echo 2. 基于 Serilog 的日志记录实现。 >> %SLN_NAME%.Infrastructure\Readme.md

:: HAL
echo # HAL (硬件抽象层 Hardware Abstraction Layer) > %SLN_NAME%.HAL\Readme.md
echo 存放具体硬件设备的驱动实现。>> %SLN_NAME%.HAL\Readme.md
echo - 视觉：OpenCvSharp4 封装 >> %SLN_NAME%.HAL\Readme.md
echo - 通信：FluentModbus (PLC通信等) >> %SLN_NAME%.HAL\Readme.md
echo - 运动控制及IO卡实现 >> %SLN_NAME%.HAL\Readme.md

:: Services
echo # Services (应用服务层) > %SLN_NAME%.Services\Readme.md
echo 存放与具体硬件无关的系统服务，如：报警服务、配方服务、用户权限服务等。 >> %SLN_NAME%.Services\Readme.md

:: BLL
echo # BLL (业务逻辑层) > %SLN_NAME%.BLL\Readme.md
echo 上位机的核心大脑。 >> %SLN_NAME%.BLL\Readme.md
echo 负责组合调用 HAL(硬件) 和 Services(服务)，实现工艺流程控制、工站状态机轮询、视觉算子逻辑等。 >> %SLN_NAME%.BLL\Readme.md

:: UI
mkdir %SLN_NAME%.UI\Views
mkdir %SLN_NAME%.UI\ViewModels
echo # UI (用户界面层) > %SLN_NAME%.UI\Readme.md
echo 基于 WPF 和 CommunityToolkit.Mvvm 搭建。 >> %SLN_NAME%.UI\Readme.md
echo 目录说明： >> %SLN_NAME%.UI\Readme.md
echo - Views: 存放 XAML 界面文件 >> %SLN_NAME%.UI\Readme.md
echo - ViewModels: 存放视图模型，处理数据绑定与交互指令 >> %SLN_NAME%.UI\Readme.md

echo.
echo ===================================================
echo 框架搭建完成！请使用 Visual Studio 2022 打开 %SLN_NAME%.slnx
echo ===================================================
pause