using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourFactoryImpl : ITourFactory
    {

        MapQuestApiProcessor mapQuestApiProcessor = new MapQuestApiProcessor();

        public string RoutPhotoFolder { get; set; }
        public TourFactoryImpl()
        {
            RoutPhotoFolder = ConfigurationManager.AppSettings["RoutPhotoFolder"];
        }
        public IEnumerable<TourItem> GetItems()
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.GetTourItems();
        }

        public IEnumerable<TourItem> Search(string itemName, bool caseSensitive = false)
        {
            IEnumerable<TourItem> items = GetItems();
            if (caseSensitive)
            {
                return items.Where(x => x.Name.Contains(itemName));
            }
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        public TourItem CreateTourItem(TourItem tourItem)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.AddNewTourItem(tourItem);
        }

        public TourLog CreateTourLog(TourLog tourLog, TourItem tourItem)
        {
            ITourLogDAO tourLogDAO = DALFactory.CreateTourLogDAO();
            return tourLogDAO.AddNewTourLog(tourLog, tourItem);
        }
        public IEnumerable<TourLog> GetTourLog(TourItem tourItem)
        {
            ITourLogDAO tourLogDAO = DALFactory.CreateTourLogDAO();
            return tourLogDAO.GetLogItems(tourItem);
        }

        public int GetLastTourId()
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.GetLastTourId();
        }

        public async void SaveRouteImageFromApi(string from, string to, string tourName)
        {
            string url = mapQuestApiProcessor.DirectionUrlCreate(from, to, tourName);
            Tuple<string, string> t = await mapQuestApiProcessor.DirectionApi(url, tourName);

            string staticUrl = mapQuestApiProcessor.StaticUrlCreate(t.Item1, t.Item2);

            mapQuestApiProcessor.StaticMapApi(staticUrl, tourName);
        }
        public string GetImageUrl(string tourName)
        {
            string path = RoutPhotoFolder + "\\" + tourName + ".png";
            return path;
        }

        public TourItem EditTourItem(TourItem tourItem)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.EditTourItem(tourItem);
        }

        public void DeleteTourItem(TourItem tourItem)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            tourItemDAO.DeleteTourItem(tourItem);
        }

        public void DeleteImageTour(TourItem tourItem)
        {
            mapQuestApiProcessor.DeleteImage(tourItem);
        }
    }
}
