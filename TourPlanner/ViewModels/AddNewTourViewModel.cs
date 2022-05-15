﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class AddNewTourViewModel: BaseViewModel
    {
        private string tourName;
        private string tourDescription;
        private string tourFrom;
        private string tourTo;
        private string tourDistance;
        private string tourTransportTyp;


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

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
            if (!string.IsNullOrEmpty(TourName) && !string.IsNullOrEmpty(TourFrom) && !string.IsNullOrEmpty(TourTo) && !string.IsNullOrEmpty(TourDescription) && !string.IsNullOrEmpty(TourTransportTyp))
            {
           
             
                TourItem newTour = new TourItem(0, tourName, tourDescription, tourFrom, tourTo, tourName, tourDistance, tourTransportTyp);

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

        private void PerformCancle(object commandParameter)
        {
            var window = Application.Current.Windows[1];
            window.Close();
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

        public string TourTransportTyp
        {
            get { return tourTransportTyp; }
            set
            {
                if ((tourTransportTyp != value) && (value != null))
                {
                    tourTransportTyp = value;
                    RaisePropertyChangedEvent(nameof(TourTransportTyp));
                }
            }
        }
    }
}
