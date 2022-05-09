using System;
using System.Windows.Input;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class SearchBarViewModel: BaseViewModel
    {
        public event EventHandler<string> SearchTextChanged;

        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }

        private string searchName;

        public string SearchName
        {
            get { return searchName; }
            set
            {
                if (searchName != value)
                {
                    searchName = value;
                    RaisePropertyChangedEvent(nameof(SearchName));
                }
            }
        }

        public SearchBarViewModel()
        {
            this.SearchCommand = new RelayCommand((_) =>
            {
                this.SearchTextChanged?.Invoke(this, SearchName);
            });

            this.ClearCommand = new RelayCommand((_) =>
            {
                this.SearchTextChanged?.Invoke(this, SearchName = "");
            });
        }
    }
}
