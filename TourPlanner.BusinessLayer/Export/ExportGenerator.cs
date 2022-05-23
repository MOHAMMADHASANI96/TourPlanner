using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.ExportGenerator
{
    public class ExportGenerator
    {
        private ITourFactory tourFactory;
        public ExportGenerator()
        {
            this.tourFactory = TourFactory.GetInstance();
        }

        public List<Export> Export()
        {
            List<Export> exportObjects = new List<Export>();
            IEnumerable<TourItem> tourItems = new List<TourItem>();
            tourItems = this.tourFactory.GetItems();

            foreach (TourItem tour in tourItems)
            {
                Export exportObject = new Export() { TourItem = tour };
                exportObject.TourLog = new List<TourLog>();
                if (this.tourFactory.GetTourLog(exportObject.TourItem) != null)
                {
                    foreach (TourLog log in this.tourFactory.GetTourLog(exportObject.TourItem))
                    {
                        if (log != null)
                        {
                            exportObject.TourLog.Add(log);
                        }
                    }
                }
                exportObjects.Add(exportObject);
            }
            return exportObjects;
        } 
    }
}
