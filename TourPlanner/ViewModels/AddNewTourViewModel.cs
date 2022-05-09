using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class AddNewTourViewModel: BaseViewModel
    {
        private string tourName;
        private string tourDescription;
        private string tourFrom;
        private string tourTo;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        private ITourFactory tourFactory;

        private ICommand addNewTourCommand;

        public ICommand AddTour => addNewTourCommand ??= new RelayCommand(AddNewTour);

        public AddNewTourViewModel()
        {
            this.tourFactory = TourFactory.GetInstance();
        }

        private void AddNewTour(object commandParameter)
        {
            if (!string.IsNullOrEmpty(TourName) && !string.IsNullOrEmpty(TourFrom) && !string.IsNullOrEmpty(TourTo) && !string.IsNullOrEmpty(TourDescription))
            {
                int id = this.tourFactory.GetLastTourId();
                TourItem newTour = new TourItem(id, tourName, tourDescription, tourFrom, tourTo, tourName, 0);

                //save to DB
                this.tourFactory.CreateTourItem(newTour);
                //save image to Folder
                this.tourFactory.SaveRouteImageFromApi(TourFrom, TourTo, TourName);

                MessageBox.Show("New Tour Successfully added.");
                TourName = string.Empty;
                TourFrom = string.Empty;
                TourTo = string.Empty;
                TourDescription = string.Empty;
            }
            else
            {
                CheckInputTourName();
                CheckInputTourFrom();
                CheckInputTourTo();
                CheckInputTourDescription();
            }

        }


        public bool CheckInputTourName()
        {
            ClearErrors(nameof(TourName));
            if (string.IsNullOrWhiteSpace(TourName))
            {
                AddError(nameof(TourName), "Name cannot be empty.");
                return false;
            }
            return true;
        }

        public bool CheckInputTourFrom()
        {
            ClearErrors(nameof(TourFrom));
            if (string.IsNullOrWhiteSpace(TourFrom))
            {
                AddError(nameof(TourFrom), "Starting Point cannot be empty.");
                return false;
            }
            return true;
        }

        public bool CheckInputTourTo()
        {
            ClearErrors(nameof(TourTo));
            if (string.IsNullOrWhiteSpace(TourTo))
            {
                AddError(nameof(TourTo), "Destination cannot be empty.");
                return false;
            }
            return true;
        }

        public bool CheckInputTourDescription()
        {
            ClearErrors(nameof(TourDescription));
            if (string.IsNullOrWhiteSpace(TourDescription))
            {
                AddError(nameof(TourDescription), "Description cannot be empty.");
                return false;
            }
            return true;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
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


        private double tourDistance()
        {
            // --------------------- 
            return 0;
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
    }
}
