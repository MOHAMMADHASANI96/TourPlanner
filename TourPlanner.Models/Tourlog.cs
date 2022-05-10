using System;


namespace TourPlanner.Models
{
    public class TourLog
    {
        public int LogId { get; set; }
        public DateTime DateTime { get; set; }
        public string Report { get; set; }
        public string Difficulty { get; set; }
        public TimeSpan TotalTime { get; set; }
        public string Rating { get; set; }
        public TourItem LogTourItem { get; set; }

        public TourLog(int logId, DateTime dateTime, string report, string difficulty, TimeSpan totalTime, string rating, TourItem LogTourItem)
        {
            this.LogId = logId;
            this.DateTime = dateTime;
            this.Report = report;
            this.Difficulty = difficulty;
            this.TotalTime = totalTime;
            this.Rating = rating;
            this.LogTourItem = LogTourItem;
        }
    }
}
