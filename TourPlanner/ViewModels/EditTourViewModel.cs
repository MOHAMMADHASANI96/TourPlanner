using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class EditTourViewModel: BaseViewModel
    {
        private string tourName;
        private string tourDescription;
        private string tourFrom;
        private string tourTo;
        private TourItem currentTour;

        private ITourFactory tourFactory;

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

            if (!string.IsNullOrEmpty(CurrentTour.Name) && !string.IsNullOrEmpty(CurrentTour.From) && !string.IsNullOrEmpty(CurrentTour.To) && !string.IsNullOrEmpty(CurrentTour.Description))
            {
                int id = CurrentTour.TourId;
                TourItem editTour = new TourItem(id, CurrentTour.Name, CurrentTour.Description, CurrentTour.From, CurrentTour.To, CurrentTour.Name, 0);

                //save to DB
                this.tourFactory.EditTourItem(editTour);
                //save image to Folder
                this.tourFactory.SaveRouteImageFromApi(CurrentTour.From, CurrentTour.To, CurrentTour.Name);

                MessageBox.Show("Edit Tour Successfully added.");

            }
        }
    }
}
