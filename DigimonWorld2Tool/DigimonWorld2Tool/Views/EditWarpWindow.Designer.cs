
namespace DigimonWorld2Tool.Views
{
    partial class EditWarpWindow
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
            this.EditWarpTypeLabel = new System.Windows.Forms.Label();
            this.EditWarpTypeComboBox = new System.Windows.Forms.ComboBox();
            this.EditWarpPositionXLabel = new System.Windows.Forms.Label();
            this.EditWarpPositionYLabel = new System.Windows.Forms.Label();
            this.WarpPositionXNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.WarpPositionYNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SaveChangesButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.WarpPositionXNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarpPositionYNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // EditWarpTypeLabel
            // 
            this.EditWarpTypeLabel.AutoSize = true;
            this.EditWarpTypeLabel.Location = new System.Drawing.Point(12, 27);
            this.EditWarpTypeLabel.Name = "EditWarpTypeLabel";
            this.EditWarpTypeLabel.Size = new System.Drawing.Size(75, 19);
            this.EditWarpTypeLabel.TabIndex = 0;
            this.EditWarpTypeLabel.Text = "Warp type:";
            // 
            // EditWarpTypeComboBox
            // 
            this.EditWarpTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EditWarpTypeComboBox.FormattingEnabled = true;
            this.EditWarpTypeComboBox.Items.AddRange(new object[] {
            "Entrance",
            "Next",
            "Exit"});
            this.EditWarpTypeComboBox.Location = new System.Drawing.Point(93, 27);
            this.EditWarpTypeComboBox.Name = "EditWarpTypeComboBox";
            this.EditWarpTypeComboBox.Size = new System.Drawing.Size(121, 25);
            this.EditWarpTypeComboBox.TabIndex = 1;
            // 
            // EditWarpPositionXLabel
            // 
            this.EditWarpPositionXLabel.AutoSize = true;
            this.EditWarpPositionXLabel.Location = new System.Drawing.Point(12, 70);
            this.EditWarpPositionXLabel.Name = "EditWarpPositionXLabel";
            this.EditWarpPositionXLabel.Size = new System.Drawing.Size(72, 19);
            this.EditWarpPositionXLabel.TabIndex = 2;
            this.EditWarpPositionXLabel.Text = "Position X:";
            // 
            // EditWarpPositionYLabel
            // 
            this.EditWarpPositionYLabel.AutoSize = true;
            this.EditWarpPositionYLabel.Location = new System.Drawing.Point(12, 110);
            this.EditWarpPositionYLabel.Name = "EditWarpPositionYLabel";
            this.EditWarpPositionYLabel.Size = new System.Drawing.Size(72, 19);
            this.EditWarpPositionYLabel.TabIndex = 3;
            this.EditWarpPositionYLabel.Text = "Position Y:";
            // 
            // WarpPositionXNumericUpDown
            // 
            this.WarpPositionXNumericUpDown.Location = new System.Drawing.Point(93, 70);
            this.WarpPositionXNumericUpDown.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.WarpPositionXNumericUpDown.Name = "WarpPositionXNumericUpDown";
            this.WarpPositionXNumericUpDown.Size = new System.Drawing.Size(61, 25);
            this.WarpPositionXNumericUpDown.TabIndex = 4;
            // 
            // WarpPositionYNumericUpDown
            // 
            this.WarpPositionYNumericUpDown.Location = new System.Drawing.Point(93, 110);
            this.WarpPositionYNumericUpDown.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.WarpPositionYNumericUpDown.Name = "WarpPositionYNumericUpDown";
            this.WarpPositionYNumericUpDown.Size = new System.Drawing.Size(61, 25);
            this.WarpPositionYNumericUpDown.TabIndex = 5;
            // 
            // SaveChangesButton
            // 
            this.SaveChangesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveChangesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.SaveChangesButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SaveChangesButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.SaveChangesButton.FlatAppearance.BorderSize = 3;
            this.SaveChangesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveChangesButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveChangesButton.ForeColor = System.Drawing.Color.White;
            this.SaveChangesButton.Location = new System.Drawing.Point(80, 155);
            this.SaveChangesButton.Name = "SaveChangesButton";
            this.SaveChangesButton.Size = new System.Drawing.Size(120, 40);
            this.SaveChangesButton.TabIndex = 66;
            this.SaveChangesButton.Text = "Save changes";
            this.SaveChangesButton.UseVisualStyleBackColor = false;
            // 
            // EditWarpWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(264, 211);
            this.Controls.Add(this.SaveChangesButton);
            this.Controls.Add(this.WarpPositionYNumericUpDown);
            this.Controls.Add(this.WarpPositionXNumericUpDown);
            this.Controls.Add(this.EditWarpPositionYLabel);
            this.Controls.Add(this.EditWarpPositionXLabel);
            this.Controls.Add(this.EditWarpTypeComboBox);
            this.Controls.Add(this.EditWarpTypeLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditWarpWindow";
            this.Text = "Edit warp";
            ((System.ComponentModel.ISupportInitialize)(this.WarpPositionXNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WarpPositionYNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EditWarpTypeLabel;
        private System.Windows.Forms.Label EditWarpPositionXLabel;
        private System.Windows.Forms.Label EditWarpPositionYLabel;
        internal System.Windows.Forms.ComboBox EditWarpTypeComboBox;
        internal System.Windows.Forms.NumericUpDown WarpPositionXNumericUpDown;
        internal System.Windows.Forms.NumericUpDown WarpPositionYNumericUpDown;
        private System.Windows.Forms.Button SaveChangesButton;
    }
}