
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
            this.SuspendLayout();
            // 
            // MainWindowHostPanel
            // 
            this.MainWindowHostPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainWindowHostPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.MainWindowHostPanel.Location = new System.Drawing.Point(150, 10);
            this.MainWindowHostPanel.MinimumSize = new System.Drawing.Size(1100, 660);
            this.MainWindowHostPanel.Name = "MainWindowHostPanel";
            this.MainWindowHostPanel.Size = new System.Drawing.Size(1100, 660);
            this.MainWindowHostPanel.TabIndex = 0;
            // 
            // OpenMapWindowButton
            // 
            this.OpenMapWindowButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.OpenMapWindowButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.OpenMapWindowButton.FlatAppearance.BorderSize = 5;
            this.OpenMapWindowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenMapWindowButton.ForeColor = System.Drawing.Color.White;
            this.OpenMapWindowButton.Location = new System.Drawing.Point(10, 10);
            this.OpenMapWindowButton.Name = "OpenMapWindowButton";
            this.OpenMapWindowButton.Size = new System.Drawing.Size(130, 75);
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
            this.OpenTexturesWindowButton.Location = new System.Drawing.Point(10, 107);
            this.OpenTexturesWindowButton.Name = "OpenTexturesWindowButton";
            this.OpenTexturesWindowButton.Size = new System.Drawing.Size(130, 75);
            this.OpenTexturesWindowButton.TabIndex = 2;
            this.OpenTexturesWindowButton.Text = "Textures";
            this.OpenTexturesWindowButton.UseVisualStyleBackColor = false;
            this.OpenTexturesWindowButton.Click += new System.EventHandler(this.OpenTexturesWindowButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.OpenTexturesWindowButton);
            this.Controls.Add(this.OpenMapWindowButton);
            this.Controls.Add(this.MainWindowHostPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "MainWindow";
            this.Text = "Digimon World 2 Tool";
            this.ResizeEnd += new System.EventHandler(this.MainWindow_ResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainWindowHostPanel;
        private System.Windows.Forms.Button OpenMapWindowButton;
        private System.Windows.Forms.Button OpenTexturesWindowButton;
    }
}