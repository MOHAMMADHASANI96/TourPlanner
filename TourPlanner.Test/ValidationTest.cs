using NUnit.Framework;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class ValidationTest
    {
        AddNewLogViewModel logAddViewModel = new AddNewLogViewModel();
        [Test, Order(0)]
        public void ValidateReportTestNull()
        {
            logAddViewModel.LogReport = null;
            logAddViewModel.CheckLogReport();
            Assert.AreEqual("Report can not be empty.", logAddViewModel._errorsByPropertyName["LogReport"][0]);
        }
    }
}
