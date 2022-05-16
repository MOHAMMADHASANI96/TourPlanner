using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TourPlanner.BusinessLayer.PdfGenerator;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourFactoryImpl : ITourFactory
    {

        MapQuestApiProcessor mapQuestApiProcessor = new MapQuestApiProcessor();

        public string RoutPhotoFolder { get; set; }
        public string RoutPDFFolder { get; set; }


        public TourFactoryImpl()
        {
            RoutPhotoFolder = ConfigurationManager.AppSettings["RoutPhotoFolder"];
            RoutPDFFolder = ConfigurationManager.AppSettings["RoutPDFFolder"];
        }

        //Get Tour Item 
        public IEnumerable<TourItem> GetItems()
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.GetTourItems();
        }

        //serach 
        public IEnumerable<TourItem> Search(string itemName, bool caseSensitive = false)
        {
            IEnumerable<TourItem> items = GetItems();
            if (caseSensitive)
            {
                return items.Where(x => x.Name.Contains(itemName));
            }
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        //Create Tour Item
        public TourItem CreateTourItem(TourItem tourItem)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.AddNewTourItem(tourItem);
        }

        //Create Tour Log
        public TourLog CreateTourLog(TourLog tourLog)
        {
            ITourLogDAO tourLogDAO = DALFactory.CreateTourLogDAO();
            return tourLogDAO.AddNewTourLog(tourLog);
        }

        //Get Tour Log
        public IEnumerable<TourLog> GetTourLog(TourItem tourItem)
        {
            ITourLogDAO tourLogDAO = DALFactory.CreateTourLogDAO();
            return tourLogDAO.GetLogItems(tourItem);
        }

        //get Last Id
        public int GetLastTourId()
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.GetLastTourId();
        }

        // Save Image in Folder
        public async void SaveRouteImageFromApi(string from, string to, string tourName)
        {
            string url = mapQuestApiProcessor.DirectionUrlCreate(from, to, tourName);
            Tuple<string, string> t = await mapQuestApiProcessor.DirectionApi(url, tourName);

            string staticUrl = mapQuestApiProcessor.StaticUrlCreate(t.Item1, t.Item2);

            mapQuestApiProcessor.StaticMapApi(staticUrl, tourName);
        }

        //Get Image URL
        public string GetImageUrl(string tourName)
        {
            string path = RoutPhotoFolder + "\\" + tourName + ".png";
            return path;
        }

        //Edit Tour Item
        public TourItem EditTourItem(TourItem tourItem)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            return tourItemDAO.EditTourItem(tourItem);
        }

        //Edit Tour Log
        public TourLog EditTourLog(TourLog tourLog)
        {
           ITourLogDAO tourLogDAO = DALFactory.CreateTourLogDAO();
           return tourLogDAO.EditTourLog(tourLog);
        }

        //Delete Tour Item
        public void DeleteTourItem(TourItem tourItem)
        {
            ITourItemDAO tourItemDAO = DALFactory.CreateTourItemDAO();
            tourItemDAO.DeleteTourItem(tourItem);
            //DeleteImageTour(tourItem.Name);
        }

        //Delete Tour Image
        public void DeleteImageTour(string tourItem)
        {
            mapQuestApiProcessor.DeleteImage(tourItem);
        }

        //Delete Tour Log
        public void DeleteTourLog(TourLog tourLog)
        {
            ITourLogDAO tourLogDAO = DALFactory.CreateTourLogDAO();
            tourLogDAO.DeleteTourLog(tourLog);
        }

        //Get Pdf file path
        public string GetPdfFilePath(string tourName)
        {
            string pdfPath = RoutPDFFolder + "\\" + tourName + ".pdf";
            return pdfPath;
        }

        public void PdfGenerate(TourItem tourItem, IEnumerable<TourLog> TourLog)
        {
            string pdfPath = GetPdfFilePath(tourItem.Name);

            PdfDataSource pdfDataSource = new PdfDataSource();
            PdfQuest model = pdfDataSource.GetDetials(tourItem, TourLog);
            var document = new ReportTemplate(model);
            document.GeneratePdf(pdfPath);
        }
    }
}
