using System.Collections.Generic;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ITourFactory tourItemFactory;
        private readonly TourViewModel tourListVM;
        IEnumerable<TourItem> result;

        public MainViewModel()
        {

        }
        public MainViewModel(TourViewModel tourListVM, SearchBarViewModel searchBarVM , MenuViewModel menuViewModel)
        {
            this.tourItemFactory = TourFactory.GetInstance();
            this.result = this.tourItemFactory.GetItems();

            searchBarVM.SearchTextChanged += (_, searchName) =>
            {
                SearchTours(searchName);
            };

            tourListVM.CurrentItemChanged += (_, currentItem) =>
            {
                //give CurrentTour and pass to menu ViewModel
                menuViewModel.CurrentTour = currentItem;
                //After clicking on the Current Tour to give a pdf, "pdf MenuItem" will be activated
                menuViewModel.Active = true;

                //save to log file
                log.Info("Active PDF MenuItem!");
            };

            // after import file FillListBox should be update
            menuViewModel.ImportSuccessful += (_, importSuccesfully) =>
            {
                //get all Tour Item anf fill list box
                this.result = this.tourItemFactory.GetItems();
                tourListVM.FillListBox(result);
                
                //save to log file
                log.Info("Updating MainScreen after Import File Done!");
            };
            this.tourListVM = tourListVM;
            tourListVM.FillListBox(result);

        }

        private void SearchTours(string searchText)
        {
            this.result = this.tourItemFactory.Search(searchText);
            tourListVM.FillListBox(result);
        }
    }
}
