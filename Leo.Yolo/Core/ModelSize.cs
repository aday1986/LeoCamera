using System.ComponentModel;


namespace Leo.Yolo
{
    /// <summary>
    /// Yolo模型大小
    /// </summary>
    public enum ModelSize
    {
        [Description("n")]
        Nano,
        [Description("s")]
        Small,
        [Description("m")]
        Medium,
        [Description("l")]
        Large,
        [Description("x")]
        XLarge
    }
}

