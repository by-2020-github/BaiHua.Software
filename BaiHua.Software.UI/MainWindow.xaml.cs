using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AppModules = BaiHua.Software.Modules;

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

    private void OnCategoryModuleSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox currentListBox
            || e.AddedItems.Count == 0
            || e.AddedItems[0] is not AppModules.ModuleExport module
            || DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        ClearOtherCategorySelections(currentListBox);
        viewModel.SelectedModule = module;
    }

    private void ClearOtherCategorySelections(ListBox currentListBox)
    {
        foreach (var listBox in FindVisualChildren<ListBox>(this))
        {
            if (!ReferenceEquals(listBox, currentListBox)
                && listBox.Name == "CategoryModuleList"
                && listBox.SelectedItem is not null)
            {
                listBox.SelectedItem = null;
            }
        }
    }

    private static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent)
        where T : DependencyObject
    {
        var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (var index = 0; index < childrenCount; index++)
        {
            var child = VisualTreeHelper.GetChild(parent, index);
            if (child is T typedChild)
            {
                yield return typedChild;
            }

            foreach (var descendant in FindVisualChildren<T>(child))
            {
                yield return descendant;
            }
        }
    }
}