using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.BusinessLayer;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;

namespace TourPlanner.Test
{
    public class TourFactoryImplTest
    {

        [Test]
        public void TestTourGet()
        {
            var tourDao_mock = new Mock<ITourItemDAO>();


            ITourFactory tourFactory = TourFactory.GetInstance(tourDao_mock.Object);
            List<TourItem> tours = new List<TourItem>();
            TourItem tourItem = new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car");
            tours.Add(tourItem);

            tourDao_mock.Setup(mock => mock.GetTourItems()).Returns(tours);

            List<TourItem> tourList = tourFactory.GetItems().ToList();

            Assert.AreEqual("Tour", tourList[0].Name);
            Assert.AreEqual(tours.Count(), tourList.Count());
        }


        [Test]
        public void TestTourCreation()
        {
            var tourDao_mock = new Mock<ITourItemDAO>();
            ITourFactory tourFactory = TourFactory.GetInstance(tourDao_mock.Object);
            
            TourItem tourItem = new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car");
            
            tourDao_mock.Setup(mock => mock.AddNewTourItem(tourItem)).Returns(tourItem);
            
            TourItem tourItem1 = new TourItem(2, "Tour1", "Test1", "Linz", "Wien", "Image1", "1000", "Car");
            TourItem tour1 = tourFactory.CreateTourItem(tourItem1);

            Assert.AreNotEqual(tourItem, tour1);
        }


    }
}
