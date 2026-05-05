namespace WinSizeUI
{
    partial class winsizeMainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(winsizeMainForm));
            widthUpDown = new NumericUpDown();
            heightUpDown = new NumericUpDown();
            windowsListBox = new ListBox();
            refreshButton = new Button();
            resizeButton = new Button();
            feedbackButton = new Button();
            label1 = new Label();
            label2 = new Label();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            displaySizesCombo = new ComboBox();
            winAlign_checkBox1 = new CheckBox();
            toolTip1 = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)widthUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)heightUpDown).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // widthUpDown
            // 
            widthUpDown.Location = new Point(617, 492);
            widthUpDown.Name = "widthUpDown";
            widthUpDown.Size = new Size(180, 31);
            widthUpDown.TabIndex = 0;
            // 
            // heightUpDown
            // 
            heightUpDown.Location = new Point(617, 545);
            heightUpDown.Name = "heightUpDown";
            heightUpDown.Size = new Size(180, 31);
            heightUpDown.TabIndex = 1;
            // 
            // windowsListBox
            // 
            windowsListBox.FormattingEnabled = true;
            windowsListBox.ItemHeight = 25;
            windowsListBox.Location = new Point(12, 119);
            windowsListBox.Name = "windowsListBox";
            windowsListBox.Size = new Size(785, 229);
            windowsListBox.TabIndex = 2;
            // 
            // refreshButton
            // 
            refreshButton.Location = new Point(569, 612);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(111, 33);
            refreshButton.TabIndex = 3;
            refreshButton.Text = "Refresh";
            refreshButton.UseVisualStyleBackColor = true;
            refreshButton.Click += refreshButton_Click;
            // 
            // resizeButton
            // 
            resizeButton.Location = new Point(686, 612);
            resizeButton.Name = "resizeButton";
            resizeButton.Size = new Size(111, 33);
            resizeButton.TabIndex = 4;
            resizeButton.Text = "Resize";
            resizeButton.UseVisualStyleBackColor = true;
            resizeButton.Click += resizeButton_Click;
            // 
            // feedbackButton
            // 
            feedbackButton.BackgroundImage = (Image)resources.GetObject("feedbackButton.BackgroundImage");
            feedbackButton.BackgroundImageLayout = ImageLayout.Zoom;
            feedbackButton.Location = new Point(754, 36);
            feedbackButton.Name = "feedbackButton";
            feedbackButton.Size = new Size(43, 43);
            feedbackButton.TabIndex = 9;
            toolTip1.SetToolTip(feedbackButton, "Feedback");
            feedbackButton.UseVisualStyleBackColor = true;
            feedbackButton.Click += feedbackButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(521, 499);
            label1.Name = "label1";
            label1.Size = new Size(64, 25);
            label1.TabIndex = 5;
            label1.Text = "Width:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(521, 550);
            label2.Name = "label2";
            label2.Size = new Size(69, 25);
            label2.TabIndex = 6;
            label2.Text = "Height:";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(808, 33);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(141, 34);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // displaySizesCombo
            // 
            displaySizesCombo.FormattingEnabled = true;
            displaySizesCombo.Location = new Point(521, 430);
            displaySizesCombo.Margin = new Padding(4, 5, 4, 5);
            displaySizesCombo.Name = "displaySizesCombo";
            displaySizesCombo.Size = new Size(274, 33);
            displaySizesCombo.TabIndex = 8;
            displaySizesCombo.SelectedIndexChanged += displaySizesCombo_SelectedIndexChanged;
            // 
            // winAlign_checkBox1
            // 
            winAlign_checkBox1.AutoSize = true;
            winAlign_checkBox1.CheckAlign = ContentAlignment.MiddleRight;
            winAlign_checkBox1.Location = new Point(450, 380);
            winAlign_checkBox1.Name = "winAlign_checkBox1";
            winAlign_checkBox1.Size = new Size(347, 29);
            winAlign_checkBox1.TabIndex = 10;
            winAlign_checkBox1.Text = "Center Align Windows Before Resizing?";
            winAlign_checkBox1.UseVisualStyleBackColor = true;
            winAlign_checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // winsizeMainForm
            // 
            AcceptButton = refreshButton;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(808, 662);
            Controls.Add(winAlign_checkBox1);
            Controls.Add(feedbackButton);
            Controls.Add(displaySizesCombo);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(resizeButton);
            Controls.Add(refreshButton);
            Controls.Add(windowsListBox);
            Controls.Add(heightUpDown);
            Controls.Add(widthUpDown);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "winsizeMainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "WinSize";
            ((System.ComponentModel.ISupportInitialize)widthUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)heightUpDown).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown widthUpDown;
        private NumericUpDown heightUpDown;
        private ListBox windowsListBox;
        private Button refreshButton;
        private Button resizeButton;
        private Button feedbackButton;
        private Label label1;
        private Label label2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ComboBox displaySizesCombo;
        private CheckBox winAlign_checkBox1;
        private ToolTip toolTip1;
    }
}
