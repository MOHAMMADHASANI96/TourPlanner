using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public interface ITourFactory
    {
        //Get 
        IEnumerable<TourItem> GetItems();
        IEnumerable<TourLog> GetTourLog(TourItem tourItem);
        string GetImageUrl(string tourName);
        int GetLastTourId();

        // save image 
        public void SaveRouteImageFromApi(string from, string to, string tourName);

        //search
        IEnumerable<TourItem> Search(string itemName, bool caseSensitive = false);

        // create
        TourItem CreateTourItem(TourItem tourItem);
        TourLog CreateTourLog(TourLog tourLog);
        void PdfGenerate(TourItem tourItem, IEnumerable<TourLog> TourLog);

        // edit
        TourItem EditTourItem(TourItem tourItem);
        TourLog EditTourLog(TourLog tourLog);

        //delete
        void DeleteTourItem(TourItem tourItem);
        void DeleteImageTour(string tourItem);
        void DeleteTourLog(TourLog tourLog);



    }
}
