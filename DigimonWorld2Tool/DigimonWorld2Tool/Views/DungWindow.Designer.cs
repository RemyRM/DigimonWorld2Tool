
namespace DigimonWorld2Tool.Views
{
    partial class DungWindow
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
            this.SelectDungDirButton = new System.Windows.Forms.Button();
            this.DungDirLabel = new System.Windows.Forms.Label();
            this.FloorLayoutPictureBox = new System.Windows.Forms.PictureBox();
            this.SelectDungLabel = new System.Windows.Forms.Label();
            this.SelectDungComboBox = new System.Windows.Forms.ComboBox();
            this.SelectDungFloorLabel = new System.Windows.Forms.Label();
            this.SelectDungFloorComboBox = new System.Windows.Forms.ComboBox();
            this.FloorLayoutButton1 = new System.Windows.Forms.Button();
            this.FloorLayoutButton2 = new System.Windows.Forms.Button();
            this.FloorLayoutButton3 = new System.Windows.Forms.Button();
            this.FloorLayoutButton4 = new System.Windows.Forms.Button();
            this.FloorLayoutButton5 = new System.Windows.Forms.Button();
            this.FloorLayoutButton6 = new System.Windows.Forms.Button();
            this.FloorLayoutButton7 = new System.Windows.Forms.Button();
            this.FloorLayoutButton8 = new System.Windows.Forms.Button();
            this.DrawGridCheckBox = new System.Windows.Forms.CheckBox();
            this.MousePositionLabel = new System.Windows.Forms.Label();
            this.ObjectInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.SlotFourLabel = new System.Windows.Forms.Label();
            this.SlotThreeLabel = new System.Windows.Forms.Label();
            this.SlotTwoLabel = new System.Windows.Forms.Label();
            this.SlotOneLabel = new System.Windows.Forms.Label();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.SubTypeLabel = new System.Windows.Forms.Label();
            this.TypeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FloorLayoutPictureBox)).BeginInit();
            this.ObjectInfoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectDungDirButton
            // 
            this.SelectDungDirButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.SelectDungDirButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.SelectDungDirButton.FlatAppearance.BorderSize = 3;
            this.SelectDungDirButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectDungDirButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SelectDungDirButton.ForeColor = System.Drawing.Color.White;
            this.SelectDungDirButton.Location = new System.Drawing.Point(10, 10);
            this.SelectDungDirButton.Name = "SelectDungDirButton";
            this.SelectDungDirButton.Size = new System.Drawing.Size(175, 40);
            this.SelectDungDirButton.TabIndex = 1;
            this.SelectDungDirButton.Text = "Select DUNG directory";
            this.SelectDungDirButton.UseVisualStyleBackColor = false;
            this.SelectDungDirButton.Click += new System.EventHandler(this.SelectDungDirButton_Click);
            // 
            // DungDirLabel
            // 
            this.DungDirLabel.AutoSize = true;
            this.DungDirLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DungDirLabel.ForeColor = System.Drawing.Color.White;
            this.DungDirLabel.Location = new System.Drawing.Point(195, 20);
            this.DungDirLabel.Name = "DungDirLabel";
            this.DungDirLabel.Size = new System.Drawing.Size(138, 19);
            this.DungDirLabel.TabIndex = 2;
            this.DungDirLabel.Text = "No directory selected";
            // 
            // FloorLayoutPictureBox
            // 
            this.FloorLayoutPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FloorLayoutPictureBox.BackColor = System.Drawing.Color.Black;
            this.FloorLayoutPictureBox.Location = new System.Drawing.Point(10, 100);
            this.FloorLayoutPictureBox.MinimumSize = new System.Drawing.Size(640, 480);
            this.FloorLayoutPictureBox.Name = "FloorLayoutPictureBox";
            this.FloorLayoutPictureBox.Size = new System.Drawing.Size(733, 550);
            this.FloorLayoutPictureBox.TabIndex = 3;
            this.FloorLayoutPictureBox.TabStop = false;
            this.FloorLayoutPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FloorLayoutPictureBox_MouseClick);
            this.FloorLayoutPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DisplayMousePositionOnGrid);
            // 
            // SelectDungLabel
            // 
            this.SelectDungLabel.AutoSize = true;
            this.SelectDungLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SelectDungLabel.ForeColor = System.Drawing.Color.White;
            this.SelectDungLabel.Location = new System.Drawing.Point(10, 63);
            this.SelectDungLabel.Name = "SelectDungLabel";
            this.SelectDungLabel.Size = new System.Drawing.Size(106, 19);
            this.SelectDungLabel.TabIndex = 4;
            this.SelectDungLabel.Text = "Select dungeon:";
            // 
            // SelectDungComboBox
            // 
            this.SelectDungComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectDungComboBox.FormattingEnabled = true;
            this.SelectDungComboBox.Location = new System.Drawing.Point(122, 62);
            this.SelectDungComboBox.Name = "SelectDungComboBox";
            this.SelectDungComboBox.Size = new System.Drawing.Size(121, 23);
            this.SelectDungComboBox.TabIndex = 5;
            this.SelectDungComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectDungComboBox_SelectedIndexChanged);
            // 
            // SelectDungFloorLabel
            // 
            this.SelectDungFloorLabel.AutoSize = true;
            this.SelectDungFloorLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SelectDungFloorLabel.ForeColor = System.Drawing.Color.White;
            this.SelectDungFloorLabel.Location = new System.Drawing.Point(264, 63);
            this.SelectDungFloorLabel.Name = "SelectDungFloorLabel";
            this.SelectDungFloorLabel.Size = new System.Drawing.Size(79, 19);
            this.SelectDungFloorLabel.TabIndex = 6;
            this.SelectDungFloorLabel.Text = "Select floor:";
            // 
            // SelectDungFloorComboBox
            // 
            this.SelectDungFloorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectDungFloorComboBox.FormattingEnabled = true;
            this.SelectDungFloorComboBox.Location = new System.Drawing.Point(349, 63);
            this.SelectDungFloorComboBox.Name = "SelectDungFloorComboBox";
            this.SelectDungFloorComboBox.Size = new System.Drawing.Size(121, 23);
            this.SelectDungFloorComboBox.TabIndex = 7;
            this.SelectDungFloorComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectDungFloorComboBox_SelectedIndexChanged);
            // 
            // FloorLayoutButton1
            // 
            this.FloorLayoutButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FloorLayoutButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.FloorLayoutButton1.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.FloorLayoutButton1.FlatAppearance.BorderSize = 3;
            this.FloorLayoutButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FloorLayoutButton1.ForeColor = System.Drawing.Color.White;
            this.FloorLayoutButton1.Location = new System.Drawing.Point(749, 100);
            this.FloorLayoutButton1.Name = "FloorLayoutButton1";
            this.FloorLayoutButton1.Size = new System.Drawing.Size(22, 62);
            this.FloorLayoutButton1.TabIndex = 8;
            this.FloorLayoutButton1.Text = "1";
            this.FloorLayoutButton1.UseVisualStyleBackColor = false;
            this.FloorLayoutButton1.Click += new System.EventHandler(this.FloorLayoutButton_Click);
            // 
            // FloorLayoutButton2
            // 
            this.FloorLayoutButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FloorLayoutButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.FloorLayoutButton2.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.FloorLayoutButton2.FlatAppearance.BorderSize = 3;
            this.FloorLayoutButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FloorLayoutButton2.ForeColor = System.Drawing.Color.White;
            this.FloorLayoutButton2.Location = new System.Drawing.Point(749, 168);
            this.FloorLayoutButton2.Name = "FloorLayoutButton2";
            this.FloorLayoutButton2.Size = new System.Drawing.Size(22, 62);
            this.FloorLayoutButton2.TabIndex = 9;
            this.FloorLayoutButton2.Text = "2";
            this.FloorLayoutButton2.UseVisualStyleBackColor = false;
            this.FloorLayoutButton2.Click += new System.EventHandler(this.FloorLayoutButton_Click);
            // 
            // FloorLayoutButton3
            // 
            this.FloorLayoutButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FloorLayoutButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.FloorLayoutButton3.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.FloorLayoutButton3.FlatAppearance.BorderSize = 3;
            this.FloorLayoutButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FloorLayoutButton3.ForeColor = System.Drawing.Color.White;
            this.FloorLayoutButton3.Location = new System.Drawing.Point(749, 236);
            this.FloorLayoutButton3.Name = "FloorLayoutButton3";
            this.FloorLayoutButton3.Size = new System.Drawing.Size(22, 62);
            this.FloorLayoutButton3.TabIndex = 10;
            this.FloorLayoutButton3.Text = "3";
            this.FloorLayoutButton3.UseVisualStyleBackColor = false;
            this.FloorLayoutButton3.Click += new System.EventHandler(this.FloorLayoutButton_Click);
            // 
            // FloorLayoutButton4
            // 
            this.FloorLayoutButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FloorLayoutButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.FloorLayoutButton4.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.FloorLayoutButton4.FlatAppearance.BorderSize = 3;
            this.FloorLayoutButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FloorLayoutButton4.ForeColor = System.Drawing.Color.White;
            this.FloorLayoutButton4.Location = new System.Drawing.Point(749, 304);
            this.FloorLayoutButton4.Name = "FloorLayoutButton4";
            this.FloorLayoutButton4.Size = new System.Drawing.Size(22, 62);
            this.FloorLayoutButton4.TabIndex = 11;
            this.FloorLayoutButton4.Text = "4";
            this.FloorLayoutButton4.UseVisualStyleBackColor = false;
            this.FloorLayoutButton4.Click += new System.EventHandler(this.FloorLayoutButton_Click);
            // 
            // FloorLayoutButton5
            // 
            this.FloorLayoutButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FloorLayoutButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.FloorLayoutButton5.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.FloorLayoutButton5.FlatAppearance.BorderSize = 3;
            this.FloorLayoutButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FloorLayoutButton5.ForeColor = System.Drawing.Color.White;
            this.FloorLayoutButton5.Location = new System.Drawing.Point(749, 372);
            this.FloorLayoutButton5.Name = "FloorLayoutButton5";
            this.FloorLayoutButton5.Size = new System.Drawing.Size(22, 62);
            this.FloorLayoutButton5.TabIndex = 12;
            this.FloorLayoutButton5.Text = "5";
            this.FloorLayoutButton5.UseVisualStyleBackColor = false;
            this.FloorLayoutButton5.Click += new System.EventHandler(this.FloorLayoutButton_Click);
            // 
            // FloorLayoutButton6
            // 
            this.FloorLayoutButton6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FloorLayoutButton6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.FloorLayoutButton6.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.FloorLayoutButton6.FlatAppearance.BorderSize = 3;
            this.FloorLayoutButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FloorLayoutButton6.ForeColor = System.Drawing.Color.White;
            this.FloorLayoutButton6.Location = new System.Drawing.Point(749, 440);
            this.FloorLayoutButton6.Name = "FloorLayoutButton6";
            this.FloorLayoutButton6.Size = new System.Drawing.Size(22, 62);
            this.FloorLayoutButton6.TabIndex = 13;
            this.FloorLayoutButton6.Text = "6";
            this.FloorLayoutButton6.UseVisualStyleBackColor = false;
            this.FloorLayoutButton6.Click += new System.EventHandler(this.FloorLayoutButton_Click);
            // 
            // FloorLayoutButton7
            // 
            this.FloorLayoutButton7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FloorLayoutButton7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.FloorLayoutButton7.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.FloorLayoutButton7.FlatAppearance.BorderSize = 3;
            this.FloorLayoutButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FloorLayoutButton7.ForeColor = System.Drawing.Color.White;
            this.FloorLayoutButton7.Location = new System.Drawing.Point(749, 508);
            this.FloorLayoutButton7.Name = "FloorLayoutButton7";
            this.FloorLayoutButton7.Size = new System.Drawing.Size(22, 62);
            this.FloorLayoutButton7.TabIndex = 14;
            this.FloorLayoutButton7.Text = "7";
            this.FloorLayoutButton7.UseVisualStyleBackColor = false;
            this.FloorLayoutButton7.Click += new System.EventHandler(this.FloorLayoutButton_Click);
            // 
            // FloorLayoutButton8
            // 
            this.FloorLayoutButton8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FloorLayoutButton8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.FloorLayoutButton8.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.FloorLayoutButton8.FlatAppearance.BorderSize = 3;
            this.FloorLayoutButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FloorLayoutButton8.ForeColor = System.Drawing.Color.White;
            this.FloorLayoutButton8.Location = new System.Drawing.Point(749, 576);
            this.FloorLayoutButton8.Name = "FloorLayoutButton8";
            this.FloorLayoutButton8.Size = new System.Drawing.Size(22, 62);
            this.FloorLayoutButton8.TabIndex = 15;
            this.FloorLayoutButton8.Text = "8";
            this.FloorLayoutButton8.UseVisualStyleBackColor = false;
            this.FloorLayoutButton8.Click += new System.EventHandler(this.FloorLayoutButton_Click);
            // 
            // DrawGridCheckBox
            // 
            this.DrawGridCheckBox.AutoSize = true;
            this.DrawGridCheckBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DrawGridCheckBox.ForeColor = System.Drawing.Color.White;
            this.DrawGridCheckBox.Location = new System.Drawing.Point(562, 64);
            this.DrawGridCheckBox.Name = "DrawGridCheckBox";
            this.DrawGridCheckBox.Size = new System.Drawing.Size(88, 23);
            this.DrawGridCheckBox.TabIndex = 16;
            this.DrawGridCheckBox.Text = "Draw grid";
            this.DrawGridCheckBox.UseVisualStyleBackColor = true;
            this.DrawGridCheckBox.CheckedChanged += new System.EventHandler(this.DrawGridCheckBox_CheckedChanged);
            // 
            // MousePositionLabel
            // 
            this.MousePositionLabel.AutoSize = true;
            this.MousePositionLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MousePositionLabel.ForeColor = System.Drawing.Color.White;
            this.MousePositionLabel.Location = new System.Drawing.Point(482, 65);
            this.MousePositionLabel.Name = "MousePositionLabel";
            this.MousePositionLabel.Size = new System.Drawing.Size(67, 19);
            this.MousePositionLabel.TabIndex = 17;
            this.MousePositionLabel.Text = "X:00 Y:00";
            // 
            // ObjectInfoGroupBox
            // 
            this.ObjectInfoGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectInfoGroupBox.Controls.Add(this.SlotFourLabel);
            this.ObjectInfoGroupBox.Controls.Add(this.SlotThreeLabel);
            this.ObjectInfoGroupBox.Controls.Add(this.SlotTwoLabel);
            this.ObjectInfoGroupBox.Controls.Add(this.SlotOneLabel);
            this.ObjectInfoGroupBox.Controls.Add(this.PositionLabel);
            this.ObjectInfoGroupBox.Controls.Add(this.SubTypeLabel);
            this.ObjectInfoGroupBox.Controls.Add(this.TypeLabel);
            this.ObjectInfoGroupBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ObjectInfoGroupBox.ForeColor = System.Drawing.Color.White;
            this.ObjectInfoGroupBox.Location = new System.Drawing.Point(793, 401);
            this.ObjectInfoGroupBox.Name = "ObjectInfoGroupBox";
            this.ObjectInfoGroupBox.Size = new System.Drawing.Size(293, 249);
            this.ObjectInfoGroupBox.TabIndex = 18;
            this.ObjectInfoGroupBox.TabStop = false;
            this.ObjectInfoGroupBox.Text = "Object info";
            // 
            // SlotFourLabel
            // 
            this.SlotFourLabel.AutoSize = true;
            this.SlotFourLabel.Location = new System.Drawing.Point(6, 213);
            this.SlotFourLabel.Name = "SlotFourLabel";
            this.SlotFourLabel.Size = new System.Drawing.Size(44, 19);
            this.SlotFourLabel.TabIndex = 6;
            this.SlotFourLabel.Text = "Slot 4";
            // 
            // SlotThreeLabel
            // 
            this.SlotThreeLabel.AutoSize = true;
            this.SlotThreeLabel.Location = new System.Drawing.Point(6, 176);
            this.SlotThreeLabel.Name = "SlotThreeLabel";
            this.SlotThreeLabel.Size = new System.Drawing.Size(44, 19);
            this.SlotThreeLabel.TabIndex = 5;
            this.SlotThreeLabel.Text = "Slot 3";
            // 
            // SlotTwoLabel
            // 
            this.SlotTwoLabel.AutoSize = true;
            this.SlotTwoLabel.Location = new System.Drawing.Point(6, 139);
            this.SlotTwoLabel.Name = "SlotTwoLabel";
            this.SlotTwoLabel.Size = new System.Drawing.Size(44, 19);
            this.SlotTwoLabel.TabIndex = 4;
            this.SlotTwoLabel.Text = "Slot 2";
            // 
            // SlotOneLabel
            // 
            this.SlotOneLabel.AutoSize = true;
            this.SlotOneLabel.Location = new System.Drawing.Point(7, 99);
            this.SlotOneLabel.Name = "SlotOneLabel";
            this.SlotOneLabel.Size = new System.Drawing.Size(44, 19);
            this.SlotOneLabel.TabIndex = 3;
            this.SlotOneLabel.Text = "Slot 1";
            // 
            // PositionLabel
            // 
            this.PositionLabel.AutoSize = true;
            this.PositionLabel.Location = new System.Drawing.Point(6, 64);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(57, 19);
            this.PositionLabel.TabIndex = 2;
            this.PositionLabel.Text = "Position";
            // 
            // SubTypeLabel
            // 
            this.SubTypeLabel.AutoSize = true;
            this.SubTypeLabel.Location = new System.Drawing.Point(147, 33);
            this.SubTypeLabel.Name = "SubTypeLabel";
            this.SubTypeLabel.Size = new System.Drawing.Size(63, 19);
            this.SubTypeLabel.TabIndex = 1;
            this.SubTypeLabel.Text = "Sub type";
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(6, 30);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(37, 19);
            this.TypeLabel.TabIndex = 0;
            this.TypeLabel.Text = "Type";
            // 
            // DungWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.Controls.Add(this.ObjectInfoGroupBox);
            this.Controls.Add(this.MousePositionLabel);
            this.Controls.Add(this.DrawGridCheckBox);
            this.Controls.Add(this.FloorLayoutButton8);
            this.Controls.Add(this.FloorLayoutButton7);
            this.Controls.Add(this.FloorLayoutButton6);
            this.Controls.Add(this.FloorLayoutButton5);
            this.Controls.Add(this.FloorLayoutButton4);
            this.Controls.Add(this.FloorLayoutButton3);
            this.Controls.Add(this.FloorLayoutButton2);
            this.Controls.Add(this.FloorLayoutButton1);
            this.Controls.Add(this.SelectDungFloorComboBox);
            this.Controls.Add(this.SelectDungFloorLabel);
            this.Controls.Add(this.SelectDungComboBox);
            this.Controls.Add(this.SelectDungLabel);
            this.Controls.Add(this.FloorLayoutPictureBox);
            this.Controls.Add(this.DungDirLabel);
            this.Controls.Add(this.SelectDungDirButton);
            this.MinimumSize = new System.Drawing.Size(1100, 660);
            this.Name = "DungWindow";
            this.Size = new System.Drawing.Size(1100, 660);
            ((System.ComponentModel.ISupportInitialize)(this.FloorLayoutPictureBox)).EndInit();
            this.ObjectInfoGroupBox.ResumeLayout(false);
            this.ObjectInfoGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button SelectDungDirButton;
        private System.Windows.Forms.Label DungDirLabel;
        private System.Windows.Forms.PictureBox FloorLayoutPictureBox;
        private System.Windows.Forms.Label SelectDungLabel;
        private System.Windows.Forms.ComboBox SelectDungComboBox;
        private System.Windows.Forms.Label SelectDungFloorLabel;
        private System.Windows.Forms.ComboBox SelectDungFloorComboBox;
        private System.Windows.Forms.Button FloorLayoutButton1;
        private System.Windows.Forms.Button FloorLayoutButton2;
        private System.Windows.Forms.Button FloorLayoutButton3;
        private System.Windows.Forms.Button FloorLayoutButton4;
        private System.Windows.Forms.Button FloorLayoutButton5;
        private System.Windows.Forms.Button FloorLayoutButton6;
        private System.Windows.Forms.Button FloorLayoutButton7;
        private System.Windows.Forms.Button FloorLayoutButton8;
        internal System.Windows.Forms.CheckBox DrawGridCheckBox;
        private System.Windows.Forms.Label MousePositionLabel;
        private System.Windows.Forms.GroupBox ObjectInfoGroupBox;
        private System.Windows.Forms.Label SlotFourLabel;
        private System.Windows.Forms.Label SlotThreeLabel;
        private System.Windows.Forms.Label SlotTwoLabel;
        private System.Windows.Forms.Label SlotOneLabel;
        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.Label SubTypeLabel;
        private System.Windows.Forms.Label TypeLabel;
    }
}
