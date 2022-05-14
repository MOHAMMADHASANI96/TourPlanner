using System;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class AddNewLogViewModel : BaseViewModel
    {
        private DateTime logDate;
        private string logDifficulty;
        private TimeSpan logTotalTime;
        private string logReport;
        private string logRating;

        private TourItem currentTour;
        private ITourFactory tourFactory;

        //command
        private ICommand addLog;
        public ICommand AddLog => addLog ??= new RelayCommand(PerformAddLog);


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
                if ((logDate != value))
                {
                    logDate = value;
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

                MessageBox.Show("New TourLog Successfully added.");
                LogDate = DateTime.MinValue;
                LogDifficulty = string.Empty;
                LogReport = string.Empty;
                LogRating = string.Empty;
            }
        }
    }
}
