using System;
using System.Collections.Generic;
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
        IEnumerable<TourItem> Search(String itemName, bool caseSensitive = false);

        // create
        TourItem CreateTourItem(TourItem tourItem);
        TourLog CreateTourLog(TourLog tourLog);

        // edit
        TourItem EditTourItem(TourItem tourItem);
        TourLog EditTourLog(TourLog tourLog);

        //delete
        void DeleteTourItem(TourItem tourItem);
        void DeleteImageTour(TourItem tourItem);



    }
}
