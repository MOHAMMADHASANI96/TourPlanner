using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public interface ITourFactory
    {
        // Get 
        IEnumerable<TourItem> GetItems();
        IEnumerable<TourLog> GetTourLog(TourItem tourItem);
        string GetImageUrl(string tourName);
        int GetLastTourId();
        TourItem FindTourItemByName(string tourName);

        // Save image 
        public void SaveRouteImageFromApi(string from, string to, string tourName);

        // Search
        IEnumerable<TourItem> Search(string itemName, bool caseSensitive = false);

        // Create
        TourItem CreateTourItem(TourItem tourItem);
        TourLog CreateTourLog(TourLog tourLog);
        bool PdfGenerate(TourItem tourItem, IEnumerable<TourLog> TourLog, string path);
        bool ExportGenerate(List<Export> exportObjects);
        bool ImportFile(string filePath);

        // Edit
        TourItem EditTourItem(TourItem tourItem);
        TourLog EditTourLog(TourLog tourLog);

        // Delete
        void DeleteTourItem(TourItem tourItem);
        void DeleteAllTourItems();
        void DeleteImageTour(string tourItem);
        void DeleteTourLog(TourLog tourLog);



    }
}
