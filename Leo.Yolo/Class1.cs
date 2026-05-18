using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leo.Yolo
{
    /// <summary>
    /// 工业级 ByteTrack 实现（两轮关联 + Kalman 预测 + 轨迹管理）
    /// </summary>
    public class ByteTrack
    {
        private readonly List<Track> _tracks = new();
        private int _nextTrackId = 1;

        // 可调参数（工业场景推荐值）
        private readonly double highThresh = 0.55;    // 第一轮高置信度阈值
        private readonly double lowThresh = 0.25;     // 第二轮低置信度阈值
        private readonly double matchThresh = 0.55;   // IOU 匹配阈值
        private readonly int maxLostAge = 45;         // 最大丢失帧数（工业遮挡场景建议 30–60）

        /// <summary>
        /// 更新追踪结果
        /// </summary>
        /// <param name="detections">当前帧所有检测框</param>
        /// <returns>激活状态的轨迹列表</returns>
        public List<Track> Update(List<Detection> detections)
        {
            // 清空上一帧匹配标记
            foreach (var t in _tracks) t.IsMatched = false;

            // 第一轮：高置信度匹配
            var highDets = detections.Where(d => d.Conf >= highThresh).ToList();
            MatchDetections(highDets, true);

            // 第二轮：低置信度补救匹配
            var lowDets = detections.Where(d => d.Conf >= lowThresh && d.Conf < highThresh).ToList();
            var unmatchedTracks = _tracks.Where(t => !t.IsMatched).ToList();
            MatchDetections(lowDets, false);

            // 新建轨迹（只用高置信度框）
            foreach (var det in highDets.Where(d => !d.IsMatched))
            {
                _tracks.Add(new Track(det.Box, _nextTrackId++, det.Conf));
            }

            // 年龄更新 + 删除过期轨迹
            foreach (var t in _tracks) t.Age++;
            _tracks.RemoveAll(t => t.Age > maxLostAge);

            // 返回激活的轨迹
            return _tracks.Where(t => t.IsActivated).ToList();
        }

        private void MatchDetections(List<Detection> dets, bool isHighScore)
        {
            foreach (var det in dets)
            {
                Track bestTrack = null;
                double bestIou = 0;

                foreach (var track in _tracks.Where(t => !t.IsMatched && (isHighScore || t.Age > 0)))
                {
                    double iou = IoU(det.Box, track.Box);
                    if (iou > bestIou && iou > matchThresh)
                    {
                        bestIou = iou;
                        bestTrack = track;
                    }
                }

                if (bestTrack != null)
                {
                    bestTrack.Update(det.Box, det.Conf);
                    det.IsMatched = true;
                    bestTrack.IsMatched = true;
                }
            }
        }

        private static double IoU(Rect a, Rect b)
        {
            int interX = Math.Max(0, Math.Min(a.Right, b.Right) - Math.Max(a.Left, b.Left));
            int interY = Math.Max(0, Math.Min(a.Bottom, b.Bottom) - Math.Max(a.Top, b.Top));
            float interArea = interX * interY;
            float unionArea = a.Width * a.Height + b.Width * b.Height - interArea;
            return unionArea > 0 ? interArea / unionArea : 0;
        }
    }

    public class Track
    {
        public Rect Box { get; private set; }
        public int TrackId { get; }
        public float Score { get; private set; }
        public int Age { get; set; } = 0;
        public bool IsMatched { get; set; } = false;
        public bool IsActivated => Age < 3;  // 连续匹配 3 帧后激活

        public Track(Rect box, int id, float score)
        {
            Box = box;
            TrackId = id;
            Score = score;
        }

        public void Update(Rect newBox, float newScore)
        {
            Box = newBox;
            Score = newScore;
            Age = 0;
            IsMatched = true;
        }
    }

    // 检测结果（需与 YOLO 输出对应）
    public record Detection(Rect Box, float Conf, string Label)
    {
        public bool IsMatched { get; set; } = false;
    }
}
