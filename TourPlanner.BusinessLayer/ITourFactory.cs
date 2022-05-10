using System;
using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public interface ITourFactory
    {
        IEnumerable<TourItem> GetItems();
        IEnumerable<TourItem> Search(String itemName, bool caseSensitive = false);
        TourItem CreateTourItem(TourItem tourItem);
        TourLog CreateTourLog(TourLog tourLog, TourItem tourItem);
        IEnumerable<TourLog> GetTourLog(TourItem tourItem);
        string GetImageUrl(string tourName);
        int GetLastTourId();
        public void SaveRouteImageFromApi(string from, string to, string tourName);
        TourItem EditTourItem(TourItem tourItem);
        void DeleteTourItem(TourItem tourItem);
        void DeleteImageTour(TourItem tourItem);
    }
}
