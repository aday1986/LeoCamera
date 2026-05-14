using System.ComponentModel;


namespace Leo.Yolo
{
    public class LeoYoloOptions
    {
        [Category("硬件")]
        [DisplayName("执行提供")]
        [Description("使用Cpu或Gpu执行")]
        public ExecutionProviderType ExecutionProvider { get; set; } = ExecutionProviderType.CPU;

        [Category("硬件")]
        [DisplayName("Gpu")]
        [Description("")]
        public int GpuId { get; set; } = 0;

        [Category("模型")]
        [DisplayName("版本")]
        [Description("")]
        public ModelVision ModelVision { get; set; } = ModelVision.V8;

        [Category("模型")]
        [DisplayName("类型")]
        [Description("")]
        public ModelType ModelType { get; set; } = ModelType.Object;

        [Category("模型")]
        [DisplayName("尺寸")]
        [Description("")]
        public ModelSize ModelSize { get; set; } = ModelSize.Small;

        [Category("模型")]
        [DisplayName("路径")]
        [Description("配置了路径，则模型以该路径为准")]
        public string? ModelPath { get; set; }


    }
}

