using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.BusinessLayer.ExportGenerator;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {

        // after import file -> listbox should be change 
        public event EventHandler<bool> ImportSuccessful;

        private ITourFactory tourFactory;
        
        private TourItem currentTour;
        private bool active = false;
        private IEnumerable<TourLog> tourLogs;
        private bool importSuccessfull = false;


        //command
        private ICommand exit;
        private ICommand createPDF;
        private ICommand createExport;
        private ICommand createimport;
       


        public ICommand Exit => exit ??= new RelayCommand(PerformExit);
        public ICommand CreatePDF => createPDF ??= new RelayCommand(PerformCreatePDF);
        public ICommand CreateExport => createExport ??= new RelayCommand(PerformCreateExport);
        public ICommand CreateImport => createimport ??= new RelayCommand(PerformImport);



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
                if ((active != value))
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
            if (CurrentTour != null)
            {
                Logs = this.tourFactory.GetTourLog(CurrentTour);
                if (this.tourFactory.PdfGenerate(CurrentTour, Logs))
                {
                    MessageBox.Show("PDF File created");
                }
                    
                else
                    MessageBox.Show("PDF File doese not created");
            }
        }

        //Create Export File
        private void PerformCreateExport(object commandParameter)
        {
            ExportGenerator export = new ExportGenerator();
            if(this.tourFactory.ExportGenerate(export.Export()))
                MessageBox.Show("Export File created");
            else
                MessageBox.Show("Export File doese not created");
        }


        //Import File
        private void PerformImport(object commandParameter)
        {
            string filePath;
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            // only show json format file
            openFileDialog.Filter = "Json documents (.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;

                if (this.tourFactory.ImportFile(filePath))
                {
                    MessageBox.Show("Import successful");
                    this.importSuccessfull = true;
                    this.ImportSuccessful?.Invoke(this, this.importSuccessfull);
                }
                else
                    MessageBox.Show("Import does not successful");
            }
        }
    }
}
