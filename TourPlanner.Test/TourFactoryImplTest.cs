using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.BusinessLayer;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;

namespace TourPlanner.Test
{
    public class TourFactoryImplTest
    {

        [Test, Order(0)]
        public void GetTourItems_GetAllTour_ReturnNumberOfTourItem()
        {

            var tourDao_mock = new Mock<ITourItemDAO>();
            ITourFactory tourFactory = TourFactory.GetInstance(tourDao_mock.Object);
            List<TourItem> tours = new List<TourItem>();
            tours.Add(new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car"));
            tourDao_mock.Setup(mock => mock.GetTourItems()).Returns(tours);

            List<TourItem> tourList = tourFactory.GetItems().ToList();

            Assert.AreEqual("Tour", tourList[0].Name);
            Assert.AreEqual(tours.Count(), tourList.Count());
        }


        [Test, Order(1)]
        public void CreateTourItem_CreateTourWithGivenInformation_ReturnTourItem()
        {
            var tourDao_mock = new Mock<ITourItemDAO>();
            ITourFactory tourFactory = TourFactory.GetInstance(tourDao_mock.Object);
            TourItem tourItem = new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car");
            tourDao_mock.Setup(mock => mock.AddNewTourItem(tourItem)).Returns(tourItem);
            
            TourItem tourItem1 = new TourItem(2, "Tour1", "Test1", "Linz", "Wien", "Image1", "1000", "Car");
            TourItem tour1 = tourFactory.CreateTourItem(tourItem1);

            Assert.AreNotEqual(tourItem, tour1);
        }


        [Test, Order(2)]
        public void Search_FindToursWithGivenName_ReturnNotEmpty()
        {
            var tourDao_mock = new Mock<ITourItemDAO>();
            ITourFactory tour = TourFactory.GetInstance(tourDao_mock.Object);
            List<TourItem> tours = new List<TourItem>();
            tours.Add(new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car"));
            tourDao_mock.Setup(mock => mock.GetTourItems()).Returns(tours);

            List<TourItem> tourList = tour.Search("Tour").ToList();

            Assert.IsNotEmpty(tourList);
        }

        [Test, Order(3)]
        public void Search_FindToursWithGivenName_ReturnZero()
        {
            var tourDao_mock = new Mock<ITourItemDAO>();
            ITourFactory tour = TourFactory.GetInstance(tourDao_mock.Object);
            List<TourItem> tours = new List<TourItem>();
            tours.Add(new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car"));
            tourDao_mock.Setup(mock => mock.GetTourItems()).Returns(tours);

            List<TourItem> tourList = tour.Search("berlin").ToList();

            Assert.AreEqual(0, tourList.Count());
        }

        [Test, Order(4)]
        public void GetTourLog_FindAllTourLogWithTourItem_ReturnLogReport()
        {
            var logDao_mock = new Mock<ITourLogDAO>();
            List<TourLog> logs = new List<TourLog>();
            TourItem newTour = new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car");
            TourLog newLog = new TourLog(1, new DateTime().ToString(), "rep1", "Hard", "01:00", "**", newTour);
            logs.Add(newLog);
            logDao_mock.Setup(mock => mock.GetLogItems(newTour)).Returns(logs);
            ITourFactory tourFactory = TourFactory.GetInstance(logDao_mock.Object);

            List<TourLog> newlogs = (List<TourLog>)tourFactory.GetTourLog(newTour);

            Assert.AreEqual(logs[0].Report, newlogs[0].Report);
        }

        [Test, Order(5)]
        public void DeleteTourLog_DeleteDesiredLog_VerifyOnceTimeCalled()
        {
            var logDao_mock = new Mock<ITourLogDAO>();
            TourItem newTour = new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car");
            TourLog newLog = new TourLog(1, new DateTime().ToString(), "rep1", "Hard", "01:00", "**", newTour);
            ITourFactory tourFactory = TourFactory.GetInstance(logDao_mock.Object);

            tourFactory.DeleteTourLog(newLog);

            logDao_mock.Verify(mock => mock.DeleteTourLog(newLog), Times.Once());
        }

        [Test, Order(6)]
        public void GetImageUrl_CreatePath_ReturnCorrectPath()
        {
            var tourDao_mock = new Mock<ITourItemDAO>();
            ITourFactory tourFactory = TourFactory.GetInstance(tourDao_mock.Object);
            string name = "wienToLinz";
            string correctpath = "\\wienToLinz.png";

            string url = tourFactory.GetImageUrl(name);

            Assert.AreEqual(correctpath, url);
        }
    }
}
