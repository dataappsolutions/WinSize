//namespace WinSizeUI
//{
//    public partial class winsizeMainForm : Form
//    {
//        public winsizeMainForm()
//        {
//            InitializeComponent();
//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {

//        }

//        private void label1_Click(object sender, EventArgs e)
//        {

//        }

//        private void windowsListBox_SelectedIndexChanged(object sender, EventArgs e)
//        {

//        }

//        private void widthUpDown_ValueChanged(object sender, EventArgs e)
//        {

//        }

//        private void heightUpDown_ValueChanged(object sender, EventArgs e)
//        {

//        }

//        private void resizeButton_Click(object sender, EventArgs e)
//        {

//        }

//        private void refreshButton_Click(object sender, EventArgs e)
//        {

//        }
//    }
//}

using System;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using WinSize; // namespace from console project

namespace WinSizeUI
{

    public partial class winsizeMainForm : Form
    {
        public const string FeedbackUrl = "https://github.com/dataappsolutions/WinSize/issues";

        public winsizeMainForm()
        {
            InitializeComponent();

            this.AcceptButton = resizeButton;

            widthUpDown.Minimum = 1;
            widthUpDown.Maximum = 10000;
            widthUpDown.Value = 800;

            heightUpDown.Minimum = 1;
            heightUpDown.Maximum = 10000;
            heightUpDown.Value = 600;

            //DisplaySizeItem ds = new();
            //ds.title= "800 x 600 (SVGA)"; ds.width = 800; ds.height = 600;
            //displaySizesCombo.Items.Add(ds);
            //displaySizesCombo.SelectedIndex = 0;
            //ds.title = "1024 x 768 (XGA)"; ds.width = 1024; ds.height = 768;
            //displaySizesCombo.Items.Add(ds);
            //ds.title = "1280 x 720 (HD 720p)"; ds.width = 1280; ds.height = 720;
            //displaySizesCombo.Items.Add(ds);
            //ds.title = "1440 x 900 (16:10)"; ds.width= 1440; ds.height = 900;
            //displaySizesCombo.Items.Add(ds);
            //ds.title = "1600 x 900 (HD+ / 900p)"; ds.width = 1600; ds.height = 900;
            //displaySizesCombo.Items.Add(ds);
            //ds.title = "1920 x 1080 (Full HD / 1080p)"; ds.width = 1920; ds.height = 1080;
            //displaySizesCombo.Items.Add(ds);
            //ds.title = "2560 x 1440 (QHD / 1440p)"; ds.width = 2560; ds.height = 1440;
            //displaySizesCombo.Items.Add(ds);
            //ds.title = "3840 x 2160 (4K UHD / 2160p)"; ds.width = 3840; ds.height = 2160;
            //displaySizesCombo.Items.Add(ds);
            List<DisplaySizeItem> displayDataList = new List<DisplaySizeItem>()
{
                new DisplaySizeItem { title = "800 x 600 (SVGA)", width = 800, height = 600 },
                new DisplaySizeItem { title = "1024 x 768 (XGA)", width = 1024, height = 768 },
                new DisplaySizeItem { title = "1280 x 720 (HD 720p)", width = 1280, height = 720 },
                new DisplaySizeItem { title = "1440 x 900 (16:10)", width = 1440, height = 900 },
                new DisplaySizeItem { title = "1600 x 900 (HD+ / 900p)", width = 1600, height = 900 },
                new DisplaySizeItem { title = "1920 x 1080 (Full HD / 1080p)", width = 1920, height = 1080 },
                new DisplaySizeItem { title = "2560 x 1440 (QHD / 1440p)", width = 2560, height = 1440 },
                new DisplaySizeItem { title = "3840 x 2160 (4K UHD / 2160p)", width = 3840, height = 2160 }
};
            // Iterate through the list and add each item to the combo box
            foreach (var data in displayDataList)
            {
                // Create a new DisplaySizeItem for each entry
                DisplaySizeItem ds = new DisplaySizeItem();
                ds.title = data.title;
                ds.width = data.width;
                ds.height = data.height;
                displaySizesCombo.Items.Add(ds);
            }

            // Set the selected index after all items are added
            if (displaySizesCombo.Items.Count > 0)
            {
                displaySizesCombo.SelectedIndex = 0;
            }
            RefreshWindows();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            RefreshWindows();
        }

        private void RefreshWindows()
        {
            var windows = WinSize.WinSizeShared.GetOpenWindows();

            windowsListBox.BeginUpdate();
            try
            {
                windowsListBox.DataSource = null;
                windowsListBox.DataSource = windows.ToList(); // bind a concrete list
            }
            finally
            {
                windowsListBox.EndUpdate();
            }
        }

        private void resizeButton_Click(object sender, EventArgs e)
        {
            if (windowsListBox.SelectedItem is not WinSize.WinSizeShared.WindowInfo w)
            {
                MessageBox.Show("Select a window first.", "WinSizeUI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int width = (int)widthUpDown.Value;
            int height = (int)heightUpDown.Value;

            if (!WinSize.WinSizeShared.TryResizeWindow(w.Handle, width, height, out int err, winAlign_checkBox1.Checked))
            {
                MessageBox.Show($"Resize failed. Win32 error: {err}", "WinSizeUI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Optional: confirm success
            MessageBox.Show("The Window '" + w.Title + "' was resized.", "WinSize");
        }

        private void feedbackButton_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = FeedbackUrl });
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void displaySizesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySizeItem? dsSelected = new();
            dsSelected = (DisplaySizeItem)displaySizesCombo.Items[displaySizesCombo.SelectedIndex];
            if (dsSelected != null)
            {
                widthUpDown.Value = dsSelected.width;
                heightUpDown.Value = dsSelected.height;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    public class DisplaySizeItem
    {
        public string? title; public int width; public int height;
        public override string ToString()
        {
            if (title == null || title.Length == 0) return string.Empty;
            else return title;
        }
    }
}
