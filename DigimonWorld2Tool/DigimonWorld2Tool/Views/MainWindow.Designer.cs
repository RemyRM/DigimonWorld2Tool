﻿
namespace DigimonWorld2Tool.Views
{
    partial class MainWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.MainWindowHostPanel = new System.Windows.Forms.Panel();
            this.OpenMapWindowButton = new System.Windows.Forms.Button();
            this.OpenTexturesWindowButton = new System.Windows.Forms.Button();
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.MainToolStripDebugItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainToolStripOpenLogWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowValuesAsHexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeIDCapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainToolStripEditorItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EnableEditModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eNEMYSETToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenSkinsWindowButton = new System.Windows.Forms.Button();
            this.OpenModelWindowButton = new System.Windows.Forms.Button();
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainWindowHostPanel
            // 
            this.MainWindowHostPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainWindowHostPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.MainWindowHostPanel.Location = new System.Drawing.Point(96, 38);
            this.MainWindowHostPanel.MinimumSize = new System.Drawing.Size(1155, 660);
            this.MainWindowHostPanel.Name = "MainWindowHostPanel";
            this.MainWindowHostPanel.Size = new System.Drawing.Size(1155, 660);
            this.MainWindowHostPanel.TabIndex = 0;
            // 
            // OpenMapWindowButton
            // 
            this.OpenMapWindowButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.OpenMapWindowButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.OpenMapWindowButton.FlatAppearance.BorderSize = 5;
            this.OpenMapWindowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenMapWindowButton.ForeColor = System.Drawing.Color.White;
            this.OpenMapWindowButton.Location = new System.Drawing.Point(10, 37);
            this.OpenMapWindowButton.Name = "OpenMapWindowButton";
            this.OpenMapWindowButton.Size = new System.Drawing.Size(80, 45);
            this.OpenMapWindowButton.TabIndex = 1;
            this.OpenMapWindowButton.Text = "Map";
            this.OpenMapWindowButton.UseVisualStyleBackColor = false;
            this.OpenMapWindowButton.Click += new System.EventHandler(this.OpenMapWindowButton_Click);
            // 
            // OpenTexturesWindowButton
            // 
            this.OpenTexturesWindowButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.OpenTexturesWindowButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.OpenTexturesWindowButton.FlatAppearance.BorderSize = 5;
            this.OpenTexturesWindowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenTexturesWindowButton.Location = new System.Drawing.Point(10, 91);
            this.OpenTexturesWindowButton.Name = "OpenTexturesWindowButton";
            this.OpenTexturesWindowButton.Size = new System.Drawing.Size(80, 45);
            this.OpenTexturesWindowButton.TabIndex = 2;
            this.OpenTexturesWindowButton.Text = "Textures";
            this.OpenTexturesWindowButton.UseVisualStyleBackColor = false;
            this.OpenTexturesWindowButton.Click += new System.EventHandler(this.OpenTexturesWindowButton_Click);
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainToolStripDebugItem,
            this.MainToolStripEditorItem,
            this.helpToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(1264, 24);
            this.MainMenuStrip.TabIndex = 3;
            this.MainMenuStrip.Text = "menuStrip1";
            // 
            // MainToolStripDebugItem
            // 
            this.MainToolStripDebugItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainToolStripOpenLogWindow,
            this.ShowValuesAsHexToolStripMenuItem,
            this.removeIDCapToolStripMenuItem});
            this.MainToolStripDebugItem.Name = "MainToolStripDebugItem";
            this.MainToolStripDebugItem.Size = new System.Drawing.Size(61, 20);
            this.MainToolStripDebugItem.Text = "Settings";
            // 
            // MainToolStripOpenLogWindow
            // 
            this.MainToolStripOpenLogWindow.Name = "MainToolStripOpenLogWindow";
            this.MainToolStripOpenLogWindow.Size = new System.Drawing.Size(180, 22);
            this.MainToolStripOpenLogWindow.Text = "Open log window";
            this.MainToolStripOpenLogWindow.Click += new System.EventHandler(this.MainToolStripOpenLogWindow_Click);
            // 
            // ShowValuesAsHexToolStripMenuItem
            // 
            this.ShowValuesAsHexToolStripMenuItem.CheckOnClick = true;
            this.ShowValuesAsHexToolStripMenuItem.Name = "ShowValuesAsHexToolStripMenuItem";
            this.ShowValuesAsHexToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ShowValuesAsHexToolStripMenuItem.Text = "Show values as Hex";
            this.ShowValuesAsHexToolStripMenuItem.Click += new System.EventHandler(this.ShowValuesAsHexToolStripMenuItem_Click);
            // 
            // removeIDCapToolStripMenuItem
            // 
            this.removeIDCapToolStripMenuItem.CheckOnClick = true;
            this.removeIDCapToolStripMenuItem.Name = "removeIDCapToolStripMenuItem";
            this.removeIDCapToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeIDCapToolStripMenuItem.Text = "Remove ID cap";
            this.removeIDCapToolStripMenuItem.Click += new System.EventHandler(this.RemoveIDCapToolStripMenuItem_Click);
            // 
            // MainToolStripEditorItem
            // 
            this.MainToolStripEditorItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EnableEditModeToolStripMenuItem,
            this.settingFilesToolStripMenuItem});
            this.MainToolStripEditorItem.Name = "MainToolStripEditorItem";
            this.MainToolStripEditorItem.Size = new System.Drawing.Size(50, 20);
            this.MainToolStripEditorItem.Text = "Editor";
            // 
            // EnableEditModeToolStripMenuItem
            // 
            this.EnableEditModeToolStripMenuItem.Name = "EnableEditModeToolStripMenuItem";
            this.EnableEditModeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.EnableEditModeToolStripMenuItem.Text = "Enable Edit Mode";
            this.EnableEditModeToolStripMenuItem.Click += new System.EventHandler(this.EnableEditModeToolStripMenuItem_Click);
            // 
            // settingFilesToolStripMenuItem
            // 
            this.settingFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eNEMYSETToolStripMenuItem});
            this.settingFilesToolStripMenuItem.Name = "settingFilesToolStripMenuItem";
            this.settingFilesToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.settingFilesToolStripMenuItem.Text = "Setting Files";
            // 
            // eNEMYSETToolStripMenuItem
            // 
            this.eNEMYSETToolStripMenuItem.Name = "eNEMYSETToolStripMenuItem";
            this.eNEMYSETToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.eNEMYSETToolStripMenuItem.Text = "ENEMYSET";
            this.eNEMYSETToolStripMenuItem.Click += new System.EventHandler(this.ENEMYSETToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.AboutToolStripMenuItem.Text = "About";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // OpenSkinsWindowButton
            // 
            this.OpenSkinsWindowButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.OpenSkinsWindowButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.OpenSkinsWindowButton.FlatAppearance.BorderSize = 5;
            this.OpenSkinsWindowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenSkinsWindowButton.Location = new System.Drawing.Point(10, 146);
            this.OpenSkinsWindowButton.Name = "OpenSkinsWindowButton";
            this.OpenSkinsWindowButton.Size = new System.Drawing.Size(80, 45);
            this.OpenSkinsWindowButton.TabIndex = 4;
            this.OpenSkinsWindowButton.Text = "Skins";
            this.OpenSkinsWindowButton.UseVisualStyleBackColor = false;
            this.OpenSkinsWindowButton.Click += new System.EventHandler(this.OpenSkinsWindowButton_Click);
            // 
            // OpenModelWindowButton
            // 
            this.OpenModelWindowButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.OpenModelWindowButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.OpenModelWindowButton.FlatAppearance.BorderSize = 5;
            this.OpenModelWindowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenModelWindowButton.Location = new System.Drawing.Point(10, 202);
            this.OpenModelWindowButton.Name = "OpenModelWindowButton";
            this.OpenModelWindowButton.Size = new System.Drawing.Size(80, 45);
            this.OpenModelWindowButton.TabIndex = 5;
            this.OpenModelWindowButton.Text = "Models";
            this.OpenModelWindowButton.UseVisualStyleBackColor = false;
            this.OpenModelWindowButton.Click += new System.EventHandler(this.OpenModelWindowButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1264, 709);
            this.Controls.Add(this.OpenModelWindowButton);
            this.Controls.Add(this.OpenSkinsWindowButton);
            this.Controls.Add(this.OpenTexturesWindowButton);
            this.Controls.Add(this.OpenMapWindowButton);
            this.Controls.Add(this.MainWindowHostPanel);
            this.Controls.Add(this.MainMenuStrip);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "MainWindow";
            this.Text = "Digimon World 2 Tool";
            this.ResizeEnd += new System.EventHandler(this.MainWindow_ResizeEnd);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel MainWindowHostPanel;
        private System.Windows.Forms.Button OpenMapWindowButton;
        private System.Windows.Forms.Button OpenTexturesWindowButton;
        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MainToolStripDebugItem;
        private System.Windows.Forms.ToolStripMenuItem MainToolStripOpenLogWindow;
        private System.Windows.Forms.ToolStripMenuItem MainToolStripEditorItem;
        private System.Windows.Forms.ToolStripMenuItem EnableEditModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowValuesAsHexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeIDCapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eNEMYSETToolStripMenuItem;
        private System.Windows.Forms.Button OpenSkinsWindowButton;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.Button OpenModelWindowButton;
    }
}