
namespace DigimonWorld2Tool.Views
{
    partial class DebugWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugWindow));
            this.DebugMessageListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // DebugMessageListBox
            // 
            this.DebugMessageListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugMessageListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.DebugMessageListBox.ForeColor = System.Drawing.Color.White;
            this.DebugMessageListBox.FormattingEnabled = true;
            this.DebugMessageListBox.HorizontalScrollbar = true;
            this.DebugMessageListBox.ItemHeight = 17;
            this.DebugMessageListBox.Location = new System.Drawing.Point(13, 13);
            this.DebugMessageListBox.Name = "DebugMessageListBox";
            this.DebugMessageListBox.Size = new System.Drawing.Size(1168, 293);
            this.DebugMessageListBox.TabIndex = 0;
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1193, 319);
            this.Controls.Add(this.DebugMessageListBox);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DebugWindow";
            this.Text = "Debug Window";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox DebugMessageListBox;
    }
}