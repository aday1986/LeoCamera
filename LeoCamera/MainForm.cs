using DevExpress.Office.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using Leo.Camera;
using Leo.Yolo;
using OpenCvSharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoloDotNet.Exceptions;
using YoloDotNet.ExecutionProvider.Cpu;
using YoloDotNet.ExecutionProvider.Cuda;
using YoloDotNet.ExecutionProvider.DirectML;
using YoloDotNet.ExecutionProvider.OpenVino;
using YoloDotNet.Extensions;
using YoloDotNet.Models;
using YoloDotNet.Models.Interfaces;
using YoloDotNet.Video;


namespace LeoCamera
{
    public partial class MainForm : Form
    {
        // 在 Form1 类中添加字段
        private Leo.Camera.LeoCamera? capture;
        private LeoYolo? yolo = null;
        private readonly Options options = new Options()
        {
            ExecutionProvider = ExecutionProviderType.DirectML
        };
        /// <summary>
        /// 
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;

        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            propertyGridControl1.SelectedObject = options;
            barButtonItem2.Enabled = false;
        }

        private void SetControlEnabled(bool isRunning)
        {
            barButtonItem1.Enabled = !isRunning;
            barButtonItem2.Enabled = isRunning;
            barButtonItem3.Enabled = !isRunning;
            propertyGridControl1.Enabled = !isRunning;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                yolo = new LeoYolo(options);
                capture = new Leo.Camera.LeoCamera() { Delay = options.Delay };
                capture.AfterStop += (s, args) =>
                {
                    var action = () =>
                    {
                        pictureEdit1.Image?.Dispose();
                        pictureEdit1.Image = null;
                        SetControlEnabled(false);
                    };
                    if (pictureEdit1.InvokeRequired)
                    {
                        pictureEdit1.Invoke(action);
                    }
                    else
                    {
                        action.Invoke();
                    }
                };
                capture.StartCamera(async (byte[] frameData) =>
                 {
                     try
                     {
                         frameData = yolo.Detect(frameData);
                         using (MemoryStream ms = new MemoryStream(frameData))
                         {
                             Image image = Image.FromStream(ms);
                             var action = () =>
                             {
                                 pictureEdit1.Image?.Dispose();
                                 pictureEdit1.Image = image;
                             };
                             if (pictureEdit1.InvokeRequired)
                             {
                                 pictureEdit1.Invoke(action);
                             }
                             else
                             {
                                 action.Invoke();
                             }
                         }
                     }
                     catch (Exception ex)
                     {
                         capture?.StopCamera();
                         // 捕获检测过程中的异常并显示
                         MessageBox.Show($"检测错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }
                 }, options.CameraId);
                SetControlEnabled(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                capture?.StopCamera();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 识别图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                yolo = new LeoYolo(options);
                var frameData = yolo.Detect(openFileDialog.FileName);
                using (MemoryStream ms = new MemoryStream(frameData))
                {
                    Image image = Image.FromStream(ms);
                    pictureEdit1.Image?.Dispose();
                    pictureEdit1.Image = image;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
