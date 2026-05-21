using YoloDotNet.Models.Interfaces;
using YoloDotNet.Trackers;


namespace Leo.Yolo
{
    public class BaseSortTracker : ITracker
    {
        private readonly SortTracker _sort;
        public BaseSortTracker(float costThreshold, int maxAge, int tailLength)
        {
            _sort = new SortTracker(costThreshold, maxAge, tailLength);
        }
        public void UpdateTracker<T>(List<T> detections) where T : IDetection
        {
            _sort.UpdateTracker<T>(detections);
        }
    }
}


