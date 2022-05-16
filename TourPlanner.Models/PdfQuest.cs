using System.Collections.Generic;

namespace TourPlanner.Models
{
    public class PdfQuest
    {
        public TourItem TourItem { get; set; }

        public IEnumerable<TourLog> TourLogs { get; set; }

        public byte[] Image { get; set; }
    }
}
