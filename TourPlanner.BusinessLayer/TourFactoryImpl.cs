﻿using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using TourPlanner.BusinessLayer.PdfGenerator;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;
using System.IO;

namespace TourPlanner.BusinessLayer
{
    internal class TourFactoryImpl : ITourFactory
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        MapQuestApiProcessor mapQuestApiProcessor = new MapQuestApiProcessor();

        public string RoutPhotoFolder { get; set; }
        public string RoutPDFFolder { get; set; }
        public string RoutExportFolder { get; set; }
        public ITourItemDAO tourItemDAO { get; set; }
        public ITourLogDAO tourLogDAO { get; set; }


        public TourFactoryImpl()
        {
            RoutPhotoFolder = ConfigurationManager.AppSettings["RoutPhotoFolder"];
            RoutPDFFolder = ConfigurationManager.AppSettings["RoutPDFFolder"];
            RoutExportFolder = ConfigurationManager.AppSettings["RoutExportFolder"];
            tourItemDAO = DALFactory.CreateTourItemDAO();
            tourLogDAO = DALFactory.CreateTourLogDAO();
        }

        //for testing
        public TourFactoryImpl(ITourItemDAO tourItemDAO)
        {
            RoutPhotoFolder = ConfigurationManager.AppSettings["RoutPhotoFolder"];
            RoutPDFFolder = ConfigurationManager.AppSettings["RoutPDFFolder"];
            RoutExportFolder = ConfigurationManager.AppSettings["RoutExportFolder"];
            this.tourItemDAO = tourItemDAO;
        }
        //for testing
        public TourFactoryImpl(ITourLogDAO tourLogDAO)
        {
            RoutPhotoFolder = ConfigurationManager.AppSettings["RoutPhotoFolder"];
            RoutPDFFolder = ConfigurationManager.AppSettings["RoutPDFFolder"];
            RoutExportFolder = ConfigurationManager.AppSettings["RoutExportFolder"];
            this.tourLogDAO = tourLogDAO;
        }


        //Get Tour Item 
        public IEnumerable<TourItem> GetItems()
        {
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
            return tourItemDAO.AddNewTourItem(tourItem);
        }

        //Create Tour Log
        public TourLog CreateTourLog(TourLog tourLog)
        {
            return tourLogDAO.AddNewTourLog(tourLog);
        }

        //Get Tour Log
        public IEnumerable<TourLog> GetTourLog(TourItem tourItem)
        {
            return tourLogDAO.GetLogItems(tourItem);
        }

        //get Last Id
        public int GetLastTourId()
        {
            return tourItemDAO.GetLastTourId();
        }

        //get Tour by Name
        public TourItem FindTourItemByName(string name)
        {
            return tourItemDAO.FindTourItemByName(name);
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
            return tourItemDAO.EditTourItem(tourItem);
        }

        //Edit Tour Log
        public TourLog EditTourLog(TourLog tourLog)
        {
           return tourLogDAO.EditTourLog(tourLog);
        }

        //Delete Tour Item
        public void DeleteTourItem(TourItem tourItem)
        {
            tourItemDAO.DeleteTourItem(tourItem);
        }

        //Delete All Tour Item
        public void DeleteAllTourItems()
        {
            tourItemDAO.DeleteAllTourItems();
        }

        //Delete Tour Image
        public void DeleteImageTour(string tourItem)
        {
            mapQuestApiProcessor.DeleteImage(tourItem);
        }

        //Delete Tour Log
        public void DeleteTourLog(TourLog tourLog)
        {
            tourLogDAO.DeleteTourLog(tourLog);
        }

        //create pdf file
        public bool PdfGenerate(TourItem tourItem, IEnumerable<TourLog> TourLog, string path)
        {
            try
            {

                PdfDataSource pdfDataSource = new PdfDataSource();
                PdfQuest model = pdfDataSource.GetDetials(tourItem, TourLog);
                var document = new ReportTemplate(model);
                document.GeneratePdf(path);
                return true;
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
                return false;
            }  
        }

        // create Export File
        public bool ExportGenerate(List<Export> exportObjects,string path)
        {
            try
            {
                string exportPath = path;

                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                using (StreamWriter sw = new StreamWriter(exportPath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, exportObjects);
                }
                return true;
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
                return false;
            }
        }

        // Import file
        public bool ImportFile(string filePath)
        {
            try
            {
                DeleteAllTourItems();

                string json = File.ReadAllText(filePath);
                List<Export> exportObjects = JsonConvert.DeserializeObject<List<Export>>(json);

                foreach (Export exportObject in exportObjects)
                {
                    TourItem tourItem = new TourItem(0, exportObject.TourItem.Name, exportObject.TourItem.Description, exportObject.TourItem.From, exportObject.TourItem.To, exportObject.TourItem.ImagePath, exportObject.TourItem.Distance, exportObject.TourItem.TransportTyp);
                    CreateTourItem(tourItem);
                    if (exportObject.TourLog != null)
                    {
                        TourItem tour = FindTourItemByName(tourItem.Name);
                        foreach (TourLog tourLog in exportObject.TourLog)
                        {

                            TourLog log = new TourLog(0, tourLog.DateTime, tourLog.Report, tourLog.Difficulty, tourLog.TotalTime, tourLog.Rating, tour);
                            CreateTourLog(log);
                        }
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
                return false;
            }    
        }
    }
}
