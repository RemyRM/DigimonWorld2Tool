
namespace DigimonWorld2Tool.Views
{
    partial class SkinsWindow
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SelectISOButton = new System.Windows.Forms.Button();
            this.SelectedIsoGroupBox = new System.Windows.Forms.GroupBox();
            this.SelectedISOPath = new System.Windows.Forms.Label();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.DigiBeetleTab = new System.Windows.Forms.TabPage();
            this.SaveDigiBeetleSkinButton = new System.Windows.Forms.Button();
            this.DigiBeetleAdamantBodyComboBox = new System.Windows.Forms.ComboBox();
            this.DigiBeetleTitaniumBodyComboBox = new System.Windows.Forms.ComboBox();
            this.DigiBeetleSteelBodyComboBox = new System.Windows.Forms.ComboBox();
            this.DigiBettleAdamantBodyLabel = new System.Windows.Forms.Label();
            this.DigiBettleTitaniumBodyLabel = new System.Windows.Forms.Label();
            this.DigiBettleSteelBodyLabel = new System.Windows.Forms.Label();
            this.DigiBeetleAdamantPanel = new System.Windows.Forms.Panel();
            this.DigiBeetleTitaniumPanel = new System.Windows.Forms.Panel();
            this.DigiBeetleSteelPanel = new System.Windows.Forms.Panel();
            this.DigiBeetlePresetComboBox = new System.Windows.Forms.ComboBox();
            this.PresetLabel = new System.Windows.Forms.Label();
            this.AikiraTab = new System.Windows.Forms.TabPage();
            this.SaveAkiraSkinButton = new System.Windows.Forms.Button();
            this.AkiraSkinPreviewPanel = new System.Windows.Forms.Panel();
            this.ChangePortraitCheckbox = new System.Windows.Forms.CheckBox();
            this.AkiraPortraitPreviewPanel = new System.Windows.Forms.Panel();
            this.AkiraStyleComboBox = new System.Windows.Forms.ComboBox();
            this.AkiraStyleLabel = new System.Windows.Forms.Label();
            this.pbarLabel = new System.Windows.Forms.Label();
            this.pbar = new System.Windows.Forms.ProgressBar();
            this.DigiBeetleBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.AkiraBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.BinaryLoaderBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SelectedIsoGroupBox.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.DigiBeetleTab.SuspendLayout();
            this.AikiraTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectISOButton
            // 
            this.SelectISOButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.SelectISOButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.SelectISOButton.FlatAppearance.BorderSize = 3;
            this.SelectISOButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectISOButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SelectISOButton.ForeColor = System.Drawing.Color.White;
            this.SelectISOButton.Location = new System.Drawing.Point(11, 13);
            this.SelectISOButton.Name = "SelectISOButton";
            this.SelectISOButton.Size = new System.Drawing.Size(85, 41);
            this.SelectISOButton.TabIndex = 3;
            this.SelectISOButton.Text = "Select ISO";
            this.SelectISOButton.UseVisualStyleBackColor = false;
            this.SelectISOButton.Click += new System.EventHandler(this.SelectISOButton_Click);
            // 
            // SelectedIsoGroupBox
            // 
            this.SelectedIsoGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedIsoGroupBox.Controls.Add(this.SelectedISOPath);
            this.SelectedIsoGroupBox.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SelectedIsoGroupBox.ForeColor = System.Drawing.Color.White;
            this.SelectedIsoGroupBox.Location = new System.Drawing.Point(101, 6);
            this.SelectedIsoGroupBox.Name = "SelectedIsoGroupBox";
            this.SelectedIsoGroupBox.Size = new System.Drawing.Size(1043, 48);
            this.SelectedIsoGroupBox.TabIndex = 4;
            this.SelectedIsoGroupBox.TabStop = false;
            this.SelectedIsoGroupBox.Text = "Selected ISO path";
            // 
            // SelectedISOPath
            // 
            this.SelectedISOPath.AutoSize = true;
            this.SelectedISOPath.Location = new System.Drawing.Point(7, 22);
            this.SelectedISOPath.Name = "SelectedISOPath";
            this.SelectedISOPath.Size = new System.Drawing.Size(122, 20);
            this.SelectedISOPath.TabIndex = 0;
            this.SelectedISOPath.Text = "No path selected";
            // 
            // MainTabControl
            // 
            this.MainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTabControl.Controls.Add(this.DigiBeetleTab);
            this.MainTabControl.Controls.Add(this.AikiraTab);
            this.MainTabControl.Location = new System.Drawing.Point(9, 66);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1134, 517);
            this.MainTabControl.TabIndex = 5;
            // 
            // DigiBeetleTab
            // 
            this.DigiBeetleTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.DigiBeetleTab.Controls.Add(this.SaveDigiBeetleSkinButton);
            this.DigiBeetleTab.Controls.Add(this.DigiBeetleAdamantBodyComboBox);
            this.DigiBeetleTab.Controls.Add(this.DigiBeetleTitaniumBodyComboBox);
            this.DigiBeetleTab.Controls.Add(this.DigiBeetleSteelBodyComboBox);
            this.DigiBeetleTab.Controls.Add(this.DigiBettleAdamantBodyLabel);
            this.DigiBeetleTab.Controls.Add(this.DigiBettleTitaniumBodyLabel);
            this.DigiBeetleTab.Controls.Add(this.DigiBettleSteelBodyLabel);
            this.DigiBeetleTab.Controls.Add(this.DigiBeetleAdamantPanel);
            this.DigiBeetleTab.Controls.Add(this.DigiBeetleTitaniumPanel);
            this.DigiBeetleTab.Controls.Add(this.DigiBeetleSteelPanel);
            this.DigiBeetleTab.Controls.Add(this.DigiBeetlePresetComboBox);
            this.DigiBeetleTab.Controls.Add(this.PresetLabel);
            this.DigiBeetleTab.Location = new System.Drawing.Point(4, 24);
            this.DigiBeetleTab.Name = "DigiBeetleTab";
            this.DigiBeetleTab.Padding = new System.Windows.Forms.Padding(3);
            this.DigiBeetleTab.Size = new System.Drawing.Size(1126, 489);
            this.DigiBeetleTab.TabIndex = 0;
            this.DigiBeetleTab.Text = "Digi-Beetle";
            // 
            // SaveDigiBeetleSkinButton
            // 
            this.SaveDigiBeetleSkinButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.SaveDigiBeetleSkinButton.Enabled = false;
            this.SaveDigiBeetleSkinButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.SaveDigiBeetleSkinButton.FlatAppearance.BorderSize = 3;
            this.SaveDigiBeetleSkinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveDigiBeetleSkinButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveDigiBeetleSkinButton.ForeColor = System.Drawing.Color.White;
            this.SaveDigiBeetleSkinButton.Location = new System.Drawing.Point(575, 16);
            this.SaveDigiBeetleSkinButton.Name = "SaveDigiBeetleSkinButton";
            this.SaveDigiBeetleSkinButton.Size = new System.Drawing.Size(100, 47);
            this.SaveDigiBeetleSkinButton.TabIndex = 11;
            this.SaveDigiBeetleSkinButton.Text = "Save";
            this.SaveDigiBeetleSkinButton.UseVisualStyleBackColor = false;
            this.SaveDigiBeetleSkinButton.Click += new System.EventHandler(this.SaveDigiBeetleSkinButton_Click);
            // 
            // DigiBeetleAdamantBodyComboBox
            // 
            this.DigiBeetleAdamantBodyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DigiBeetleAdamantBodyComboBox.FormattingEnabled = true;
            this.DigiBeetleAdamantBodyComboBox.Location = new System.Drawing.Point(934, 77);
            this.DigiBeetleAdamantBodyComboBox.Name = "DigiBeetleAdamantBodyComboBox";
            this.DigiBeetleAdamantBodyComboBox.Size = new System.Drawing.Size(121, 23);
            this.DigiBeetleAdamantBodyComboBox.TabIndex = 10;
            this.DigiBeetleAdamantBodyComboBox.SelectedIndexChanged += new System.EventHandler(this.DigiBeetleAdamantBodyComboBox_SelectedIndexChanged);
            // 
            // DigiBeetleTitaniumBodyComboBox
            // 
            this.DigiBeetleTitaniumBodyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DigiBeetleTitaniumBodyComboBox.FormattingEnabled = true;
            this.DigiBeetleTitaniumBodyComboBox.Location = new System.Drawing.Point(554, 77);
            this.DigiBeetleTitaniumBodyComboBox.Name = "DigiBeetleTitaniumBodyComboBox";
            this.DigiBeetleTitaniumBodyComboBox.Size = new System.Drawing.Size(121, 23);
            this.DigiBeetleTitaniumBodyComboBox.TabIndex = 9;
            this.DigiBeetleTitaniumBodyComboBox.SelectedIndexChanged += new System.EventHandler(this.DigiBeetleTitaniumBodyComboBox_SelectedIndexChanged);
            // 
            // DigiBeetleSteelBodyComboBox
            // 
            this.DigiBeetleSteelBodyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DigiBeetleSteelBodyComboBox.FormattingEnabled = true;
            this.DigiBeetleSteelBodyComboBox.Location = new System.Drawing.Point(155, 77);
            this.DigiBeetleSteelBodyComboBox.Name = "DigiBeetleSteelBodyComboBox";
            this.DigiBeetleSteelBodyComboBox.Size = new System.Drawing.Size(121, 23);
            this.DigiBeetleSteelBodyComboBox.TabIndex = 8;
            this.DigiBeetleSteelBodyComboBox.SelectedIndexChanged += new System.EventHandler(this.DigiBeetleSteelBodyComboBox_SelectedIndexChanged);
            // 
            // DigiBettleAdamantBodyLabel
            // 
            this.DigiBettleAdamantBodyLabel.AutoSize = true;
            this.DigiBettleAdamantBodyLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DigiBettleAdamantBodyLabel.ForeColor = System.Drawing.Color.White;
            this.DigiBettleAdamantBodyLabel.Location = new System.Drawing.Point(857, 84);
            this.DigiBettleAdamantBodyLabel.Name = "DigiBettleAdamantBodyLabel";
            this.DigiBettleAdamantBodyLabel.Size = new System.Drawing.Size(73, 20);
            this.DigiBettleAdamantBodyLabel.TabIndex = 7;
            this.DigiBettleAdamantBodyLabel.Text = "Adamant:";
            // 
            // DigiBettleTitaniumBodyLabel
            // 
            this.DigiBettleTitaniumBodyLabel.AutoSize = true;
            this.DigiBettleTitaniumBodyLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DigiBettleTitaniumBodyLabel.ForeColor = System.Drawing.Color.White;
            this.DigiBettleTitaniumBodyLabel.Location = new System.Drawing.Point(480, 84);
            this.DigiBettleTitaniumBodyLabel.Name = "DigiBettleTitaniumBodyLabel";
            this.DigiBettleTitaniumBodyLabel.Size = new System.Drawing.Size(70, 20);
            this.DigiBettleTitaniumBodyLabel.TabIndex = 6;
            this.DigiBettleTitaniumBodyLabel.Text = "Titanium:";
            // 
            // DigiBettleSteelBodyLabel
            // 
            this.DigiBettleSteelBodyLabel.AutoSize = true;
            this.DigiBettleSteelBodyLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DigiBettleSteelBodyLabel.ForeColor = System.Drawing.Color.White;
            this.DigiBettleSteelBodyLabel.Location = new System.Drawing.Point(106, 84);
            this.DigiBettleSteelBodyLabel.Name = "DigiBettleSteelBodyLabel";
            this.DigiBettleSteelBodyLabel.Size = new System.Drawing.Size(45, 20);
            this.DigiBettleSteelBodyLabel.TabIndex = 5;
            this.DigiBettleSteelBodyLabel.Text = "Steel:";
            // 
            // DigiBeetleAdamantPanel
            // 
            this.DigiBeetleAdamantPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.DigiBeetleAdamantPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DigiBeetleAdamantPanel.Location = new System.Drawing.Point(760, 115);
            this.DigiBeetleAdamantPanel.Name = "DigiBeetleAdamantPanel";
            this.DigiBeetleAdamantPanel.Size = new System.Drawing.Size(350, 350);
            this.DigiBeetleAdamantPanel.TabIndex = 4;
            // 
            // DigiBeetleTitaniumPanel
            // 
            this.DigiBeetleTitaniumPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.DigiBeetleTitaniumPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DigiBeetleTitaniumPanel.Location = new System.Drawing.Point(389, 115);
            this.DigiBeetleTitaniumPanel.Name = "DigiBeetleTitaniumPanel";
            this.DigiBeetleTitaniumPanel.Size = new System.Drawing.Size(350, 350);
            this.DigiBeetleTitaniumPanel.TabIndex = 3;
            // 
            // DigiBeetleSteelPanel
            // 
            this.DigiBeetleSteelPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.DigiBeetleSteelPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DigiBeetleSteelPanel.Location = new System.Drawing.Point(20, 115);
            this.DigiBeetleSteelPanel.Name = "DigiBeetleSteelPanel";
            this.DigiBeetleSteelPanel.Size = new System.Drawing.Size(350, 350);
            this.DigiBeetleSteelPanel.TabIndex = 2;
            // 
            // DigiBeetlePresetComboBox
            // 
            this.DigiBeetlePresetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DigiBeetlePresetComboBox.FormattingEnabled = true;
            this.DigiBeetlePresetComboBox.Location = new System.Drawing.Point(75, 30);
            this.DigiBeetlePresetComboBox.Name = "DigiBeetlePresetComboBox";
            this.DigiBeetlePresetComboBox.Size = new System.Drawing.Size(121, 23);
            this.DigiBeetlePresetComboBox.TabIndex = 1;
            this.DigiBeetlePresetComboBox.SelectedIndexChanged += new System.EventHandler(this.DigiBeetlePresetComboBox_SelectedIndexChanged);
            // 
            // PresetLabel
            // 
            this.PresetLabel.AutoSize = true;
            this.PresetLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PresetLabel.ForeColor = System.Drawing.Color.White;
            this.PresetLabel.Location = new System.Drawing.Point(26, 32);
            this.PresetLabel.Name = "PresetLabel";
            this.PresetLabel.Size = new System.Drawing.Size(52, 20);
            this.PresetLabel.TabIndex = 0;
            this.PresetLabel.Text = "Preset:";
            // 
            // AikiraTab
            // 
            this.AikiraTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.AikiraTab.Controls.Add(this.SaveAkiraSkinButton);
            this.AikiraTab.Controls.Add(this.AkiraSkinPreviewPanel);
            this.AikiraTab.Controls.Add(this.ChangePortraitCheckbox);
            this.AikiraTab.Controls.Add(this.AkiraPortraitPreviewPanel);
            this.AikiraTab.Controls.Add(this.AkiraStyleComboBox);
            this.AikiraTab.Controls.Add(this.AkiraStyleLabel);
            this.AikiraTab.Location = new System.Drawing.Point(4, 24);
            this.AikiraTab.Name = "AikiraTab";
            this.AikiraTab.Padding = new System.Windows.Forms.Padding(3);
            this.AikiraTab.Size = new System.Drawing.Size(1126, 489);
            this.AikiraTab.TabIndex = 1;
            this.AikiraTab.Text = "Akira";
            // 
            // SaveAkiraSkinButton
            // 
            this.SaveAkiraSkinButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveAkiraSkinButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.SaveAkiraSkinButton.Enabled = false;
            this.SaveAkiraSkinButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.SaveAkiraSkinButton.FlatAppearance.BorderSize = 3;
            this.SaveAkiraSkinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveAkiraSkinButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveAkiraSkinButton.ForeColor = System.Drawing.Color.White;
            this.SaveAkiraSkinButton.Location = new System.Drawing.Point(6, 436);
            this.SaveAkiraSkinButton.Name = "SaveAkiraSkinButton";
            this.SaveAkiraSkinButton.Size = new System.Drawing.Size(128, 47);
            this.SaveAkiraSkinButton.TabIndex = 5;
            this.SaveAkiraSkinButton.Text = "Save";
            this.SaveAkiraSkinButton.UseVisualStyleBackColor = false;
            // 
            // AkiraSkinPreviewPanel
            // 
            this.AkiraSkinPreviewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.AkiraSkinPreviewPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AkiraSkinPreviewPanel.Location = new System.Drawing.Point(275, 18);
            this.AkiraSkinPreviewPanel.Name = "AkiraSkinPreviewPanel";
            this.AkiraSkinPreviewPanel.Size = new System.Drawing.Size(450, 450);
            this.AkiraSkinPreviewPanel.TabIndex = 4;
            // 
            // ChangePortraitCheckbox
            // 
            this.ChangePortraitCheckbox.AutoSize = true;
            this.ChangePortraitCheckbox.Checked = true;
            this.ChangePortraitCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChangePortraitCheckbox.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ChangePortraitCheckbox.ForeColor = System.Drawing.Color.White;
            this.ChangePortraitCheckbox.Location = new System.Drawing.Point(23, 208);
            this.ChangePortraitCheckbox.Name = "ChangePortraitCheckbox";
            this.ChangePortraitCheckbox.Size = new System.Drawing.Size(130, 24);
            this.ChangePortraitCheckbox.TabIndex = 3;
            this.ChangePortraitCheckbox.Text = "Change Portrait";
            this.ChangePortraitCheckbox.UseVisualStyleBackColor = true;
            this.ChangePortraitCheckbox.CheckedChanged += new System.EventHandler(this.ChangePortraitCheckbox_CheckedChanged);
            // 
            // AkiraPortraitPreviewPanel
            // 
            this.AkiraPortraitPreviewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.AkiraPortraitPreviewPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AkiraPortraitPreviewPanel.Location = new System.Drawing.Point(20, 71);
            this.AkiraPortraitPreviewPanel.Name = "AkiraPortraitPreviewPanel";
            this.AkiraPortraitPreviewPanel.Size = new System.Drawing.Size(128, 128);
            this.AkiraPortraitPreviewPanel.TabIndex = 2;
            // 
            // AkiraStyleComboBox
            // 
            this.AkiraStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AkiraStyleComboBox.FormattingEnabled = true;
            this.AkiraStyleComboBox.Location = new System.Drawing.Point(109, 18);
            this.AkiraStyleComboBox.Name = "AkiraStyleComboBox";
            this.AkiraStyleComboBox.Size = new System.Drawing.Size(121, 23);
            this.AkiraStyleComboBox.TabIndex = 1;
            this.AkiraStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.AkiraStyleComboBox_SelectedIndexChanged);
            // 
            // AkiraStyleLabel
            // 
            this.AkiraStyleLabel.AutoSize = true;
            this.AkiraStyleLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AkiraStyleLabel.ForeColor = System.Drawing.Color.White;
            this.AkiraStyleLabel.Location = new System.Drawing.Point(23, 25);
            this.AkiraStyleLabel.Name = "AkiraStyleLabel";
            this.AkiraStyleLabel.Size = new System.Drawing.Size(82, 20);
            this.AkiraStyleLabel.TabIndex = 0;
            this.AkiraStyleLabel.Text = "Akira Style:";
            // 
            // pbarLabel
            // 
            this.pbarLabel.AutoSize = true;
            this.pbarLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pbarLabel.ForeColor = System.Drawing.Color.White;
            this.pbarLabel.Location = new System.Drawing.Point(155, 590);
            this.pbarLabel.Name = "pbarLabel";
            this.pbarLabel.Size = new System.Drawing.Size(0, 20);
            this.pbarLabel.TabIndex = 15;
            // 
            // pbar
            // 
            this.pbar.Location = new System.Drawing.Point(11, 589);
            this.pbar.Name = "pbar";
            this.pbar.Size = new System.Drawing.Size(136, 23);
            this.pbar.TabIndex = 14;
            this.pbar.Visible = false;
            // 
            // DigiBeetleBackgroundWorker
            // 
            this.DigiBeetleBackgroundWorker.WorkerReportsProgress = true;
            this.DigiBeetleBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DigiBeetleBackgroundWorker_DoWork);
            this.DigiBeetleBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.DigiBeetleBackgroundWorker_ProgressChanged);
            this.DigiBeetleBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DigiBeetleBackgroundWorker_RunWorkerCompleted);
            // 
            // AkiraBackgroundWorker
            // 
            this.AkiraBackgroundWorker.WorkerReportsProgress = true;
            this.AkiraBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AkiraBackgroundWorker_DoWork);
            this.AkiraBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.AkiraBackgroundWorker_ProgressChanged);
            this.AkiraBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AkiraBackgroundWorker_RunWorkerCompleted);
            // 
            // BinaryLoaderBackgroundWorker
            // 
            this.BinaryLoaderBackgroundWorker.WorkerReportsProgress = true;
            this.BinaryLoaderBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BinaryLoaderBackgroundWorker_DoWork);
            this.BinaryLoaderBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BinaryLoaderBackgroundWorker_ProgressChanged);
            this.BinaryLoaderBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BinaryLoaderBackgroundWorker_RunWorkerCompleted);
            // 
            // SkinsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.Controls.Add(this.SelectISOButton);
            this.Controls.Add(this.pbarLabel);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.pbar);
            this.Controls.Add(this.SelectedIsoGroupBox);
            this.Name = "SkinsWindow";
            this.Size = new System.Drawing.Size(1155, 660);
            this.SelectedIsoGroupBox.ResumeLayout(false);
            this.SelectedIsoGroupBox.PerformLayout();
            this.MainTabControl.ResumeLayout(false);
            this.DigiBeetleTab.ResumeLayout(false);
            this.DigiBeetleTab.PerformLayout();
            this.AikiraTab.ResumeLayout(false);
            this.AikiraTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelectISOButton;
        private System.Windows.Forms.GroupBox SelectedIsoGroupBox;
        private System.Windows.Forms.Label SelectedISOPath;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage DigiBeetleTab;
        private System.Windows.Forms.Button SaveDigiBeetleSkinButton;
        public System.Windows.Forms.ComboBox DigiBeetleAdamantBodyComboBox;
        public System.Windows.Forms.ComboBox DigiBeetleTitaniumBodyComboBox;
        public System.Windows.Forms.ComboBox DigiBeetleSteelBodyComboBox;
        private System.Windows.Forms.Label DigiBettleAdamantBodyLabel;
        private System.Windows.Forms.Label DigiBettleTitaniumBodyLabel;
        private System.Windows.Forms.Label DigiBettleSteelBodyLabel;
        public System.Windows.Forms.Panel DigiBeetleAdamantPanel;
        public System.Windows.Forms.Panel DigiBeetleTitaniumPanel;
        public System.Windows.Forms.Panel DigiBeetleSteelPanel;
        private System.Windows.Forms.ComboBox DigiBeetlePresetComboBox;
        private System.Windows.Forms.Label PresetLabel;
        private System.Windows.Forms.TabPage AikiraTab;
        private System.Windows.Forms.Button SaveAkiraSkinButton;
        internal System.Windows.Forms.Panel AkiraSkinPreviewPanel;
        private System.Windows.Forms.CheckBox ChangePortraitCheckbox;
        internal System.Windows.Forms.Panel AkiraPortraitPreviewPanel;
        internal System.Windows.Forms.ComboBox AkiraStyleComboBox;
        private System.Windows.Forms.Label AkiraStyleLabel;
        private System.Windows.Forms.Label pbarLabel;
        public System.Windows.Forms.ProgressBar pbar;
        public System.ComponentModel.BackgroundWorker DigiBeetleBackgroundWorker;
        public System.ComponentModel.BackgroundWorker AkiraBackgroundWorker;
        private System.ComponentModel.BackgroundWorker BinaryLoaderBackgroundWorker;
    }
}
