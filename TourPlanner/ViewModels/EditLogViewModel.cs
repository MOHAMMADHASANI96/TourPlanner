using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class EditLogViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TourLog currentLog;
        private TourItem currentTour;
        private ITourFactory tourFactory;
        private Window window;

        private string logDate;
        private string logDifficulty;
        private string logTotalTime;
        private string logReport;
        private string logRating;

        // for validation
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        public bool HasErrors => _errorsByPropertyName.Any();

        //command
        private ICommand editLog;
        private ICommand cancle;
        
        public ICommand EditLog => editLog ??= new RelayCommand(PerformEditLog);
        public ICommand Cancle => cancle ??= new RelayCommand(PerformCancle);

        public TourLog CurrentLog
        {
            get { return currentLog; }
            set
            {
                if ((currentLog != value) && (value != null))
                {
                    currentLog = value;
                    RaisePropertyChangedEvent(nameof(CurrentLog));
                }
            }
        }


        public TourItem CurrentTour
        {
            get { return currentTour; }
            set
            {
                if ((currentTour != value) && (value != null))
                {
                    currentTour = value;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                }
            }
        }


        public string LogDate
        {
            get { return logDate; }
            set
            {
                if ((logDate != value) && (value != null))
                {
                    logDate = value;
                    CheckLogDate();
                    RaisePropertyChangedEvent(nameof(LogDate));
                }
            }
        }

        public string LogDifficulty
        {
            get { return logDifficulty; }
            set
            {
                if ((logDifficulty != value) && (value != null))
                {
                    logDifficulty = value;
                    CheckLogDifficulty();
                    RaisePropertyChangedEvent(nameof(LogDifficulty));
                }
            }
        }

        public string LogTotalTime
        {
            get { return logTotalTime; }
            set
            {
                if ((logTotalTime != value) && (value != null))
                {
                    logTotalTime = value;
                    CheckLogTotalTime();
                    RaisePropertyChangedEvent(nameof(LogTotalTime));
                }
            }
        }

        public string LogReport
        {
            get { return logReport; }
            set
            {
                if ((logReport != value) && (value != null))
                {
                    logReport = value;
                    CheckLogReport();
                    RaisePropertyChangedEvent(nameof(LogReport));
                }
            }
        }

        public string LogRating
        {
            get { return logRating; }
            set
            {
                if ((logRating != value) && (value != null))
                {
                    logRating = value;
                    CheckLogRating();
                    RaisePropertyChangedEvent(nameof(LogRating));
                }
            }
        }

        public EditLogViewModel()
        {
            this.tourFactory = TourFactory.GetInstance();

        }


        private void PerformEditLog(object commandParameter)
        {
            if (!string.IsNullOrEmpty(LogDate) && !string.IsNullOrEmpty(LogDifficulty) && !string.IsNullOrEmpty(LogReport) && !string.IsNullOrEmpty(LogRating) && !string.IsNullOrEmpty(LogTotalTime))
            {
                TourLog editLog = new TourLog(CurrentLog.LogId,LogDate, LogReport, LogDifficulty, LogTotalTime, LogRating, currentTour);

                //save to DB
                this.tourFactory.EditTourLog(editLog);

                //save to log file
                log.Info("Editing Tour DONE!");

                //Show Successfully Message 
                MessageBox.Show("Edit Log Successfully added.");

                //Close Window
                window = Application.Current.Windows[2];
                window.Close();
            }
        }


        private void PerformCancle(object commandParameter)
        {
            var window = Application.Current.Windows[1];
            window.Close();
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


        public bool CheckLogDate()
        {
            Regex regex = new Regex(@"(([0]?[1-9]|1[0-2])\/([0]?[1-9]|1[0-9]|2[0-9]|3[0-1])\/((19|20)\d\d))$");
            ClearErrors(nameof(LogDate));

            if (string.IsNullOrEmpty(LogDate))
            {
                AddError(nameof(LogDate), "Date cannot be empty.");
                return false;
            }
            if (!regex.IsMatch(LogDate))
            {
                AddError(nameof(LogDate), "Date Time Format must be MM/DD/YYYY.");
                return false;
            }
            return true;
        }

        public bool CheckLogDifficulty()
        {
            ClearErrors(nameof(LogDifficulty));
            if (string.IsNullOrEmpty(LogDifficulty))
            {
                AddError(nameof(LogDifficulty), "Difficulty can not be empty.");
                return false;
            }
            return true;
        }

        public bool CheckLogTotalTime()
        {
            Regex regex = new Regex(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$");
            ClearErrors(nameof(LogTotalTime));
            if (string.IsNullOrEmpty(LogTotalTime))
            {
                AddError(nameof(LogTotalTime), "Total Time cannot be empty.");
                return false;
            }

            if (!regex.IsMatch(LogTotalTime) && !string.IsNullOrEmpty(LogTotalTime))
            {
                AddError(nameof(LogTotalTime), "Total Time Format must be HH:MM:SS.");
                return false;
            }

            return true;
        }

        public bool CheckLogReport()
        {
            ClearErrors(nameof(LogReport));
            if (string.IsNullOrEmpty(LogReport))
            {
                AddError(nameof(LogReport), "Report can not be empty.");
                return false;
            }
            if (LogReport.Length > 50)
            {
                AddError(nameof(LogReport), "Report has to be lower than 50 characters.");
                return false;
            }
            return true;
        }

        public bool CheckLogRating()
        {
            ClearErrors(nameof(LogRating));
            if (string.IsNullOrEmpty(LogRating))
            {
                AddError(nameof(LogRating), "Rating can not be empty.");
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
