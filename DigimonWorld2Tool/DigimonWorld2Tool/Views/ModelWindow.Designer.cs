
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
            this.LoadAnimationButton = new System.Windows.Forms.Button();
            this.DigimonModelsComboBox = new System.Windows.Forms.ComboBox();
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
            // LoadAnimationButton
            // 
            this.LoadAnimationButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.LoadAnimationButton.FlatAppearance.BorderSize = 3;
            this.LoadAnimationButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadAnimationButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LoadAnimationButton.ForeColor = System.Drawing.Color.White;
            this.LoadAnimationButton.Location = new System.Drawing.Point(31, 111);
            this.LoadAnimationButton.Name = "LoadAnimationButton";
            this.LoadAnimationButton.Size = new System.Drawing.Size(139, 42);
            this.LoadAnimationButton.TabIndex = 2;
            this.LoadAnimationButton.Text = "Load Animation";
            this.LoadAnimationButton.UseVisualStyleBackColor = true;
            this.LoadAnimationButton.Click += new System.EventHandler(this.LoadAnimationButton_Click);
            // 
            // DigimonModelsComboBox
            // 
            this.DigimonModelsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DigimonModelsComboBox.FormattingEnabled = true;
            this.DigimonModelsComboBox.Location = new System.Drawing.Point(293, 123);
            this.DigimonModelsComboBox.Name = "DigimonModelsComboBox";
            this.DigimonModelsComboBox.Size = new System.Drawing.Size(254, 23);
            this.DigimonModelsComboBox.TabIndex = 3;
            this.DigimonModelsComboBox.SelectedIndexChanged += new System.EventHandler(this.DigimonModelsComboBox_SelectedIndexChanged);
            // 
            // ModelWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.Controls.Add(this.DigimonModelsComboBox);
            this.Controls.Add(this.LoadAnimationButton);
            this.Controls.Add(this.ConvertToFbxButton);
            this.Controls.Add(this.LoadModelButton);
            this.Name = "ModelWindow";
            this.Size = new System.Drawing.Size(1100, 660);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LoadModelButton;
        private System.Windows.Forms.Button ConvertToFbxButton;
        private System.Windows.Forms.Button LoadAnimationButton;
        private System.Windows.Forms.ComboBox DigimonModelsComboBox;
    }
}
