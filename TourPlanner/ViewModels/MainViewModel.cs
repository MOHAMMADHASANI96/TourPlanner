using System.Collections.Generic;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
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
            };

            // after import file FillListBox should be update
            menuViewModel.ImportSuccessful += (_, importSuccesfully) =>
            {
                this.result = this.tourItemFactory.GetItems();
                tourListVM.FillListBox(result);
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
