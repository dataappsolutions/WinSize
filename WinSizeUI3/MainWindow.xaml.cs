using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using WinSize;
using static WinSize.WinSizeShared;

namespace WinSizeUI3;

public sealed partial class MainWindow : Window
{
    private record DisplaySizeItem(string Title, int Width, int Height)
    {
        public override string ToString() => Title;
    }

    public MainWindow()
    {
        InitializeComponent();
        var area = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
        var workArea = area.WorkArea;
        AppWindow.Resize(new Windows.Graphics.SizeInt32(1920, 1080));
        AppWindow.Move(new Windows.Graphics.PointInt32(
            (workArea.Width - 1920) / 2,
            (workArea.Height - 1080) / 2));
        LoadPresets();
        RefreshWindows();
    }

    private void LoadPresets()
    {
        PresetsComboBox.ItemsSource = new[]
        {
            new DisplaySizeItem("800 x 600 (SVGA)", 800, 600),
            new DisplaySizeItem("1024 x 768 (XGA)", 1024, 768),
            new DisplaySizeItem("1280 x 720 (HD 720p)", 1280, 720),
            new DisplaySizeItem("1440 x 900 (16:10)", 1440, 900),
            new DisplaySizeItem("1600 x 900 (HD+ / 900p)", 1600, 900),
            new DisplaySizeItem("1920 x 1080 (Full HD / 1080p)", 1920, 1080),
            new DisplaySizeItem("2560 x 1440 (QHD / 1440p)", 2560, 1440),
            new DisplaySizeItem("3840 x 2160 (4K UHD / 2160p)", 3840, 2160),
        };
    }

    private void RefreshWindows() =>
        WindowsListView.ItemsSource = WinSizeShared.GetOpenWindows();

    private void RefreshButton_Click(object sender, RoutedEventArgs e) =>
        RefreshWindows();

    private async void ResizeButton_Click(object sender, RoutedEventArgs e)
    {
        if (WindowsListView.SelectedItem is not WindowInfo selected)
        {
            await ShowDialog("No window slected", "Please select a window from the list.");
            return;
        }

        bool ok = WinSizeShared.TryResizeWindow(
            selected.Handle,
            (int)WidthBox.Value,
            (int)HeightBox.Value,
            out int err,
            CenterCheckBox.IsChecked == true);

        if (!ok)
            await ShowDialog("Resize failed", $"Could not resize the window. Win32 error: {err}");
    }

    private void PresetsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PresetsComboBox.SelectedItem is DisplaySizeItem preset)
        {
            WidthBox.Value = preset.Width;
            HeightBox.Value = preset.Height;
        }
    }

    private async Task ShowDialog(string title, string message)
    {
        var dlg = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = Content.XamlRoot
        };
        await dlg.ShowAsync().AsTask();
    }
}



















//using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Controls;
//using Microsoft.UI.Xaml.Controls.Primitives;
//using Microsoft.UI.Xaml.Data;
//using Microsoft.UI.Xaml.Input;
//using Microsoft.UI.Xaml.Media;
//using Microsoft.UI.Xaml.Navigation;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.Foundation;
//using Windows.Foundation.Collections;

//// To learn more about WinUI, the WinUI project structure,
//// and more about our project templates, see: http://aka.ms/winui-project-info.

//namespace WinSizeUI3
//{
//    /// <summary>
//    /// An empty window that can be used on its own or navigated to within a Frame.
//    /// </summary>
//    public sealed partial class MainWindow : Window
//    {
//        public MainWindow()
//        {
//            InitializeComponent();
//        }
//    }
//}
