
namespace DigimonWorld2Tool.Views
{
    partial class ModelWindow
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
            this.LoadModelButton = new System.Windows.Forms.Button();
            this.ConvertToFbxButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LoadModelButton
            // 
            this.LoadModelButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.LoadModelButton.FlatAppearance.BorderSize = 3;
            this.LoadModelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadModelButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LoadModelButton.ForeColor = System.Drawing.Color.White;
            this.LoadModelButton.Location = new System.Drawing.Point(31, 26);
            this.LoadModelButton.Name = "LoadModelButton";
            this.LoadModelButton.Size = new System.Drawing.Size(114, 42);
            this.LoadModelButton.TabIndex = 0;
            this.LoadModelButton.Text = "Load Model";
            this.LoadModelButton.UseVisualStyleBackColor = true;
            this.LoadModelButton.Click += new System.EventHandler(this.LoadModelButton_Click);
            // 
            // ConvertToFbxButton
            // 
            this.ConvertToFbxButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.ConvertToFbxButton.FlatAppearance.BorderSize = 3;
            this.ConvertToFbxButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConvertToFbxButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ConvertToFbxButton.ForeColor = System.Drawing.Color.White;
            this.ConvertToFbxButton.Location = new System.Drawing.Point(283, 26);
            this.ConvertToFbxButton.Name = "ConvertToFbxButton";
            this.ConvertToFbxButton.Size = new System.Drawing.Size(149, 42);
            this.ConvertToFbxButton.TabIndex = 1;
            this.ConvertToFbxButton.Text = "Convert to FBX";
            this.ConvertToFbxButton.UseVisualStyleBackColor = true;
            this.ConvertToFbxButton.Click += new System.EventHandler(this.ConvertToFbxButton_Click);
            // 
            // ModelWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.Controls.Add(this.ConvertToFbxButton);
            this.Controls.Add(this.LoadModelButton);
            this.Name = "ModelWindow";
            this.Size = new System.Drawing.Size(1100, 660);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LoadModelButton;
        private System.Windows.Forms.Button ConvertToFbxButton;
    }
}
