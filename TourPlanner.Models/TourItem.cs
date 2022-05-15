using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class TourItem
    {
        public int TourId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ImagePath { get; set; }
        public string Distance { get; set; }
        public string TransportTyp { get; set; }

        public TourItem(int tourId, string name, string description, string from, string to, string imagePath, string distance, string transportTyp)
        {
            this.TourId = tourId;
            this.Name = name;
            this.Description = description;
            this.From = from;
            this.To = to;
            this.ImagePath = imagePath;
            this.Distance = distance;
            this.TransportTyp = transportTyp;
        }
    }
}
