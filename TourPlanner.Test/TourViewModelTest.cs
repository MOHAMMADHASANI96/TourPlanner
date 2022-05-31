using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class TourViewModelTest
    {

        AddNewLogViewModel logAddViewModel = new AddNewLogViewModel();
        //[Test]
        //public void TestInitlistView()
        //{
        //    var tourDao_mock = new Mock<ITourItemDAO>();
        //    //TourViewModel model = new TourViewModel();
        //    ObservableCollection<TourItem> tours = new ObservableCollection<TourItem>();

        //    Assert.AreEqual(tours.Count(),0);

        //    tours.Add(new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car"));
        //    //tourDao_mock.Setup(mock => mock.GetTourItems()).Returns(tours);
        //    //model.FillListBox(tours);

        //    Assert.AreEqual(tours.Count(), 1);
        //}

        [Test, Order(3)]
        public void ValidateReportTestNull()
        {
            logAddViewModel.LogReport = null;
            logAddViewModel.CheckLogReport();
            Assert.AreEqual("Report can not be empty.", logAddViewModel._errorsByPropertyName["LogReport"][0]);
        }
    }
}
