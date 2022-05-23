using System;
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
    public class TourViewModel : BaseViewModel
    {
        // if current item selected -> active button pdf  
        public event EventHandler<TourItem> CurrentItemChanged;

        private ITourFactory tourItemFactory;
        private IEnumerable<TourLog> tourLogs;
       

        private TourItem currentItem;
        private TourLog currentLog;

        public EditTourViewModel editTourViewModel;
        public EditLogViewModel editLogViewModel;
        public AddNewLogViewModel addNewLogViewModel;
        public MenuViewModel menuViewModel;

        //Tour items variable 
        private string currentTourImagePath;

        // TourLog items variable 
        private string dateTime;
        private string report;
        private string distance;
        private string totalTime;
        private string rating;
        private string description;

        //command
        private ICommand addNewTour;
        private ICommand editTour;
        private ICommand deleteTour;
        private ICommand addNewLog;
        private ICommand editLog;
        private ICommand deleteLog;



        public ICommand AddNewTour => addNewTour ??= new RelayCommand(PerformAddNewTour);
        public ICommand EditTour => editTour ??= new RelayCommand(PerformEditTour);
        public ICommand DeleteTour => deleteTour ??= new RelayCommand(PerformDeleteTour);
        public ICommand AddNewLog => addNewLog ??= new RelayCommand(PerformAddNewLog);
        public ICommand EditLog => editLog ??= new RelayCommand(PerformEditLog);
        public ICommand DeleteLog => deleteLog ??= new RelayCommand(PerformDeleteLog);



        public ObservableCollection<TourItem> TourItems { get; set; }
        public ObservableCollection<TourLog> TourLogs { get; set; }

        public TourViewModel()
        {
            TourItems = new ObservableCollection<TourItem>();
            TourLogs = new ObservableCollection<TourLog>();
            menuViewModel = new MenuViewModel();

        }

        // Fill Tour ListBox
        public void FillListBox(IEnumerable<TourItem> tourItems)
        {
            TourItems.Clear();
            foreach (TourItem item in tourItems)
            {
                TourItems.Add(item);
            }
        }

        // Fill Log ListBox
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
                    
                    this.description = DescriptionText(CurrentItem);
                    RaisePropertyChangedEvent(nameof(Description));

                    this.CurrentItemChanged?.Invoke(this, CurrentItem);

                    FillLogBox(this.tourItemFactory.GetTourLog(currentItem));

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
                    RaisePropertyChangedEvent(nameof(CurrentLog));
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

        private string DescriptionText(TourItem tourItem)
        {
            var descriptionText = "Name of tour: " + tourItem.Name +
                                  "\nTour from : " + tourItem.From +
                                  "\nTour Destination: " + tourItem.To +
                                  "\nTour Distance: " + tourItem.Distance +
                                  "\nTransport Typ: " + tourItem.TransportTyp +
                                  "\nOther Description: " + tourItem.Description;

            return descriptionText;

        }

        public string Description
        {
            get { return description; }
            set
            {
                if ((description != value) && (value != null))
                {
                    description = value;
                    RaisePropertyChangedEvent(nameof(Description));
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


        // add new Tour 
        private void PerformAddNewTour(object commandParameter)
        {
            AddNewTourView addTour = new AddNewTourView();
            addTour.DataContext = new AddNewTourViewModel();
            addTour.ShowDialog();
            this.tourItemFactory = TourFactory.GetInstance();
            IEnumerable<TourItem> result = this.tourItemFactory.GetItems();
            FillListBox(result);

        }

        // edit Tour
        private void PerformEditTour(object commandParameter)
        {
            if (CurrentItem != null)
            {
                currentTourImagePath = null;
                RaisePropertyChangedEvent(nameof(CurrentTourImagePath));
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

        // delete Tour
        private void PerformDeleteTour(object commandParameter)
        {
            if (CurrentItem != null)
            {
                //clear image path
                currentTourImagePath = null;
                RaisePropertyChangedEvent(nameof(CurrentTourImagePath));
                MessageBoxResult result = MessageBox.Show("Would you like to Delete this Tour?", "Delete Tour", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        {
                            //Clear LogBox
                            TourLogs.Clear();
                            //Delete Tour
                            this.tourItemFactory.DeleteTourItem(CurrentItem);
                            MessageBox.Show("Tour Deleted", "Delete Tour");
                            this.tourItemFactory = TourFactory.GetInstance();
                            IEnumerable<TourItem> results = this.tourItemFactory.GetItems();
                            FillListBox(results);
                            break;
                        }
                        
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        // add new Tour Log
        private void PerformAddNewLog(object commandParameter)
        {
            this.addNewLogViewModel = new AddNewLogViewModel();
            addNewLogViewModel.CurrentTour = CurrentItem;
            AddNewLogView addLog = new AddNewLogView();
            addLog.DataContext = this.addNewLogViewModel;
            addLog.ShowDialog();
            this.tourItemFactory = TourFactory.GetInstance();
            FillLogBox(this.tourItemFactory.GetTourLog(currentItem));
        }

        // edit Tour Log
        private void PerformEditLog(object commandParameter)
        {
            if (CurrentLog != null)
            {
                this.editLogViewModel = new EditLogViewModel();
                editLogViewModel.CurrentLog = CurrentLog;
                EditLogView editLog = new EditLogView();
                editLog.DataContext = this.editLogViewModel;
                editLog.ShowDialog();
                this.tourItemFactory = TourFactory.GetInstance();
                FillLogBox(this.tourItemFactory.GetTourLog(currentItem));
            }
        }



        // delete Tour Log
        private void PerformDeleteLog(object commandParameter)
        {
            if (CurrentLog != null)
            {
                MessageBoxResult result = MessageBox.Show("Would you like to Delete this Log?", "Delete Log", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        this.tourItemFactory.DeleteTourLog(CurrentLog);
                        MessageBox.Show("Log Deleted", "Delete Log");
                        this.tourItemFactory = TourFactory.GetInstance();
                        FillLogBox(this.tourItemFactory.GetTourLog(currentItem));
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }
    }
}
