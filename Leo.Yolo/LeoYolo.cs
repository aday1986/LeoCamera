using SkiaSharp;
using System.ComponentModel;
using System.Drawing;
using YoloDotNet;
using YoloDotNet.Extensions;
using YoloDotNet.Models;
using YoloDotNet.Models.Interfaces;
using YoloDotNet.Trackers;


namespace Leo.Yolo
{

    public class LeoYolo : IDisposable
    {
        private static PoseDrawingOptions? _drawingOptions = null;
        private readonly LeoYoloOptions options;
        private YoloDotNet.Yolo? yolo = null;
        private static SortTracker _sortTracker = default!;

        public LeoYolo(LeoYoloOptions options)
        {
            IExecutionProvider? extenderProvider = null;
            _sortTracker = new SortTracker(0.7f, 10, 30);
            var modelPath = options.ModelPath ?? $"./models/yolo{GetEnumDescription(options.ModelVision).Trim('v')}{GetEnumDescription(options.ModelSize)}{("-" + GetEnumDescription(options.ModelType).Replace("object", "")).TrimEnd('-')}.onnx";
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"Model file not found at path: {modelPath}");
            }
            switch (options.ExecutionProvider)
            {
                case ExecutionProviderType.CPU:
                    extenderProvider = new YoloDotNet.ExecutionProvider.Cpu.CpuExecutionProvider(modelPath) ;
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

            this.options = options;
        }


        private static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttribute = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return descriptionAttribute != null && descriptionAttribute.Length > 0 ? descriptionAttribute[0].Description : value.ToString();
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
                    var objResults = yolo.RunObjectDetection(skBitmap,0.7f);
                    objResults.Track(_sortTracker);
                    skBitmap.Draw(objResults);
                    break;
                case ModelType.Obb:
                    var obbResults = yolo.RunObbDetection(skBitmap);
                    skBitmap.Draw(obbResults);
                    break;
                case ModelType.Pose:
                    var poseResults = yolo.RunPoseEstimation(skBitmap);
                    skBitmap.Draw(poseResults, _drawingOptions!);
                    break;
                case ModelType.Seg:
                    var segResults = yolo.RunSegmentation(skBitmap);
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

        /// <summary>
        /// 
        /// </summary>
        static LeoYolo()
        {
            SetDrawingOptions();
        }


        private static void SetDrawingOptions()
        {
            _drawingOptions = new PoseDrawingOptions
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


