using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class Export
    {
        public TourItem TourItem { get; set; }
        public List<TourLog> TourLog { get; set; }
    }
}
