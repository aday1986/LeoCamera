using YoloDotNet.Models.Interfaces;


namespace Leo.Yolo
{
    public static class TrackerExtension
    {
        public static List<T> Track<T>(this List<T> detections, ITracker? track) where T : IDetection
        {
            track?.UpdateTracker(detections);
            return detections;
        }
    }
}


