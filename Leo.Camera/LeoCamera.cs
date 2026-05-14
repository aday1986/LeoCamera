using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;

namespace Leo.Camera
{
    public class LeoCamera
    {
        private VideoCapture? capture;
        private CancellationTokenSource? cancelToken;
        private Task? cameraTask;

        public LeoCamera()
        {

        }

        /// <summary>
        /// 启动摄像头并在每帧图像上执行指定的操作。操作以OpenCV的Mat对象形式接收图像数据。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cameraIndex"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        public void StartCamera(Action<byte[]> action, int cameraIndex = 0)
        {
            if (capture != null && capture.IsOpened())
            {
                throw new InvalidOperationException("Camera is already running.");
            }
            capture = new VideoCapture(cameraIndex);
            if (!capture.IsOpened())
            {
                throw new Exception($"Failed to open camera with index {cameraIndex}.");
            }
            cancelToken = new CancellationTokenSource();
            var token = cancelToken.Token;
            cameraTask = Task.Run(() =>
            {
                using var mat = new Mat();
                while (!token.IsCancellationRequested && capture != null && capture.IsOpened())
                {
                    try
                    {
                        capture.Read(mat);
                        action?.Invoke(mat.ToBytes());
                        Task.Delay(33);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                AfterStop?.Invoke(this, new EventArgs());
            }, token);
        }

        public event EventHandler AfterStop;

        /// <summary>
        /// 关闭摄像头并停止图像处理任务，释放相关资源。
        /// </summary>
        public void StopCamera()
        {
            capture?.Release();
            cancelToken?.Cancel();
        }
    }
}
