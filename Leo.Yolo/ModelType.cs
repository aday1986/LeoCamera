using System.ComponentModel;


namespace Leo.Yolo
{
    /// <summary>
    /// Yolo模型类型
    /// </summary>
    public enum ModelType
    {
        [Description("")]
        Object,
        [Description("obb")]
        Obb,
        [Description("pose")]
        Pose,
        [Description("seg")]
        Seg,
        [Description("cls")]
        Cls
    }
}

