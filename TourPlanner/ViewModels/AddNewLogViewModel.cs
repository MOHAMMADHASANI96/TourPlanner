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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DateTime logDate;
        private string logDifficulty;
        private TimeSpan logTotalTime;
        private string logReport;
        private string logRating;

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
                
                // save to lof file
                log.Info("Adding new Log DONE!");

                //Show Successfully Message 
                MessageBox.Show("New TourLog Successfully added.");
                
                //empty filed 
                LogDate = DateTime.MinValue;
                LogDifficulty = string.Empty;
                LogReport = string.Empty;
                LogRating = string.Empty;
            }
        }


        private void PerformCancle(object commandParameter)
        {
            var window = Application.Current.Windows[1];
            window.Close();
        }
    }
}
