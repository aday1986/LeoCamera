using Leo.Yolo;


namespace LeoCamera
{
    public partial class MainForm : Form
    {
        // 在 Form1 类中添加字段
        private Leo.Camera.LeoCamera? capture;
        private LeoYolo? yolo = null;
        private Options? options = null;
        /// <summary>
        /// 
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            LoadOptions();
            propertyGrid1.SelectedObject = options;
            propertyGrid1.PropertyValueChanged += (s, args) =>
            {
                SaveOptions();
            };
            btnStart.Click += barButtonItem1_ItemClick;
            btnStop.Click += barButtonItem2_ItemClick;
            btnImage.Click += barButtonItem3_ItemClick;

        }

        private void LoadOptions()
        {
            try
            {
                if (File.Exists("config.json"))
                {
                    var json = File.ReadAllText("config.json");
                    options = System.Text.Json.JsonSerializer.Deserialize<Options>(json) ?? new Options();
                }
                else
                {
                    options = new Options();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void SaveOptions()
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(options, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("config.json", json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            SetControlEnabled(false);
        }

        private void SetControlEnabled(bool isRunning)
        {
            btnStart.Enabled = !isRunning;
            btnStop.Enabled = isRunning;
            btnImage.Enabled = !isRunning;
            propertyGrid1.Enabled = !isRunning;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object? sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                yolo = new LeoYolo(options);

                capture = new Leo.Camera.LeoCamera() { Delay = options.Delay };
                capture.AfterStop += (s, args) =>
                {
                    var action = () =>
                    {
                        pictureBox1.Image?.Dispose();
                        pictureBox1.Image = null;
                        SetControlEnabled(false);
                    };
                    if (pictureBox1.InvokeRequired)
                    {
                        pictureBox1.Invoke(action);
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
                                 pictureBox1.Image?.Dispose();
                                 pictureBox1.Image = image;
                             };
                             if (pictureBox1.InvokeRequired)
                             {
                                 pictureBox1.Invoke(action);
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void barButtonItem2_ItemClick(object? sender, EventArgs e)
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
        private void barButtonItem3_ItemClick(object? sender, EventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                yolo = new LeoYolo(options);
                var frameData = yolo.Detect(openFileDialog.FileName);
                using (MemoryStream ms = new MemoryStream(frameData))
                {
                    Image image = Image.FromStream(ms);
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = image;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
