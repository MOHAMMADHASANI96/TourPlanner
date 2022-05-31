using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class AddNewLogViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DateTime logDate;
        private string logDifficulty;
        private TimeSpan logTotalTime;
        private string logReport;
        private string logRating;

        // for validation
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        public bool HasErrors => _errorsByPropertyName.Any();

        private TourItem currentTour;
        private ITourFactory tourFactory;

        //command
        private ICommand addLog;
        private ICommand cancle;
        
        public ICommand AddLog => addLog ??= new RelayCommand(PerformAddLog);
        public ICommand Cancle => cancle ??= new RelayCommand(PerformCancle);


        public AddNewLogViewModel()
        {
            this.tourFactory = TourFactory.GetInstance();
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


        public DateTime LogDate
        {
            get { return logDate; }
            set
            {
                if ((logDate != DateTime.MinValue))
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

        public TimeSpan LogTotalTime
        {
            get { return logTotalTime; }
            set
            {
                if ((logTotalTime != value))
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


        private void PerformAddLog(object commandParameter)
        {
            if (!string.IsNullOrEmpty(LogDifficulty) && !string.IsNullOrEmpty(LogReport) && !string.IsNullOrEmpty(LogRating))
            {
                TourLog newLog = new TourLog(0, logDate, logReport,logDifficulty,logTotalTime,logRating,currentTour);

                //save to DB
                this.tourFactory.CreateTourLog(newLog);
                
                // save to lof file
                log.Info("Adding new Log DONE!");

                //Show Successfully Message 
                MessageBox.Show("New TourLog Successfully added.");
                
                //empty filed 
                LogDate = DateTime.MinValue;
                LogDifficulty = string.Empty;
                LogTotalTime = TimeSpan.Zero;
                LogReport = string.Empty;
                LogRating = string.Empty;
            }
            else
            {
                CheckLogDate();
                CheckLogDifficulty();
                CheckLogTotalTime();
                CheckLogReport();
                CheckLogRating();
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
            ClearErrors(nameof(LogDate));
            if (LogDate == DateTime.MinValue)
            {
                AddError(nameof(LogDate), "Date cannot be empty.");
                return false;
            }
            return true;
        }

        public bool CheckLogDifficulty()
        {
            ClearErrors(nameof(LogDifficulty));
            if (string.IsNullOrWhiteSpace(LogDifficulty))
            {
                AddError(nameof(LogDifficulty), "Difficulty can not be empty.");
                return false;
            }
            return true;
        }

        public bool CheckLogTotalTime()
        {
            ClearErrors(nameof(LogTotalTime));
            if (logTotalTime == TimeSpan.Zero)
            {
                AddError(nameof(LogTotalTime), "TotalTime cannot be Ziro.");
                return false;
            }
            return true;
        }

        public bool CheckLogReport()
        {
            ClearErrors(nameof(LogReport));
            if (string.IsNullOrWhiteSpace(LogReport))
            {
                AddError(nameof(LogReport), "Report can not be empty.");
                return false;
            }
            return true;
        }

        public bool CheckLogRating()
        {
            ClearErrors(nameof(LogRating));
            if (string.IsNullOrWhiteSpace(LogRating))
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
