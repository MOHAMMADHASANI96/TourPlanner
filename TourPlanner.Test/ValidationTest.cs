using NUnit.Framework;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class ValidationTest
    {
        // AddNewTourLogTest-----------------------------------------------------------------------------------------

        AddNewLogViewModel logAddViewModel = new AddNewLogViewModel();

        [Test, Order(0)]
        public void CheckLogDate_EntryLogDateIsEmpty_ReturnValidationError()
        {
            logAddViewModel.LogDate = null;
            logAddViewModel.CheckLogDate();
            Assert.AreEqual("Date cannot be empty.", logAddViewModel._errorsByPropertyName["LogDate"][0]);
        }

        [Test, Order(1)]
        public void CheckLogDate_EntryLogDateHasIncorrectFormat_ReturnValidationError()
        {
            logAddViewModel.LogDate = "13/03/2020";
            logAddViewModel.CheckLogDate();
            Assert.AreEqual("Date Time Format must be MM/DD/YYYY.", logAddViewModel._errorsByPropertyName["LogDate"][0]);
        }

        [Test, Order(2)]
        public void CheckLogDifficulty_EntryLogDifficultyIsEmpty_ReturnValidationError()
        {
            logAddViewModel.LogDifficulty = null;
            logAddViewModel.CheckLogDifficulty();
            Assert.AreEqual("Difficulty can not be empty.", logAddViewModel._errorsByPropertyName["LogDifficulty"][0]);
        }

        

        [Test, Order(3)]
        public void CheckLogTotalTime_EntryLogTotalTimeIsEmpty_ReturnValidationError()
        {
            logAddViewModel.LogTotalTime = null;
            logAddViewModel.CheckLogTotalTime();
            Assert.AreEqual("Total Time cannot be empty.", logAddViewModel._errorsByPropertyName["LogTotalTime"][0]);
        }

        [Test, Order(4)]
        public void CheckLogTotalTime_EntryLogTotalTimeHasIncorrectFormat_ReturnValidationError()
        {
            logAddViewModel.LogTotalTime = "1100:20";
            logAddViewModel.CheckLogTotalTime();
            Assert.AreEqual("Total Time Format must be HH:MM ,and Max Hours until 999", logAddViewModel._errorsByPropertyName["LogTotalTime"][0]);
        }

        [Test, Order(5)]
        public void CheckLogReport_EntryLogReportIsEmpty_ReturnValidationError()
        {
            logAddViewModel.LogReport = null;
            logAddViewModel.CheckLogReport();
            Assert.AreEqual("Report can not be empty.", logAddViewModel._errorsByPropertyName["LogReport"][0]);
        }

        [Test, Order(6)]
        public void CheckLogReport_EntryLogReportHasMoreCharacter_ReturnValidationError()
        {
            logAddViewModel.LogReport = "jfkhldjhdkglsmbjfkhldmhjshdnflkeorwgkjslhjeorfgdbetrztr";
            logAddViewModel.CheckLogReport();
            Assert.AreEqual("Report has to be lower than 50 characters.", logAddViewModel._errorsByPropertyName["LogReport"][0]);
        }

        [Test, Order(7)]
        public void CheckLogRating_EntryLogRatingIsEmpty_ReturnValidationError()
        {
            logAddViewModel.LogRating = null;
            logAddViewModel.CheckLogRating();
            Assert.AreEqual("Rating can not be empty.", logAddViewModel._errorsByPropertyName["LogRating"][0]);
        }

        // AddNewTourItemTest-----------------------------------------------------------------------------------------

        AddNewTourViewModel addNewTourViewModel = new AddNewTourViewModel();

        [Test, Order(8)]
        public void CheckTourDistance_EntryTourDistanceIsLong_ReturnValidationError()
        {
            addNewTourViewModel.TourDistance = "10001";
            addNewTourViewModel.CheckTourDistance();
            Assert.AreEqual("Distance has to be between 1 and 10000 km.", addNewTourViewModel._errorsByPropertyName["TourDistance"][0]);
        }

        [Test, Order(9)]
        public void CheckTourDistance_EntryTourDistanceHasIncorrectFormat_ReturnValidationError()
        {
            addNewTourViewModel.TourDistance = "ABC";
            addNewTourViewModel.CheckTourDistance();
            Assert.AreEqual("Distance has to be a float.", addNewTourViewModel._errorsByPropertyName["TourDistance"][0]);
        }



    }
}
