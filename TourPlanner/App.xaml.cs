using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

            var wnd = new MainWindow
            {
                DataContext = new MainViewModel(tourVM, searchBarVM),
                SearchBar = { DataContext = searchBarVM },
                Tour = { DataContext = tourVM }
            };

            wnd.Show();
        }
    }
}
