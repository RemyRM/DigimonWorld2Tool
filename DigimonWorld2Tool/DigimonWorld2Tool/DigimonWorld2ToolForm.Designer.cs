
namespace DigimonWorld2Tool
{
    partial class DigimonWorld2ToolForm
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
            this.TabControlMain = new System.Windows.Forms.TabControl();
            this.MapVisualizerTab = new System.Windows.Forms.TabPage();
            this.SaveLayoutToFileButton = new System.Windows.Forms.Button();
            this.ShowGridCheckbox = new System.Windows.Forms.CheckBox();
            this.LayoutInformationGroupbox = new System.Windows.Forms.GroupBox();
            this.OccuranceChanceLabel = new System.Windows.Forms.Label();
            this.ResizeGridButton = new System.Windows.Forms.Button();
            this.TileSizeInput = new System.Windows.Forms.NumericUpDown();
            this.TileSizeLabel = new System.Windows.Forms.Label();
            this.MousePositionOnGridLabel = new System.Windows.Forms.Label();
            this.LayoutNotAvailableLabel = new System.Windows.Forms.Label();
            this.FloorSelectorComboBox = new System.Windows.Forms.ComboBox();
            this.SelectFloorLabel = new System.Windows.Forms.Label();
            this.MapLayoutsTabControl = new System.Windows.Forms.TabControl();
            this.TabLayoutPage0 = new System.Windows.Forms.TabPage();
            this.renderLayoutTab0 = new DigimonWorld2Tool.UserControls.RenderLayoutTab();
            this.TabLayoutPage1 = new System.Windows.Forms.TabPage();
            this.renderLayoutTab1 = new DigimonWorld2Tool.UserControls.RenderLayoutTab();
            this.TabLayoutPage2 = new System.Windows.Forms.TabPage();
            this.renderLayoutTab2 = new DigimonWorld2Tool.UserControls.RenderLayoutTab();
            this.TabLayoutPage3 = new System.Windows.Forms.TabPage();
            this.renderLayoutTab3 = new DigimonWorld2Tool.UserControls.RenderLayoutTab();
            this.TabLayoutPage4 = new System.Windows.Forms.TabPage();
            this.renderLayoutTab4 = new DigimonWorld2Tool.UserControls.RenderLayoutTab();
            this.TabLayoutPage5 = new System.Windows.Forms.TabPage();
            this.renderLayoutTab5 = new DigimonWorld2Tool.UserControls.RenderLayoutTab();
            this.TabLayoutPage6 = new System.Windows.Forms.TabPage();
            this.renderLayoutTab6 = new DigimonWorld2Tool.UserControls.RenderLayoutTab();
            this.TabLayoutPage7 = new System.Windows.Forms.TabPage();
            this.renderLayoutTab7 = new DigimonWorld2Tool.UserControls.RenderLayoutTab();
            this.DungeonFilesComboBox = new System.Windows.Forms.ComboBox();
            this.DomainNameLabel = new System.Windows.Forms.Label();
            this.MapCustomizerTab = new System.Windows.Forms.TabPage();
            this.TabControlMain.SuspendLayout();
            this.MapVisualizerTab.SuspendLayout();
            this.LayoutInformationGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileSizeInput)).BeginInit();
            this.MapLayoutsTabControl.SuspendLayout();
            this.TabLayoutPage0.SuspendLayout();
            this.TabLayoutPage1.SuspendLayout();
            this.TabLayoutPage2.SuspendLayout();
            this.TabLayoutPage3.SuspendLayout();
            this.TabLayoutPage4.SuspendLayout();
            this.TabLayoutPage5.SuspendLayout();
            this.TabLayoutPage6.SuspendLayout();
            this.TabLayoutPage7.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControlMain
            // 
            this.TabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControlMain.Controls.Add(this.MapVisualizerTab);
            this.TabControlMain.Controls.Add(this.MapCustomizerTab);
            this.TabControlMain.Location = new System.Drawing.Point(-5, 0);
            this.TabControlMain.Name = "TabControlMain";
            this.TabControlMain.SelectedIndex = 0;
            this.TabControlMain.Size = new System.Drawing.Size(1189, 867);
            this.TabControlMain.TabIndex = 0;
            this.TabControlMain.SelectedIndexChanged += new System.EventHandler(this.TabControlMain_SelectedIndexChanged);
            // 
            // MapVisualizerTab
            // 
            this.MapVisualizerTab.AutoScroll = true;
            this.MapVisualizerTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.MapVisualizerTab.Controls.Add(this.SaveLayoutToFileButton);
            this.MapVisualizerTab.Controls.Add(this.ShowGridCheckbox);
            this.MapVisualizerTab.Controls.Add(this.LayoutInformationGroupbox);
            this.MapVisualizerTab.Controls.Add(this.ResizeGridButton);
            this.MapVisualizerTab.Controls.Add(this.TileSizeInput);
            this.MapVisualizerTab.Controls.Add(this.TileSizeLabel);
            this.MapVisualizerTab.Controls.Add(this.MousePositionOnGridLabel);
            this.MapVisualizerTab.Controls.Add(this.LayoutNotAvailableLabel);
            this.MapVisualizerTab.Controls.Add(this.FloorSelectorComboBox);
            this.MapVisualizerTab.Controls.Add(this.SelectFloorLabel);
            this.MapVisualizerTab.Controls.Add(this.MapLayoutsTabControl);
            this.MapVisualizerTab.Controls.Add(this.DungeonFilesComboBox);
            this.MapVisualizerTab.Controls.Add(this.DomainNameLabel);
            this.MapVisualizerTab.Location = new System.Drawing.Point(4, 24);
            this.MapVisualizerTab.Name = "MapVisualizerTab";
            this.MapVisualizerTab.Padding = new System.Windows.Forms.Padding(3);
            this.MapVisualizerTab.Size = new System.Drawing.Size(1181, 839);
            this.MapVisualizerTab.TabIndex = 0;
            this.MapVisualizerTab.Text = "Map Visualizer";
            // 
            // SaveLayoutToFileButton
            // 
            this.SaveLayoutToFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveLayoutToFileButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveLayoutToFileButton.Location = new System.Drawing.Point(570, 798);
            this.SaveLayoutToFileButton.Name = "SaveLayoutToFileButton";
            this.SaveLayoutToFileButton.Size = new System.Drawing.Size(102, 32);
            this.SaveLayoutToFileButton.TabIndex = 16;
            this.SaveLayoutToFileButton.Text = "Save Layout";
            this.SaveLayoutToFileButton.UseVisualStyleBackColor = true;
            this.SaveLayoutToFileButton.Click += new System.EventHandler(this.SaveLayoutToFileButton_Click);
            // 
            // ShowGridCheckbox
            // 
            this.ShowGridCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ShowGridCheckbox.AutoSize = true;
            this.ShowGridCheckbox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ShowGridCheckbox.ForeColor = System.Drawing.Color.White;
            this.ShowGridCheckbox.Location = new System.Drawing.Point(462, 250);
            this.ShowGridCheckbox.Name = "ShowGridCheckbox";
            this.ShowGridCheckbox.Size = new System.Drawing.Size(100, 25);
            this.ShowGridCheckbox.TabIndex = 15;
            this.ShowGridCheckbox.Text = "Show grid";
            this.ShowGridCheckbox.UseVisualStyleBackColor = true;
            this.ShowGridCheckbox.CheckedChanged += new System.EventHandler(this.ShowGridCheckbox_CheckedChanged);
            // 
            // LayoutInformationGroupbox
            // 
            this.LayoutInformationGroupbox.Controls.Add(this.OccuranceChanceLabel);
            this.LayoutInformationGroupbox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LayoutInformationGroupbox.ForeColor = System.Drawing.Color.White;
            this.LayoutInformationGroupbox.Location = new System.Drawing.Point(12, 12);
            this.LayoutInformationGroupbox.Name = "LayoutInformationGroupbox";
            this.LayoutInformationGroupbox.Size = new System.Drawing.Size(652, 191);
            this.LayoutInformationGroupbox.TabIndex = 14;
            this.LayoutInformationGroupbox.TabStop = false;
            this.LayoutInformationGroupbox.Text = "Layout information";
            // 
            // OccuranceChanceLabel
            // 
            this.OccuranceChanceLabel.AutoSize = true;
            this.OccuranceChanceLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OccuranceChanceLabel.ForeColor = System.Drawing.Color.White;
            this.OccuranceChanceLabel.Location = new System.Drawing.Point(7, 34);
            this.OccuranceChanceLabel.Name = "OccuranceChanceLabel";
            this.OccuranceChanceLabel.Size = new System.Drawing.Size(86, 21);
            this.OccuranceChanceLabel.TabIndex = 13;
            this.OccuranceChanceLabel.Text = "Occurance:";
            // 
            // ResizeGridButton
            // 
            this.ResizeGridButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ResizeGridButton.Location = new System.Drawing.Point(371, 250);
            this.ResizeGridButton.Name = "ResizeGridButton";
            this.ResizeGridButton.Size = new System.Drawing.Size(75, 23);
            this.ResizeGridButton.TabIndex = 12;
            this.ResizeGridButton.Text = "Resize grid";
            this.ResizeGridButton.UseVisualStyleBackColor = true;
            this.ResizeGridButton.Click += new System.EventHandler(this.ResizeGridButton_Click);
            // 
            // TileSizeInput
            // 
            this.TileSizeInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TileSizeInput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TileSizeInput.Location = new System.Drawing.Point(321, 250);
            this.TileSizeInput.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.TileSizeInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TileSizeInput.Name = "TileSizeInput";
            this.TileSizeInput.Size = new System.Drawing.Size(44, 23);
            this.TileSizeInput.TabIndex = 11;
            this.TileSizeInput.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // TileSizeLabel
            // 
            this.TileSizeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TileSizeLabel.AutoSize = true;
            this.TileSizeLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TileSizeLabel.ForeColor = System.Drawing.Color.White;
            this.TileSizeLabel.Location = new System.Drawing.Point(248, 250);
            this.TileSizeLabel.Name = "TileSizeLabel";
            this.TileSizeLabel.Size = new System.Drawing.Size(67, 21);
            this.TileSizeLabel.TabIndex = 10;
            this.TileSizeLabel.Text = "Tile size:";
            // 
            // MousePositionOnGridLabel
            // 
            this.MousePositionOnGridLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MousePositionOnGridLabel.AutoSize = true;
            this.MousePositionOnGridLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MousePositionOnGridLabel.ForeColor = System.Drawing.Color.White;
            this.MousePositionOnGridLabel.Location = new System.Drawing.Point(595, 250);
            this.MousePositionOnGridLabel.Name = "MousePositionOnGridLabel";
            this.MousePositionOnGridLabel.Size = new System.Drawing.Size(74, 21);
            this.MousePositionOnGridLabel.TabIndex = 9;
            this.MousePositionOnGridLabel.Text = "X:00 Y:00";
            // 
            // LayoutNotAvailableLabel
            // 
            this.LayoutNotAvailableLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LayoutNotAvailableLabel.AutoSize = true;
            this.LayoutNotAvailableLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LayoutNotAvailableLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.LayoutNotAvailableLabel.Location = new System.Drawing.Point(12, 810);
            this.LayoutNotAvailableLabel.Name = "LayoutNotAvailableLabel";
            this.LayoutNotAvailableLabel.Size = new System.Drawing.Size(455, 20);
            this.LayoutNotAvailableLabel.TabIndex = 7;
            this.LayoutNotAvailableLabel.Text = "Layout not be selected as it not unique, selecting last unique layout.";
            this.LayoutNotAvailableLabel.Visible = false;
            // 
            // FloorSelectorComboBox
            // 
            this.FloorSelectorComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FloorSelectorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FloorSelectorComboBox.FormattingEnabled = true;
            this.FloorSelectorComboBox.Location = new System.Drawing.Point(110, 250);
            this.FloorSelectorComboBox.Name = "FloorSelectorComboBox";
            this.FloorSelectorComboBox.Size = new System.Drawing.Size(121, 23);
            this.FloorSelectorComboBox.TabIndex = 6;
            this.FloorSelectorComboBox.SelectedIndexChanged += new System.EventHandler(this.FloorSelectorComboBox_SelectedIndexChanged);
            // 
            // SelectFloorLabel
            // 
            this.SelectFloorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectFloorLabel.AutoSize = true;
            this.SelectFloorLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SelectFloorLabel.ForeColor = System.Drawing.Color.White;
            this.SelectFloorLabel.Location = new System.Drawing.Point(12, 250);
            this.SelectFloorLabel.Name = "SelectFloorLabel";
            this.SelectFloorLabel.Size = new System.Drawing.Size(91, 21);
            this.SelectFloorLabel.TabIndex = 5;
            this.SelectFloorLabel.Text = "Select floor:";
            // 
            // MapLayoutsTabControl
            // 
            this.MapLayoutsTabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.MapLayoutsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MapLayoutsTabControl.Controls.Add(this.TabLayoutPage0);
            this.MapLayoutsTabControl.Controls.Add(this.TabLayoutPage1);
            this.MapLayoutsTabControl.Controls.Add(this.TabLayoutPage2);
            this.MapLayoutsTabControl.Controls.Add(this.TabLayoutPage3);
            this.MapLayoutsTabControl.Controls.Add(this.TabLayoutPage4);
            this.MapLayoutsTabControl.Controls.Add(this.TabLayoutPage5);
            this.MapLayoutsTabControl.Controls.Add(this.TabLayoutPage6);
            this.MapLayoutsTabControl.Controls.Add(this.TabLayoutPage7);
            this.MapLayoutsTabControl.Location = new System.Drawing.Point(12, 285);
            this.MapLayoutsTabControl.Multiline = true;
            this.MapLayoutsTabControl.Name = "MapLayoutsTabControl";
            this.MapLayoutsTabControl.SelectedIndex = 0;
            this.MapLayoutsTabControl.Size = new System.Drawing.Size(660, 520);
            this.MapLayoutsTabControl.TabIndex = 4;
            this.MapLayoutsTabControl.SelectedIndexChanged += new System.EventHandler(this.MapLayoutsTabControl_SelectedIndexChanged);
            // 
            // TabLayoutPage0
            // 
            this.TabLayoutPage0.BackColor = System.Drawing.Color.Black;
            this.TabLayoutPage0.Controls.Add(this.renderLayoutTab0);
            this.TabLayoutPage0.Location = new System.Drawing.Point(4, 4);
            this.TabLayoutPage0.Name = "TabLayoutPage0";
            this.TabLayoutPage0.Padding = new System.Windows.Forms.Padding(3);
            this.TabLayoutPage0.Size = new System.Drawing.Size(652, 492);
            this.TabLayoutPage0.TabIndex = 4;
            this.TabLayoutPage0.Text = "Layout 0";
            // 
            // renderLayoutTab0
            // 
            this.renderLayoutTab0.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderLayoutTab0.AutoScroll = true;
            this.renderLayoutTab0.BackColor = System.Drawing.Color.Black;
            this.renderLayoutTab0.Location = new System.Drawing.Point(3, 3);
            this.renderLayoutTab0.Name = "renderLayoutTab0";
            this.renderLayoutTab0.Size = new System.Drawing.Size(645, 485);
            this.renderLayoutTab0.TabIndex = 0;
            // 
            // TabLayoutPage1
            // 
            this.TabLayoutPage1.BackColor = System.Drawing.Color.Black;
            this.TabLayoutPage1.Controls.Add(this.renderLayoutTab1);
            this.TabLayoutPage1.Location = new System.Drawing.Point(4, 4);
            this.TabLayoutPage1.Name = "TabLayoutPage1";
            this.TabLayoutPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabLayoutPage1.Size = new System.Drawing.Size(652, 492);
            this.TabLayoutPage1.TabIndex = 5;
            this.TabLayoutPage1.Text = "Layout 1";
            // 
            // renderLayoutTab1
            // 
            this.renderLayoutTab1.AutoScroll = true;
            this.renderLayoutTab1.BackColor = System.Drawing.Color.Black;
            this.renderLayoutTab1.Location = new System.Drawing.Point(3, 3);
            this.renderLayoutTab1.Name = "renderLayoutTab1";
            this.renderLayoutTab1.Size = new System.Drawing.Size(645, 485);
            this.renderLayoutTab1.TabIndex = 0;
            // 
            // TabLayoutPage2
            // 
            this.TabLayoutPage2.BackColor = System.Drawing.Color.Black;
            this.TabLayoutPage2.Controls.Add(this.renderLayoutTab2);
            this.TabLayoutPage2.Location = new System.Drawing.Point(4, 4);
            this.TabLayoutPage2.Name = "TabLayoutPage2";
            this.TabLayoutPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabLayoutPage2.Size = new System.Drawing.Size(652, 492);
            this.TabLayoutPage2.TabIndex = 6;
            this.TabLayoutPage2.Text = "Layout 2";
            // 
            // renderLayoutTab2
            // 
            this.renderLayoutTab2.AutoScroll = true;
            this.renderLayoutTab2.BackColor = System.Drawing.Color.Black;
            this.renderLayoutTab2.Location = new System.Drawing.Point(3, 3);
            this.renderLayoutTab2.Name = "renderLayoutTab2";
            this.renderLayoutTab2.Size = new System.Drawing.Size(645, 485);
            this.renderLayoutTab2.TabIndex = 0;
            // 
            // TabLayoutPage3
            // 
            this.TabLayoutPage3.BackColor = System.Drawing.Color.Black;
            this.TabLayoutPage3.Controls.Add(this.renderLayoutTab3);
            this.TabLayoutPage3.Location = new System.Drawing.Point(4, 4);
            this.TabLayoutPage3.Name = "TabLayoutPage3";
            this.TabLayoutPage3.Padding = new System.Windows.Forms.Padding(3);
            this.TabLayoutPage3.Size = new System.Drawing.Size(652, 492);
            this.TabLayoutPage3.TabIndex = 7;
            this.TabLayoutPage3.Text = "Layout 3";
            // 
            // renderLayoutTab3
            // 
            this.renderLayoutTab3.AutoScroll = true;
            this.renderLayoutTab3.BackColor = System.Drawing.Color.Black;
            this.renderLayoutTab3.Location = new System.Drawing.Point(3, 3);
            this.renderLayoutTab3.Name = "renderLayoutTab3";
            this.renderLayoutTab3.Size = new System.Drawing.Size(645, 485);
            this.renderLayoutTab3.TabIndex = 0;
            // 
            // TabLayoutPage4
            // 
            this.TabLayoutPage4.BackColor = System.Drawing.Color.Black;
            this.TabLayoutPage4.Controls.Add(this.renderLayoutTab4);
            this.TabLayoutPage4.Location = new System.Drawing.Point(4, 4);
            this.TabLayoutPage4.Name = "TabLayoutPage4";
            this.TabLayoutPage4.Padding = new System.Windows.Forms.Padding(3);
            this.TabLayoutPage4.Size = new System.Drawing.Size(652, 492);
            this.TabLayoutPage4.TabIndex = 8;
            this.TabLayoutPage4.Text = "Layout 4";
            // 
            // renderLayoutTab4
            // 
            this.renderLayoutTab4.AutoScroll = true;
            this.renderLayoutTab4.BackColor = System.Drawing.Color.Black;
            this.renderLayoutTab4.Location = new System.Drawing.Point(3, 3);
            this.renderLayoutTab4.Name = "renderLayoutTab4";
            this.renderLayoutTab4.Size = new System.Drawing.Size(645, 485);
            this.renderLayoutTab4.TabIndex = 0;
            // 
            // TabLayoutPage5
            // 
            this.TabLayoutPage5.BackColor = System.Drawing.Color.Black;
            this.TabLayoutPage5.Controls.Add(this.renderLayoutTab5);
            this.TabLayoutPage5.Location = new System.Drawing.Point(4, 4);
            this.TabLayoutPage5.Name = "TabLayoutPage5";
            this.TabLayoutPage5.Padding = new System.Windows.Forms.Padding(3);
            this.TabLayoutPage5.Size = new System.Drawing.Size(652, 492);
            this.TabLayoutPage5.TabIndex = 9;
            this.TabLayoutPage5.Text = "Layout 5";
            // 
            // renderLayoutTab5
            // 
            this.renderLayoutTab5.AutoScroll = true;
            this.renderLayoutTab5.BackColor = System.Drawing.Color.Black;
            this.renderLayoutTab5.Location = new System.Drawing.Point(3, 3);
            this.renderLayoutTab5.Name = "renderLayoutTab5";
            this.renderLayoutTab5.Size = new System.Drawing.Size(645, 485);
            this.renderLayoutTab5.TabIndex = 0;
            // 
            // TabLayoutPage6
            // 
            this.TabLayoutPage6.BackColor = System.Drawing.Color.Black;
            this.TabLayoutPage6.Controls.Add(this.renderLayoutTab6);
            this.TabLayoutPage6.Location = new System.Drawing.Point(4, 4);
            this.TabLayoutPage6.Name = "TabLayoutPage6";
            this.TabLayoutPage6.Padding = new System.Windows.Forms.Padding(3);
            this.TabLayoutPage6.Size = new System.Drawing.Size(652, 492);
            this.TabLayoutPage6.TabIndex = 10;
            this.TabLayoutPage6.Text = "Layout 6";
            // 
            // renderLayoutTab6
            // 
            this.renderLayoutTab6.AutoScroll = true;
            this.renderLayoutTab6.BackColor = System.Drawing.Color.Black;
            this.renderLayoutTab6.Location = new System.Drawing.Point(3, 3);
            this.renderLayoutTab6.Name = "renderLayoutTab6";
            this.renderLayoutTab6.Size = new System.Drawing.Size(645, 485);
            this.renderLayoutTab6.TabIndex = 0;
            // 
            // TabLayoutPage7
            // 
            this.TabLayoutPage7.BackColor = System.Drawing.Color.Black;
            this.TabLayoutPage7.Controls.Add(this.renderLayoutTab7);
            this.TabLayoutPage7.Location = new System.Drawing.Point(4, 4);
            this.TabLayoutPage7.Name = "TabLayoutPage7";
            this.TabLayoutPage7.Padding = new System.Windows.Forms.Padding(3);
            this.TabLayoutPage7.Size = new System.Drawing.Size(652, 492);
            this.TabLayoutPage7.TabIndex = 11;
            this.TabLayoutPage7.Text = "Layout 7";
            // 
            // renderLayoutTab7
            // 
            this.renderLayoutTab7.AutoScroll = true;
            this.renderLayoutTab7.BackColor = System.Drawing.Color.Black;
            this.renderLayoutTab7.Location = new System.Drawing.Point(3, 3);
            this.renderLayoutTab7.Name = "renderLayoutTab7";
            this.renderLayoutTab7.Size = new System.Drawing.Size(645, 485);
            this.renderLayoutTab7.TabIndex = 0;
            // 
            // DungeonFilesComboBox
            // 
            this.DungeonFilesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DungeonFilesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DungeonFilesComboBox.FormattingEnabled = true;
            this.DungeonFilesComboBox.Location = new System.Drawing.Point(110, 215);
            this.DungeonFilesComboBox.MaxDropDownItems = 40;
            this.DungeonFilesComboBox.Name = "DungeonFilesComboBox";
            this.DungeonFilesComboBox.Size = new System.Drawing.Size(183, 23);
            this.DungeonFilesComboBox.TabIndex = 1;
            this.DungeonFilesComboBox.SelectedIndexChanged += new System.EventHandler(this.DungeonFilesComboBox_SelectedIndexChanged);
            // 
            // DomainNameLabel
            // 
            this.DomainNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DomainNameLabel.AutoSize = true;
            this.DomainNameLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DomainNameLabel.ForeColor = System.Drawing.Color.White;
            this.DomainNameLabel.Location = new System.Drawing.Point(12, 215);
            this.DomainNameLabel.Name = "DomainNameLabel";
            this.DomainNameLabel.Size = new System.Drawing.Size(68, 21);
            this.DomainNameLabel.TabIndex = 0;
            this.DomainNameLabel.Text = "Domain:";
            // 
            // MapCustomizerTab
            // 
            this.MapCustomizerTab.Location = new System.Drawing.Point(4, 24);
            this.MapCustomizerTab.Name = "MapCustomizerTab";
            this.MapCustomizerTab.Padding = new System.Windows.Forms.Padding(3);
            this.MapCustomizerTab.Size = new System.Drawing.Size(1181, 839);
            this.MapCustomizerTab.TabIndex = 1;
            this.MapCustomizerTab.Text = "Empty";
            this.MapCustomizerTab.UseVisualStyleBackColor = true;
            // 
            // DigimonWorld2ToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(1180, 863);
            this.Controls.Add(this.TabControlMain);
            this.Name = "DigimonWorld2ToolForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.DigimonWorld2ToolForm_Load);
            this.TabControlMain.ResumeLayout(false);
            this.MapVisualizerTab.ResumeLayout(false);
            this.MapVisualizerTab.PerformLayout();
            this.LayoutInformationGroupbox.ResumeLayout(false);
            this.LayoutInformationGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileSizeInput)).EndInit();
            this.MapLayoutsTabControl.ResumeLayout(false);
            this.TabLayoutPage0.ResumeLayout(false);
            this.TabLayoutPage1.ResumeLayout(false);
            this.TabLayoutPage2.ResumeLayout(false);
            this.TabLayoutPage3.ResumeLayout(false);
            this.TabLayoutPage4.ResumeLayout(false);
            this.TabLayoutPage5.ResumeLayout(false);
            this.TabLayoutPage6.ResumeLayout(false);
            this.TabLayoutPage7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControlMain;
        private System.Windows.Forms.TabPage MapVisualizerTab;
        private System.Windows.Forms.ComboBox DungeonFilesComboBox;
        private System.Windows.Forms.Label DomainNameLabel;
        private System.Windows.Forms.TabControl MapLayoutsTabControl;
        private System.Windows.Forms.TabPage TabLayoutPage0;
        private System.Windows.Forms.TabPage TabLayoutPage1;
        private System.Windows.Forms.TabPage TabLayoutPage2;
        private System.Windows.Forms.TabPage TabLayoutPage3;
        private System.Windows.Forms.TabPage TabLayoutPage4;
        private System.Windows.Forms.TabPage TabLayoutPage5;
        private System.Windows.Forms.TabPage TabLayoutPage6;
        private System.Windows.Forms.TabPage TabLayoutPage7;
        private System.Windows.Forms.Label SelectFloorLabel;
        private System.Windows.Forms.TabPage MapCustomizerTab;
        public System.Windows.Forms.ComboBox FloorSelectorComboBox;
        private System.Windows.Forms.Label LayoutNotAvailableLabel;
        private System.Windows.Forms.Label MousePositionOnGridLabel;
        private System.Windows.Forms.NumericUpDown TileSizeInput;
        private System.Windows.Forms.Label TileSizeLabel;
        private System.Windows.Forms.Button ResizeGridButton;
        private System.Windows.Forms.Label OccuranceChanceLabel;
        private System.Windows.Forms.GroupBox LayoutInformationGroupbox;
        public System.Windows.Forms.CheckBox ShowGridCheckbox;
        private System.Windows.Forms.Button SaveLayoutToFileButton;
        private UserControls.RenderLayoutTab renderLayoutTab0;
        private UserControls.RenderLayoutTab renderLayoutTab1;
        private UserControls.RenderLayoutTab renderLayoutTab2;
        private UserControls.RenderLayoutTab renderLayoutTab3;
        private UserControls.RenderLayoutTab renderLayoutTab4;
        private UserControls.RenderLayoutTab renderLayoutTab5;
        private UserControls.RenderLayoutTab renderLayoutTab6;
        private UserControls.RenderLayoutTab renderLayoutTab7;
    }
}

