using System.Windows.Input;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {

        //command
        private ICommand exit;
        public ICommand Exit => exit ??= new RelayCommand(PerformExit);

        public MenuViewModel()
        {

        }
        private void PerformExit(object commandParameter)
        {
            App.Current.MainWindow.Close();
        }
       

    }
}
