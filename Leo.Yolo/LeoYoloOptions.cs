using System.ComponentModel;

namespace Leo.Yolo
{
    public class LeoYoloOptions
    {

        public virtual ExecutionProviderType ExecutionProvider { get; set; } = ExecutionProviderType.DirectML;

        public virtual int GpuId { get; set; } = 0;

        public virtual ModelVision ModelVision { get; set; } = ModelVision.V26;

        public virtual ModelType ModelType { get; set; } = ModelType.Object;

        public virtual ModelSize ModelSize { get; set; } = ModelSize.Nano;

        public virtual string? ModelPath { get; set; }

        public virtual double Confidence { get; set; } = 0.2d;

        public virtual double Iou { get; set; } = 0.5d;

        /// <summary>
        /// 跟踪器类型
        /// </summary>
        public virtual TrackerTypeEnum TrackerType { get; set; } = TrackerTypeEnum.Sort;

        /// <summary>
        /// 匹配阈值
        /// 值越大：匹配越宽松，更容易把不同物体当成同一个（ID 切换少，但容易跟错）
        /// 值越小：匹配越严格，不容易跟错，但容易丢失目标（ID 切换多）
        /// </summary>
        public virtual float CostThreshold { get; set; } = 0.5f;

        /// <summary>
        /// 最大消失帧数 / 目标丢失后保留帧数
        /// </summary>
        public virtual int MaxAge { get; set; } = 3;

        /// <summary>
        /// 轨迹历史长度 / 保留的历史轨迹点数量
        /// 只影响轨迹显示和历史数据缓存，不会改变跟踪结果。
        /// 
        /// </summary>
        public virtual int TailLength { get; set; } = 30;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TrackerTypeEnum
    {
        None = 0,
        Sort=1,
        ByteTrack=2,
        DeepSort=3
    }
}

