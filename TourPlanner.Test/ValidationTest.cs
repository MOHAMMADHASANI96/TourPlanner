using NUnit.Framework;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class ValidationTest
    {
        // AddNewTourLogTest

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
        public void ValidateReportTestNull()
        {
            logAddViewModel.LogReport = null;
            logAddViewModel.CheckLogReport();
            Assert.AreEqual("Report can not be empty.", logAddViewModel._errorsByPropertyName["LogReport"][0]);
        }
    }
}
