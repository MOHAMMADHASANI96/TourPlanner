using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class EditLogViewModel : BaseViewModel
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TourLog currentLog;
        private TourItem currentTour;
        private ITourFactory tourFactory;
        private Window window;

        //command
        private ICommand editLog;
        private ICommand cancle;
        
        public ICommand EditLog => editLog ??= new RelayCommand(PerformEditLog);
        public ICommand Cancle => cancle ??= new RelayCommand(PerformCancle);

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

        public EditLogViewModel()
        {
            this.tourFactory = TourFactory.GetInstance();

        }


        private void PerformEditLog(object commandParameter)
        {
            if (!string.IsNullOrEmpty(CurrentLog.Difficulty) && !string.IsNullOrEmpty(CurrentLog.Rating) && !string.IsNullOrEmpty(CurrentLog.Report))
            {
                TourLog editLog = new TourLog(CurrentLog.LogId,CurrentLog.DateTime,CurrentLog.Report,CurrentLog.Difficulty,CurrentLog.TotalTime,CurrentLog.Rating,currentTour);

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
    }
}
