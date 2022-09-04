
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
            this.MODELDT0GridView = new System.Windows.Forms.DataGridView();
            this.SelectMainModelDirButton = new System.Windows.Forms.Button();
            this.SelectedMainModelDirLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MODELDT0GridView)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadModelButton
            // 
            this.LoadModelButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.LoadModelButton.FlatAppearance.BorderSize = 3;
            this.LoadModelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadModelButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LoadModelButton.ForeColor = System.Drawing.Color.White;
            this.LoadModelButton.Location = new System.Drawing.Point(14, 445);
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
            this.ConvertToFbxButton.Location = new System.Drawing.Point(150, 445);
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
            this.LoadAnimationButton.Location = new System.Drawing.Point(314, 445);
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
            this.DigimonModelsComboBox.Location = new System.Drawing.Point(472, 457);
            this.DigimonModelsComboBox.Name = "DigimonModelsComboBox";
            this.DigimonModelsComboBox.Size = new System.Drawing.Size(254, 23);
            this.DigimonModelsComboBox.TabIndex = 3;
            this.DigimonModelsComboBox.SelectedIndexChanged += new System.EventHandler(this.DigimonModelsComboBox_SelectedIndexChanged);
            // 
            // MODELDT0GridView
            // 
            this.MODELDT0GridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.MODELDT0GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MODELDT0GridView.GridColor = System.Drawing.SystemColors.Control;
            this.MODELDT0GridView.Location = new System.Drawing.Point(14, 105);
            this.MODELDT0GridView.Name = "MODELDT0GridView";
            this.MODELDT0GridView.RowTemplate.Height = 25;
            this.MODELDT0GridView.Size = new System.Drawing.Size(1073, 318);
            this.MODELDT0GridView.TabIndex = 5;
            this.MODELDT0GridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MODELDT0GridView_CellContentClick);
            this.MODELDT0GridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.MODELDT0GridView_CellFormatting);
            this.MODELDT0GridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged_1);
            // 
            // SelectMainModelDirButton
            // 
            this.SelectMainModelDirButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.SelectMainModelDirButton.FlatAppearance.BorderSize = 3;
            this.SelectMainModelDirButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectMainModelDirButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SelectMainModelDirButton.ForeColor = System.Drawing.Color.White;
            this.SelectMainModelDirButton.Location = new System.Drawing.Point(14, 18);
            this.SelectMainModelDirButton.Name = "SelectMainModelDirButton";
            this.SelectMainModelDirButton.Size = new System.Drawing.Size(199, 42);
            this.SelectMainModelDirButton.TabIndex = 6;
            this.SelectMainModelDirButton.Text = "Select main Model folder";
            this.SelectMainModelDirButton.UseVisualStyleBackColor = true;
            this.SelectMainModelDirButton.Click += new System.EventHandler(this.SelectMainModelDirButton_Click);
            // 
            // SelectedMainModelDirLabel
            // 
            this.SelectedMainModelDirLabel.AutoSize = true;
            this.SelectedMainModelDirLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SelectedMainModelDirLabel.ForeColor = System.Drawing.Color.White;
            this.SelectedMainModelDirLabel.Location = new System.Drawing.Point(222, 31);
            this.SelectedMainModelDirLabel.Name = "SelectedMainModelDirLabel";
            this.SelectedMainModelDirLabel.Size = new System.Drawing.Size(123, 20);
            this.SelectedMainModelDirLabel.TabIndex = 7;
            this.SelectedMainModelDirLabel.Text = "Current directory:";
            // 
            // ModelWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.Controls.Add(this.SelectedMainModelDirLabel);
            this.Controls.Add(this.SelectMainModelDirButton);
            this.Controls.Add(this.MODELDT0GridView);
            this.Controls.Add(this.DigimonModelsComboBox);
            this.Controls.Add(this.LoadAnimationButton);
            this.Controls.Add(this.ConvertToFbxButton);
            this.Controls.Add(this.LoadModelButton);
            this.Name = "ModelWindow";
            this.Size = new System.Drawing.Size(1100, 660);
            ((System.ComponentModel.ISupportInitialize)(this.MODELDT0GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadModelButton;
        private System.Windows.Forms.Button ConvertToFbxButton;
        private System.Windows.Forms.Button LoadAnimationButton;
        private System.Windows.Forms.ComboBox DigimonModelsComboBox;
        private System.Windows.Forms.DataGridView MODELDT0GridView;
        private System.Windows.Forms.Button SelectMainModelDirButton;
        private System.Windows.Forms.Label SelectedMainModelDirLabel;
    }
}
