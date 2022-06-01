using System;


namespace TourPlanner.Models
{
    public class TourLog
    {
        public int LogId { get; set; }
        public string DateTime { get; set; }
        public string Report { get; set; }
        public string Difficulty { get; set; }
        public string TotalTime { get; set; }
        public string Rating { get; set; }
        public TourItem LogTourItem { get; set; }

        public TourLog(int logId, string dateTime, string report, string difficulty, string totalTime, string rating, TourItem logTourItem)
        {
            this.LogId = logId;
            this.DateTime = dateTime;
            this.Report = report;
            this.Difficulty = difficulty;
            this.TotalTime = totalTime;
            this.Rating = rating;
            this.LogTourItem = logTourItem;
        }
    }
}
