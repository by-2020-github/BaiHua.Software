using System.Windows;

namespace BaiHua.Software.UI;

/// <summary>
/// 应用程序主窗口
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}