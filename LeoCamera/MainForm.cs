using DevExpress.Office.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using Leo.Camera;
using Leo.Yolo;
using OpenCvSharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace LeoCamera
{
    public partial class MainForm : Form
    {
        // 在 Form1 类中添加字段
        private Leo.Camera.LeoCamera? capture;
        private LeoYolo? yolo = null;
        private readonly Options options = new  Options()
        {
            ExecutionProvider = ExecutionProviderType.DirectML
        };
        /// <summary>
        /// 
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            propertyGridControl1.SelectedObject = options;
            barButtonItem2.Enabled = false;

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

                // 初始化模型
                yolo = new LeoYolo(options);
                // 打开默认摄像头（索引0）
                capture = new Leo.Camera.LeoCamera() { Delay=options.Delay };
                capture.AfterStop += (s, args) =>
                {
                    var action = () =>
                    {
                        pictureEdit1.Image?.Dispose();
                        pictureEdit1.Image = null;
                        barButtonItem1.Enabled = true;
                        barButtonItem2.Enabled = false;
                        propertyGridControl1.Enabled = true;
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
                         // 进行检测
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
                         // 捕获检测过程中的异常并显示
                         MessageBox.Show($"检测错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }
                 });

                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = true;
                propertyGridControl1.Enabled = false;
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class  Options:LeoYoloOptions
    {
        [DefaultValue(30)]
        [Category("基础配置")]
        [DisplayName("延迟")]
        [Description("延迟时间，单位为毫秒，默认值为30ms。这个属性可以用来控制每帧图像处理之间的时间间隔，以避免过度占用系统资源或实现特定的帧率要求。")]
        public int Delay { get; set; } = 30;
    }
}
