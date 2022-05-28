using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class AddNewTourViewModel: BaseViewModel,IDataErrorInfo
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string tourName;
        private string tourDescription;
        private string tourFrom;
        private string tourTo;
        private string tourDistance;
        private string tourTransportType;

        public event PropertyChangedEventHandler PropertyChanged;

        private ITourFactory tourFactory;

        private ICommand addNewTourCommand;
        private ICommand cancle;
     

        public ICommand AddTour => addNewTourCommand ??= new RelayCommand(AddNewTour);
        public ICommand Cancle => cancle ??= new RelayCommand(PerformCancle);

        public AddNewTourViewModel()
        {
            this.tourFactory = TourFactory.GetInstance();
        }

        private void AddNewTour(object commandParameter)
        {
            if (!string.IsNullOrEmpty(TourName) && !string.IsNullOrEmpty(TourFrom) && !string.IsNullOrEmpty(TourTo) && !string.IsNullOrEmpty(TourDescription) && !string.IsNullOrEmpty(TourTransportType))
            {
                TourItem newTour = new TourItem(0, tourName, tourDescription, tourFrom, tourTo, tourName, tourDistance, tourTransportType);

                //save to DB
                this.tourFactory.CreateTourItem(newTour);

                //save image to Folder
                this.tourFactory.SaveRouteImageFromApi(TourFrom, TourTo, TourName);

                //show successfully message
                MessageBox.Show("New Tour Successfully added.");
                
                //save to log file
                log.Info("Adding new Tour DONE!");
                
                //empty all field
                TourName = string.Empty;
                TourFrom = string.Empty;
                TourTo = string.Empty;
                TourDescription = string.Empty;
                TourDistance = string.Empty;
            }
            else
            {
                MessageBox.Show(Error);
                //save to log file
                log.Info("FAILED to add a new tour!");
            }

        }

        private void PerformCancle(object commandParameter)
        {
            var window = Application.Current.Windows[1];
            window.Close();
        }


       
        public string TourDistance
        {
            get { return tourDistance; }
            set
            {
                if ((tourDistance != value) && (value != null))
                {
                    tourDistance = value;
                    RaisePropertyChangedEvent(nameof(TourDistance));
                }
            }
        }

        public string TourName
        {
            get { return tourName; }
            set
            {
                if ((tourName != value) && (value != null))
                {
                    tourName = value;
                    RaisePropertyChangedEvent(nameof(TourName));
                }
            }
        }


        public string TourDescription
        {
            get { return tourDescription; }
            set
            {
                if ((tourDescription != value) && (value != null))
                {
                    tourDescription = value;
                    RaisePropertyChangedEvent(nameof(TourDescription));
                }
            }
        }

        public string TourFrom
        {
            get { return tourFrom; }
            set
            {
                if ((tourFrom != value) && (value != null))
                {
                    tourFrom = value;
                    RaisePropertyChangedEvent(nameof(TourFrom));
                }
            }
        }

        public string TourTo
        {
            get { return tourTo; }
            set
            {
                if ((tourTo != value) && (value != null))
                {
                    tourTo = value;
                    RaisePropertyChangedEvent(nameof(TourTo));
                }
            }
        }

        public string TourTransportType
        {
            get { return tourTransportType; }
            set
            {
                if ((tourTransportType != value) && (value != null))
                {
                    tourTransportType = value;
                    RaisePropertyChangedEvent(nameof(TourTransportType));
                }
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return GetErrorForProperty(propertyName);
            }
        }

        public string Error { get; set; } = "";
        private string GetErrorForProperty(string propertyName)
        {
            Error = "";

            switch (propertyName)
            {
                case "TourName":
                    if (string.IsNullOrEmpty(TourName))
                    {
                        Error = "TourName length must be >= 5";
                        return Error;
                    }
                    break;
            }

            return string.Empty;
        }


        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
