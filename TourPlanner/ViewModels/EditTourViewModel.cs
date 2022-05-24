using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class EditTourViewModel: BaseViewModel
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TourItem currentTour;
        private ITourFactory tourFactory;

        //command
        private ICommand editTour;
        public ICommand EditTour => editTour ??= new RelayCommand(PerformEditTour);

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

        public EditTourViewModel()
        {
            this.tourFactory = TourFactory.GetInstance();
        }

        private void PerformEditTour(object commandParameter)
        {            
            if (!string.IsNullOrEmpty(CurrentTour.Name) && !string.IsNullOrEmpty(CurrentTour.From) && !string.IsNullOrEmpty(CurrentTour.To) && !string.IsNullOrEmpty(CurrentTour.Description) && !string.IsNullOrEmpty(CurrentTour.TransportTyp))
            {
                int id = CurrentTour.TourId;
                TourItem editTour = new TourItem(id, CurrentTour.Name, CurrentTour.Description, CurrentTour.From, CurrentTour.To, CurrentTour.Name, CurrentTour.Distance , CurrentTour.TransportTyp);

                //save to DB
                this.tourFactory.EditTourItem(editTour);

                // save to lof file
                log.Info("Editing Tour DONE!");

                //Save image to Folder
                this.tourFactory.SaveRouteImageFromApi(CurrentTour.From, CurrentTour.To, CurrentTour.Name);

                //Show Successfully Message 
                MessageBox.Show("Edit Tour Successfully added.");

                //Close Window
                var window = Application.Current.Windows[2];
                window.Close();

            }
        }
    }
}
