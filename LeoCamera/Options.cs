using Leo.Yolo;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;


namespace LeoCamera
{
    /// <summary>
    /// 
    /// </summary>
    public class  Options:LeoYoloOptions
    {
        [DefaultValue(30)]
        [Category("基础")]
        [DisplayName("延迟")]
        [Description("延迟时间，单位为毫秒，默认值为30ms。这个属性可以用来控制每帧图像处理之间的时间间隔，以避免过度占用系统资源或实现特定的帧率要求。")]
        public int Delay { get; set; } = 30;

        [DefaultValue(0.2)]
        [Category("基础")]
        [DisplayName("置信度")]
        [Description("0-1之间，默认0.2")]
        public override double Confidence { get; set; } = 0.2d;

        [DefaultValue(0.5)]
        [Category("基础")]
        [DisplayName("交并比")]
        [Description("预测框 和 真实框 重叠程度，0-1之间，默认0.5")]
        public override double Iou { get; set; } = 0.5d;

        [DefaultValue(0)]
        [Category("硬件")]
        [DisplayName("摄像头")]
        [Description("")]
        public int CameraId { get; set; } = 0;

        [DefaultValue(ExecutionProviderType.DirectML)]
        [Category("硬件")]
        [DisplayName("执行提供")]
        [Description("使用Cpu或Gpu执行")]
        public override ExecutionProviderType ExecutionProvider { get; set; } = ExecutionProviderType.DirectML;

        [DefaultValue(0)]
        [Category("硬件")]
        [DisplayName("Gpu")]
        [Description("")]
        public override int GpuId { get; set; } = 0;

        [DefaultValue(ModelVision.V26)]
        [Category("模型")]
        [DisplayName("版本")]
        [Description("")]
        public override ModelVision ModelVision { get; set; } = ModelVision.V26;

        [DefaultValue(ModelType.Object)]
        [Category("模型")]
        [DisplayName("类型")]
        [Description("")]
        public override ModelType ModelType { get; set; } = ModelType.Object;

        [DefaultValue(ModelSize.Nano)]
        [Category("模型")]
        [DisplayName("尺寸")]
        [Description("")]
        [ReadOnly(false)]
        public override ModelSize ModelSize { get; set; } = ModelSize.Nano;

        [Category("模型")]
        [DisplayName("路径")]
        [Description("配置了路径，则模型以该路径为准")]
        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public override string? ModelPath { get; set; }

        /// <summary>
        /// 追踪器类型
        /// </summary>
        [DefaultValue(TrackerTypeEnum.Sort)]
        [Category("追踪器")]
        [DisplayName("类型")]
        [Description("")]
        [ReadOnly(false)]
        public override TrackerTypeEnum TrackerType { get; set; } = TrackerTypeEnum.Sort;

        /// <summary>
        /// 匹配阈值
        /// 值越大：匹配越宽松，更容易把不同物体当成同一个（ID 切换少，但容易跟错）
        /// 值越小：匹配越严格，不容易跟错，但容易丢失目标（ID 切换多）
        /// </summary>
        [DefaultValue(0.5f)]
        [Category("追踪器")]
        [DisplayName("匹配阈值")]
        [Description("值越大：匹配越宽松，更容易把不同物体当成同一个（ID 切换少，但容易跟错）；值越小：匹配越严格，不容易跟错，但容易丢失目标（ID 切换多）")]
        [ReadOnly(false)]
        public override float CostThreshold { get; set; } = 0.5f;

        /// <summary>
        /// 最大消失帧数 / 目标丢失后保留帧数
        /// </summary>
        [DefaultValue(3)]
        [Category("追踪器")]
        [DisplayName("消失帧数")]
        [Description("最大允许消失帧数 / 目标丢失后保留帧数")]
        [ReadOnly(false)]
        public override int MaxAge { get; set; } = 3;

        /// <summary>
        /// 轨迹历史长度 / 保留的历史轨迹点数量
        /// 只影响轨迹显示和历史数据缓存，不会改变跟踪结果。
        /// </summary>
        [DefaultValue(30)]
        [Category("追踪器")]
        [DisplayName("轨迹长度")]
        [Description("只影响轨迹显示和历史数据缓存，不会改变跟踪结果。")]
        [ReadOnly(false)]
        public override int TailLength { get; set; } = 30;


    }
}
