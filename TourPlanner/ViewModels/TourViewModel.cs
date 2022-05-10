using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels;
using TourPlanner.ViewModels.Abstract;
using TourPlanner.Views;

namespace TourPlanner.ViewModels
{
    public class TourViewModel:BaseViewModel
    {
        private ITourFactory tourItemFactory;
        private IEnumerable<TourLog> tourLogs;


        private TourItem currentItem;
        private TourLog currentLog;

        public EditTourViewModel editTourViewModel;

        //Tour items variable 
        private string currentTourImagePath;

        // TourLog items variable 
        private string dateTime;
        private string report;
        private string distance;
        private string totalTime;
        private string rating;

        //command
        private ICommand addNewTour;
        private ICommand editTour;
        private ICommand deleteTour;
        public ICommand DeleteTour => deleteTour ??= new RelayCommand(PerformDeleteTour);
        public ICommand AddNewTour => addNewTour ??= new RelayCommand(PerformAddNewTour);
        public ICommand EditTour => editTour ??= new RelayCommand(PerformEditTour);

        public ObservableCollection<TourItem> TourItems { get; set; }
        public ObservableCollection<TourLog> TourLogs { get; set; }

        public TourViewModel()
        {
            TourItems = new ObservableCollection<TourItem>();
            TourLogs = new ObservableCollection<TourLog>();
        }

        public void FillListBox(IEnumerable<TourItem> tourItems)
        {
            TourItems.Clear();
            foreach (TourItem item in tourItems)
            {
                TourItems.Add(item);
            }
        }

        public void FillLogBox(IEnumerable<TourLog> tourLogs)
        {
            TourLogs.Clear();
            foreach (TourLog item in tourLogs)
            {
                TourLogs.Add(item);
            }
        }

        public TourItem CurrentItem
        {
            get
            {
                return currentItem;
            }
            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                    this.tourItemFactory = TourFactory.GetInstance();
                    currentTourImagePath = this.tourItemFactory.GetImageUrl(currentItem.ImagePath);
                    RaisePropertyChangedEvent(nameof(CurrentTourImagePath));
                    FillLogBox(this.tourItemFactory.GetTourLog(currentItem));

                }
            }
        }

        public IEnumerable<TourLog> Logs
        {
            get
            {
                return tourLogs;
            }
            set
            {
                if ((tourLogs != value) && (value != null))
                {
                    tourLogs = value;
                    RaisePropertyChangedEvent(nameof(Logs));
                }
            }
        }

        public string CurrentTourImagePath
        {
            get { return currentTourImagePath; }
            set
            {
                if ((currentTourImagePath != value) && (value != null))
                {
                    currentTourImagePath = value;
                    RaisePropertyChangedEvent(nameof(CurrentTourImagePath));
                }
            }
        }

        public TourLog CurrentLog
        {
            get
            {
                return currentLog;
            }
            set
            {
                if ((currentLog != value) && (value != null))
                {
                    currentLog = value;
                    RaisePropertyChangedEvent(nameof(currentLog));
                }
            }
        }

        public string DateTime
        {
            get { return dateTime.ToString(); }
            set
            {
                if ((dateTime != value) && (value != null))
                {
                    dateTime = value;
                    RaisePropertyChangedEvent(nameof(DateTime));
                }
            }
        }

        public string Report
        {
            get { return report; }
            set
            {
                if ((report != value) && (value != null))
                {
                    report = value;
                    RaisePropertyChangedEvent(nameof(Report));
                }
            }
        }

        public string Distance
        {
            get { return distance.ToString(); }
            set
            {
                if ((distance != value) && (value != null))
                {
                    distance = value;
                    RaisePropertyChangedEvent(nameof(Distance));
                }
            }
        }

        public string TotalTime
        {
            get { return totalTime.ToString(); }
            set
            {
                if ((totalTime != value) && (value != null))
                {
                    totalTime = value;
                    RaisePropertyChangedEvent(nameof(TotalTime));
                }
            }
        }

        public string Rating
        {
            get { return rating.ToString(); }
            set
            {
                if ((rating != value) && (value != null))
                {
                    rating = value;
                    RaisePropertyChangedEvent(nameof(Rating));
                }
            }
        }



        private void PerformAddNewTour(object commandParameter)
        {
            AddNewTourView addTour = new AddNewTourView();
            addTour.DataContext = new AddNewTourViewModel();
            addTour.ShowDialog();
            this.tourItemFactory = TourFactory.GetInstance();
            IEnumerable<TourItem> result = this.tourItemFactory.GetItems();
            FillListBox(result);

        }

        private void PerformEditTour(object commandParameter)
        {
            if(CurrentItem != null)
            {
                this.editTourViewModel = new EditTourViewModel();
                editTourViewModel.CurrentTour = CurrentItem;
                EditTourView editTour = new EditTourView();
                editTour.DataContext = this.editTourViewModel;
                editTour.ShowDialog();
                this.tourItemFactory = TourFactory.GetInstance();
                IEnumerable<TourItem> result = this.tourItemFactory.GetItems();
                FillListBox(result);
            }
            

        }



        private void PerformDeleteTour(object commandParameter)
        {
            if (CurrentItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Would you like to Delete this Tour?", "Delete Tour", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        this.tourItemFactory.DeleteTourItem(CurrentItem);
                        MessageBox.Show("Tour Deleted", "Delete Tour");
                        this.tourItemFactory = TourFactory.GetInstance();
                        IEnumerable<TourItem> results = this.tourItemFactory.GetItems();
                        FillListBox(results);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }
    }
}
