using Microsoft.Win32;
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                
                // SaveFileDialog settings
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Save an PDF File";
                saveFileDialog.InitialDirectory = @"D:\informatik\SS2022\SWE2\TourPlanner\RoutePDF";
                string fileName = CurrentTour.Name;
                saveFileDialog.FileName = fileName;
                saveFileDialog.DefaultExt = "pdf";
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;


                if (saveFileDialog.ShowDialog() == true)
                {

                    if (this.tourFactory.PdfGenerate(CurrentTour, Logs , saveFileDialog.FileName))
                    {
                        MessageBox.Show("PDF File created");

                        //save to log file
                        log.Info("Creating PDF File DONE!");
                    }

                    else
                    {
                        MessageBox.Show("PDF File doese not created");

                        //save to log file
                        log.Info("FAILED Creating PDF File!");
                    }

                }
            }
        }

        //Create Export File
        private void PerformCreateExport(object commandParameter)
        {

            // SaveFileDialog settings
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save an Export File";
            saveFileDialog.InitialDirectory = @"D:\informatik\SS2022\SWE2\TourPlanner\RouteExport";
            string fileName = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            saveFileDialog.FileName = fileName;
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;


            if (saveFileDialog.ShowDialog() == true)
            {

                ExportGenerator export = new ExportGenerator();
                if (this.tourFactory.ExportGenerate(export.Export(), saveFileDialog.FileName))
                {
                    MessageBox.Show("Export File created");
                    //save to log file
                    log.Info("Creating Export File DONE!");
                }

                else
                {
                    MessageBox.Show("Export File doese not created");
                    //save to log file
                    log.Info("FAILED Creating Export File!");
                }
            }  
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
                    //save to log file
                    log.Info("Importing File DONE!");

                    MessageBox.Show("Import successful");
                    this.importSuccessfull = true;
                    this.ImportSuccessful?.Invoke(this, this.importSuccessfull);
                        
                }
                else
                {
                    MessageBox.Show("Import does not successful");
                    //save to log file
                    log.Info("FAILED importing File!");
                }
                    
            }
        }
    }
}
