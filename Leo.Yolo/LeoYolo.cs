using SkiaSharp;
using System.ComponentModel;
using YoloDotNet.Extensions;
using YoloDotNet.Models;
using YoloDotNet.Models.Interfaces;


namespace Leo.Yolo
{

    public class LeoYolo : IDisposable
    {
        private static PoseDrawingOptions? drawingOptions = null;
        private readonly LeoYoloOptions options;
        private readonly YoloDotNet.Yolo? yolo = null;
        private readonly ITracker? tracker = null;

        public LeoYolo(LeoYoloOptions options)
        {
            IExecutionProvider? extenderProvider = null;
            var modelPath = options.ModelPath;
            if (modelPath is null || modelPath == string.Empty)
            {
                modelPath = $"./models/yolo{GetEnumDescription(options.ModelVision).Trim('v')}{GetEnumDescription(options.ModelSize)}{("-" + GetEnumDescription(options.ModelType).Replace("object", "")).TrimEnd('-')}.onnx";
            }
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"Model file not found at path: {modelPath}");
            }
            switch (options.ExecutionProvider)
            {
                case ExecutionProviderType.CPU:
                    extenderProvider = new YoloDotNet.ExecutionProvider.Cpu.CpuExecutionProvider(modelPath);
                    break;
                case ExecutionProviderType.CUDA:
                    extenderProvider = new YoloDotNet.ExecutionProvider.Cuda.CudaExecutionProvider(modelPath, options.GpuId);
                    break;
                case ExecutionProviderType.DirectML:
                    extenderProvider = new YoloDotNet.ExecutionProvider.DirectML.DirectMLExecutionProvider(modelPath, options.GpuId);
                    break;
                case ExecutionProviderType.OpenVINO:
                    extenderProvider = new YoloDotNet.ExecutionProvider.OpenVino.OpenVinoExecutionProvider(modelPath);
                    break;
                case ExecutionProviderType.CoreML:
                    extenderProvider = new YoloDotNet.ExecutionProvider.CoreML.CoreMLExecutionProvider(modelPath);
                    break;
                default:
                    throw new NotSupportedException($"Execution provider {options.ExecutionProvider} is not supported.");
            }
            yolo = new YoloDotNet.Yolo(new YoloOptions()
            {
                ExecutionProvider = extenderProvider,
            });
            switch (options.TrackerType)
            {
                case TrackerTypeEnum.None:
                    tracker = null;
                    break;
                case TrackerTypeEnum.Sort:
                    tracker = new BaseSortTracker(options.CostThreshold, options.MaxAge, options.TailLength);
                    break;
                case TrackerTypeEnum.ByteTrack:
                    throw new Exception("ByteTrack tracker is currently not supported.");
                case TrackerTypeEnum.DeepSort:
                    throw new Exception("DeepSort tracker is currently not supported.");
                default:
                    tracker = null;
                    break;
            }

            this.options = options;
        }

        public byte[] Detect(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"Image file not found at path: {imagePath}");
            }
            var imageData = File.ReadAllBytes(imagePath);
            return Detect(imageData);
        }

        public byte[] Detect(byte[] image)
        {
            if (yolo == null)
            {
                throw new InvalidOperationException("Yolo model is not initialized.");
            }
            var skBitmap = SKBitmap.Decode(image);
            switch (options.ModelType)
            {
                case ModelType.Object:
                    var objResults = yolo.RunObjectDetection(skBitmap, options.Confidence, options.Iou);
                    if (tracker != null)
                    {
                        objResults.Track(tracker);
                    }
                    skBitmap.Draw(objResults);
                    break;
                case ModelType.Obb:
                    var obbResults = yolo.RunObbDetection(skBitmap, options.Confidence, options.Iou);
                    if (tracker != null)
                    {
                        obbResults.Track(tracker);
                    }
                    skBitmap.Draw(obbResults);
                    break;
                case ModelType.Pose:
                    var poseResults = yolo.RunPoseEstimation(skBitmap, options.Confidence, options.Iou);
                    skBitmap.Draw(poseResults, drawingOptions!);
                    break;
                case ModelType.Seg:
                    var segResults = yolo.RunSegmentation(skBitmap, options.Confidence);
                    skBitmap.Draw(segResults);
                    break;
                case ModelType.Cls:
                    var clsResults = yolo.RunClassification(skBitmap);
                    skBitmap.Draw(clsResults);
                    break;
                default:
                    throw new NotSupportedException($"Model type {options.ModelType} is not supported.");
            }
            using var data = skBitmap.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }

        static LeoYolo()
        {
            SetDrawingOptions();
        }

        private static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttribute = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return descriptionAttribute != null && descriptionAttribute.Length > 0 ? descriptionAttribute[0].Description : value.ToString();
        }

        private static void SetDrawingOptions()
        {
            drawingOptions = new PoseDrawingOptions
            {
                DrawBoundingBoxes = true,
                DrawConfidenceScore = true,
                DrawLabels = true,
                EnableFontShadow = true,
                Font = SKTypeface.Default,
                FontSize = 18,
                FontColor = SKColors.White,
                DrawLabelBackground = true,
                EnableDynamicScaling = true,
                BorderThickness = 2,
                BoundingBoxOpacity = 128,
                KeyPointMarkers = CustomKeyPointColorMap.KeyPoints,
                PoseConfidence = 0.65f
            };
        }

        public void Dispose()
        {
            if (yolo != null)
            {
                yolo.Dispose();
            }
        }
    }
}


