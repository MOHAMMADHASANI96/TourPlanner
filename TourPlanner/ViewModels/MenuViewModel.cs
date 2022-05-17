using System.Collections.Generic;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private ITourFactory tourFactory;
        
        
        private TourItem currentTour;
        private bool active = false;
        private IEnumerable<TourLog> tourLogs;


        //command
        private ICommand exit;
        private ICommand createPDF;


      
        public ICommand Exit => exit ??= new RelayCommand(PerformExit);
        public ICommand CreatePDF => createPDF ??= new RelayCommand(PerformCreatePDF);
        


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

        public bool Active
        {
            get { return active; }
            set
            {
                if ((active != value) && (value != null))
                {
                    active = value;
                    RaisePropertyChangedEvent(nameof(Active));
                }
            }
        }

        public IEnumerable<TourLog> Logs
        {
            get
            {
                return tourLogs;
            }
            set
            {
                if ((tourLogs != value) && (value != null))
                {
                    tourLogs = value;
                    RaisePropertyChangedEvent(nameof(Logs));
                }
            }
        }

        public MenuViewModel()
        {
            this.tourFactory = TourFactory.GetInstance();
        }

        //Close Window
        private void PerformExit(object commandParameter)
        {
            App.Current.MainWindow.Close();
        }

        //Create PDF file
        private void PerformCreatePDF(object commandParameter)
        {
            if(CurrentTour != null)
            {
                Logs = this.tourFactory.GetTourLog(CurrentTour);
                this.tourFactory.PdfGenerate(CurrentTour, Logs);
            }
        }
    }
}
