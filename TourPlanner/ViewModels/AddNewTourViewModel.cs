using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class AddNewTourViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string tourName;
        private string tourDescription;
        private string tourFrom;
        private string tourTo;
        private string tourDistance;
        private string tourTransportType;
        private Window window;

        public event PropertyChangedEventHandler PropertyChanged;

        // for validation
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        public bool HasErrors => _errorsByPropertyName.Any();

        private ITourFactory tourFactory;

        private ICommand addNewTourCommand;
        private ICommand cancle;


        public ICommand AddTour => addNewTourCommand ??= new RelayCommand(AddNewTour);
        public ICommand Cancle => cancle ??= new RelayCommand(PerformCancle);

        public AddNewTourViewModel()
        {
            this.tourFactory = TourFactory.GetInstance();
        }

        private void PerformCancle(object commandParameter)
        {
            var window = Application.Current.Windows[1];
            window.Close();
        }


        private void AddNewTour(object commandParameter)
        {
            if (!string.IsNullOrEmpty(TourName)&& CheckTourName() && !string.IsNullOrEmpty(TourFrom) && !string.IsNullOrEmpty(TourTo) && !string.IsNullOrEmpty(TourDescription) && !string.IsNullOrEmpty(TourTransportType))
            {
                TourItem newTour = new TourItem(0, tourName, tourDescription, tourFrom, tourTo, tourName, tourDistance, tourTransportType);

                //save to DB
                if(this.tourFactory.CreateTourItem(newTour) != null)
                {
                    //save image to Folder
                    this.tourFactory.SaveRouteImageFromApi(TourFrom, TourTo, TourName);

                    //show successfully message
                    MessageBox.Show("New Tour Successfully added.");

                    //save to log file
                    log.Info("Adding new Tour DONE!");

                    //Close Window
                    window = Application.Current.Windows[2];
                    window.Close();
                }
                else
                {
                    //show Faild message
                    MessageBox.Show("New Tour Successfully Does not added.");

                    //save to log file
                    log.Info("Adding new Tour FAILD!");
                }

                
            }
            else
            {
                CheckTourName();
                CheckTourFrom();
                CheckTourTo();
                CheckTourDistance();
                CheckTourTransportType();
                CheckTourDescription();
                //save to log file
                log.Info("FAILED to add a new tour!");
            }

        }

        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string TourName
        {
            get { return tourName; }
            set
            {
                if ((tourName != value) && (value != null))
                {
                    tourName = value;
                    CheckTourName();
                    RaisePropertyChangedEvent(nameof(TourName));
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
                    CheckTourFrom();
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
                    CheckTourTo();
                    RaisePropertyChangedEvent(nameof(TourTo));
                }
            }
        }

        public string TourDistance
        {
            get { return tourDistance; }
            set
            {
                if ((tourDistance != value) && (value != null))
                {
                    tourDistance = value;
                    CheckTourDistance();
                    RaisePropertyChangedEvent(nameof(TourDistance));
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
                    CheckTourTransportType();
                    RaisePropertyChangedEvent(nameof(TourTransportType));
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
                    CheckTourDescription();
                    RaisePropertyChangedEvent(nameof(TourDescription));
                }
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
            _errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool CheckTourName()
        {
            ClearErrors(nameof(TourName));
            if (TourName == null)
            {
                AddError(nameof(TourName), "Tour Name cannot be empty.");
                return false;
            }
            else if(this.tourFactory.FindTourItemByName(TourName) != null)
            {
                AddError(nameof(TourName), "This tour name has already been registered.Please define another Tour Name");
                return false;
            }
            else
                return true;
        }

        public bool CheckTourFrom()
        {
            ClearErrors(nameof(TourFrom));
            if (string.IsNullOrWhiteSpace(TourFrom))
            {
                AddError(nameof(TourFrom), "Origin can not be empty");
                return false;
            }
            return true;
        }

        public bool CheckTourTo()
        {
            ClearErrors(nameof(TourTo));
            if (string.IsNullOrWhiteSpace(TourTo))
            {
                AddError(nameof(TourTo), "Destination can not be empty");
                return false;
            }
            return true;
        }

        public bool CheckTourDistance()
        {
            bool res;
            float distance;
            res = float.TryParse(TourDistance, out distance);
            ClearErrors(nameof(TourDistance));
            if (string.IsNullOrWhiteSpace(TourDistance))
            {
                AddError(nameof(TourDistance), "Distance can not be empty");
                return false;
            }
            if (!res)
            {
                AddError(nameof(TourDistance), "Distance has to be a float.");
                return false;
            }
            else
            {
                if ((distance < 1) || (distance > 10000))
                {
                    AddError(nameof(TourDistance), "Distance has to be between 1 and 10000 km.");
                    return false;
                }
            }
            return true;
        }

        public bool CheckTourTransportType()
        {
            ClearErrors(nameof(TourTransportType));
            if (string.IsNullOrWhiteSpace(TourTransportType))
            {
                AddError(nameof(TourTransportType), "Transport Type cannot be empty.");
                return false;
            }
            return true;
        }

        public bool CheckTourDescription()
        {
            ClearErrors(nameof(TourDescription));
            if (string.IsNullOrWhiteSpace(TourDescription))
            {
                AddError(nameof(TourDescription), "Description cannot be empty.");
                return false;
            }
            return true;
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}