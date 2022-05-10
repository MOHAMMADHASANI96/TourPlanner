using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.DAO
{
    public interface ITourItemDAO
    {
        TourItem FindTourItemById(int tourItemId);
        TourItem AddNewTourItem(TourItem tourItem);
        IEnumerable<TourItem> GetTourItems();
        int GetLastTourId();
        TourItem EditTourItem(TourItem tourItem);
        void DeleteTourItem(TourItem tourItem);
    }
}
