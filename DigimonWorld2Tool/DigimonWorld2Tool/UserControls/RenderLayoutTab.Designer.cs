
namespace DigimonWorld2Tool.UserControls
{
    partial class RenderLayoutTab
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
            this.RenderLayoutPanel = new System.Windows.Forms.Panel();
            this.GridRenderLayer = new System.Windows.Forms.PictureBox();
            this.MapRenderLayer = new System.Windows.Forms.PictureBox();
            this.RenderLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridRenderLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapRenderLayer)).BeginInit();
            this.SuspendLayout();
            // 
            // RenderLayoutPanel
            // 
            this.RenderLayoutPanel.AutoScroll = true;
            this.RenderLayoutPanel.Controls.Add(this.GridRenderLayer);
            this.RenderLayoutPanel.Controls.Add(this.MapRenderLayer);
            this.RenderLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.RenderLayoutPanel.Name = "RenderLayoutPanel";
            this.RenderLayoutPanel.Size = new System.Drawing.Size(645, 485);
            this.RenderLayoutPanel.TabIndex = 0;
            // 
            // GridRenderLayer
            // 
            this.GridRenderLayer.BackColor = System.Drawing.Color.Transparent;
            this.GridRenderLayer.Location = new System.Drawing.Point(0, 0);
            this.GridRenderLayer.Name = "GridRenderLayer";
            this.GridRenderLayer.Size = new System.Drawing.Size(645, 485);
            this.GridRenderLayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.GridRenderLayer.TabIndex = 1;
            this.GridRenderLayer.TabStop = false;
            // 
            // MapRenderLayer
            // 
            this.MapRenderLayer.BackColor = System.Drawing.Color.Transparent;
            this.MapRenderLayer.Location = new System.Drawing.Point(0, 0);
            this.MapRenderLayer.Name = "MapRenderLayer";
            this.MapRenderLayer.Size = new System.Drawing.Size(645, 485);
            this.MapRenderLayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.MapRenderLayer.TabIndex = 0;
            this.MapRenderLayer.TabStop = false;
            // 
            // RenderLayoutTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.RenderLayoutPanel);
            this.Name = "RenderLayoutTab";
            this.Size = new System.Drawing.Size(645, 485);
            this.RenderLayoutPanel.ResumeLayout(false);
            this.RenderLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridRenderLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapRenderLayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Panel RenderLayoutPanel;
        public System.Windows.Forms.PictureBox GridRenderLayer;
        public System.Windows.Forms.PictureBox MapRenderLayer;
    }
}
