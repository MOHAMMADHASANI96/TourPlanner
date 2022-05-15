using System.Windows;
using TourPlanner.ViewModels;

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var searchBarVM = new SearchBarViewModel();
            var tourVM = new TourViewModel();
            var menuVM = new MenuViewModel();

            var wnd = new MainWindow
            {
                DataContext = new MainViewModel(tourVM, searchBarVM, menuVM),
                SearchBar = { DataContext = searchBarVM },
                Tour = { DataContext = tourVM },
                Menu = {DataContext = menuVM}
            };

            wnd.Show();
        }
    }
}
