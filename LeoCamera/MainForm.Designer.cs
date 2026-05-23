namespace LeoCamera
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            propertyGrid1 = new PropertyGrid();
            splitContainer1 = new SplitContainer();
            pictureBox1 = new PictureBox();
            toolStrip1 = new ToolStrip();
            btnStart = new ToolStripButton();
            btnStop = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnImage = new ToolStripButton();
            btnReset = new ToolStripButton();
            statusStrip1 = new StatusStrip();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // propertyGrid1
            // 
            propertyGrid1.BackColor = SystemColors.Control;
            propertyGrid1.Dock = DockStyle.Fill;
            propertyGrid1.Location = new Point(0, 0);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.Size = new Size(377, 761);
            propertyGrid1.TabIndex = 7;
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.Location = new Point(0, 38);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(propertyGrid1);
            splitContainer1.Size = new Size(1283, 763);
            splitContainer1.SplitterDistance = 900;
            splitContainer1.TabIndex = 9;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(898, 761);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(28, 28);
            toolStrip1.Items.AddRange(new ToolStripItem[] { btnStart, btnStop, toolStripSeparator1, btnImage, btnReset });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1283, 38);
            toolStrip1.TabIndex = 11;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnStart
            // 
            btnStart.Image = (Image)resources.GetObject("btnStart.Image");
            btnStart.ImageTransparentColor = Color.Magenta;
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(86, 32);
            btnStart.Text = "开启";
            // 
            // btnStop
            // 
            btnStop.Image = (Image)resources.GetObject("btnStop.Image");
            btnStop.ImageTransparentColor = Color.Magenta;
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(86, 32);
            btnStop.Text = "停止";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 38);
            // 
            // btnImage
            // 
            btnImage.Image = (Image)resources.GetObject("btnImage.Image");
            btnImage.ImageTransparentColor = Color.Magenta;
            btnImage.Name = "btnImage";
            btnImage.Size = new Size(128, 32);
            btnImage.Text = "选择图片";
            // 
            // btnReset
            // 
            btnReset.Alignment = ToolStripItemAlignment.Right;
            btnReset.Image = (Image)resources.GetObject("btnReset.Image");
            btnReset.ImageTransparentColor = Color.Magenta;
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(128, 32);
            btnReset.Text = "重置配置";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(28, 28);
            statusStrip1.Location = new Point(0, 801);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1283, 22);
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "statusStrip1";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1283, 823);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            Name = "MainForm";
            Text = "摄像头识别";
            WindowState = FormWindowState.Maximized;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PropertyGrid propertyGrid1;
        private SplitContainer splitContainer1;
        private PictureBox pictureBox1;
        private ToolStrip toolStrip1;
        private ToolStripButton btnStart;
        private ToolStripButton btnStop;
        private ToolStripButton btnImage;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btnReset;
        private StatusStrip statusStrip1;
    }
}