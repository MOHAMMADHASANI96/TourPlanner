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
        private TourLog currentLog;
        private TourItem currentTour;
        private ITourFactory tourFactory;
        private Window window;

        //command
        private RelayCommand editLog;
        public ICommand EditLog => editLog ??= new RelayCommand(PerformEditLog);

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

                //Show Successfully Message 
                MessageBox.Show("Edit Log Successfully added.");

                //Close Window
                window = Application.Current.Windows[2];
                window.Close();
            }
        }
    }
}
