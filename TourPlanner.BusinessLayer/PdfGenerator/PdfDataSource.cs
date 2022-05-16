using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.PdfGenerator
{
    public class PdfDataSource
    {
        private string RoutPhotoFolder;
        public PdfDataSource()
        {
            RoutPhotoFolder = ConfigurationManager.AppSettings["RoutPhotoFolder"];
        }
        public PdfQuest GetDetials(TourItem tour, IEnumerable<TourLog> log)
        {

            string imagePath = RoutPhotoFolder + "\\" + tour.Name + ".png";

            return new PdfQuest
            {
                TourItem = tour,
                TourLogs = log,
                Image = File.ReadAllBytes(imagePath)
            };
        }
    }
}
