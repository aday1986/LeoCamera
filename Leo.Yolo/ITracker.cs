using YoloDotNet.Models.Interfaces;


namespace Leo.Yolo
{
    public interface ITracker
    {
        public void UpdateTracker<T>(List<T> detections) where T : IDetection;
    }
}


