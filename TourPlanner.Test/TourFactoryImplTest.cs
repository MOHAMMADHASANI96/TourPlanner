using Moq;
using NUnit.Framework;
using System;
using System.Configuration;
using TourPlanner.BusinessLayer;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class TourFactoryImplTest
    {
        //[Test]
        //public void VerifyAppDomainHasConfigurationSettings()
        //{
        //    string value = ConfigurationManager.AppSettings["ApiKey"];
        //    Assert.AreEqual(value, "gbxoMANTCgZCQPPwmBdaCcXtkpX6KAfJ");
        //    //Assert.IsFalse(String.IsNullOrEmpty(value), "No App.Config found.");
        //}
        //[Test]
        //public void TestTourCreation()
        //{
        //    var tourItemDao_mock = new Mock<ITourItemDAO>();
        //    TourItem tourItem = new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car");
        //    tourItemDao_mock.Setup(mock => mock.AddNewTourItem(tourItem)).Returns(tourItem);


        //    ITourFactory tourFactory = TourFactory.GetInstance();
        //    TourItem tourItem1 = new TourItem(1, "Tour", "Test1", "Linz", "Wien", "Image1", "1000", "Car");
        //    TourItem newTourItem = tourFactory.CreateTourItem(tourItem1);

        //    Assert.AreEqual(newTourItem, tourItem);
        //}

        //[Test]
        //public void TestData_ShouldContainInitialList()
        //{
        //    // Arrange
        //    TourViewModel tourListVM = new TourViewModel();
        //    SearchBarViewModel searchBarVM = new SearchBarViewModel();
        //    MenuViewModel menuViewModel = new MenuViewModel();
        //    MainViewModel mainViewModel = new MainViewModel(tourListVM, searchBarVM, menuViewModel);
        //    // Act
        //    int expected = 4;
        //    int actual = tourListVM.TourItems.Count;
        //    // Assert
        //    Assert.AreEqual(expected, actual, "List should contain 4 Tour!");
        //}


        //[Test]
        //public void TestSearchCommand_Shouldfind()
        //{
        //    // Arrange
        //    TourViewModel tourListVM = new TourViewModel();
        //    SearchBarViewModel searchBarVM = new SearchBarViewModel();
        //    MenuViewModel menuViewModel = new MenuViewModel();
        //    MainViewModel mainViewModel = new MainViewModel(tourListVM,searchBarVM, menuViewModel);
        //    searchBarVM.SearchName = "1";
        //    // Act
        //    searchBarVM.SearchCommand.Execute(null); // simulate search button click
        //    int expectedDataCount = 1;
        //    int currentDataCount = tourListVM.TourItems.Count;
        //    // Assert
        //    Assert.AreEqual(expectedDataCount, currentDataCount, "Two Tour finded");
        //}

        //[Test]
        //public void TestClearCommand_ShouldClear()
        //{
        //    // Arrange
        //    TourViewModel tourListVM = new TourViewModel();
        //    SearchBarViewModel searchBarVM = new SearchBarViewModel();
        //    MenuViewModel menuViewModel = new MenuViewModel();
        //    MainViewModel mainViewModel = new MainViewModel(tourListVM, searchBarVM, menuViewModel);
        //    // Act
        //    searchBarVM.ClearCommand.Execute(null); // simulate clear button click
        //    int expectedDataCount = 4;
        //    int currentDataCount = tourListVM.TourItems.Count;
        //    // Assert
        //    Assert.AreEqual(expectedDataCount, currentDataCount, "4 Tour finded");
        //}
    }
}
