using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using FolderBrowser.ViewModel.Implementation;

namespace FolderBrowser.ViewModel
{
    /// <summary>
    /// The ViewModel implementation for the MainDialogbox through which subsequent
    /// Folder Browser and Display Browser are spawned
    /// </summary>
    public class MainDialogViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region private Fields
        private string _selectedDirectory = String.Empty;
        private readonly IDialogService dialogService;
        private readonly IFolderBrowserDialog dialog;
        #endregion

        #region Public Properties/Commands"
        public ICommand ShowDirectoryBrowserCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public string SelectedDirectory
        {
            get
            {
                return (string)GetValue(SelectedDirectoryProperty);
            }

            set
            {
                SetValue(SelectedDirectoryProperty, value);
            }
        }
        #endregion

        #region "Dependency property"
        public static readonly DependencyProperty SelectedDirectoryProperty =
            DependencyProperty.Register("SelectedDirectory", 
                                         typeof(string),
                                         typeof(MainDialogViewModel));

        #endregion
        /// <summary>
        /// Default Constructor which initializes FolderBrowserDialog and DialogService
        /// </summary>
        public MainDialogViewModel() : this (ServiceLocator.Resolve<IFolderBrowserDialog>(), ServiceLocator.Resolve<IDialogService>())
        {
            ShowDirectoryBrowserCommand = new DelegateCommand(ShowDirectoryBrowser, CanShowDirectoryBrowser);
            NextCommand = new DelegateCommand(NextProcessingStep, CanNextProcessingProceed);
            CancelCommand = new DelegateCommand(CancelProcessing, CanCancelProcessing);
        }
         
        /// <summary>
        /// Secondary constructor which initializes the dialog service.
        /// </summary>
        /// <param name="dialog_"></param>
        /// <param name="dialogSvc_"></param>
        public MainDialogViewModel(IFolderBrowserDialog dialog_, IDialogService dialogSvc_)
        {
            this.dialog = dialog_;
            this.dialogService = dialogSvc_;
        }

        /// <summary>
        /// Command Implementation for the Next Button which fires up the Folder Browser
        /// </summary>
        /// <param name="parameter">object</param>
        private void NextProcessingStep(object parameter)
        {
            IDisplayBrowserDialog browserDialog = new DisplayBrowserDialogViewModel();
            browserDialog.SelectedPath = SelectedDirectory;
            dialogService.ShowDialog(this, browserDialog);

        }

        /// <summary>
        /// Can Next Button be enabled decision maker which allows Next button to be enabled
        /// based on whether a valid directory has been picked up for browsing
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>boolean which indicates whether Next button can be enabled or not</returns>
        private bool CanNextProcessingProceed(object parameter)
        {
            return Directory.Exists(SelectedDirectory) ? true : false;            
        }


        /// <summary>
        /// Closes the main window
        /// </summary>
        /// <param name="parameter"></param>
        private void CancelProcessing(object parameter)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Can Cancel processing decision maker
        /// </summary>
        /// <param name="parameter">object</param>
        /// <returns>bool indicating whether cancel can be cancelled !</returns>
        private bool CanCancelProcessing(object parameter)
        {
            return true;
        }

        /// <summary>
        /// This method will spawn the Directory Browser and populate the grid
        /// with the files found
        /// </summary>
        /// <param name="parameter">object</param>
        private void ShowDirectoryBrowser(object parameter)
        {
            IFolderBrowserDialog dialog = new FolderBrowserDialogViewModel();
            dialog.SelectedPath = String.IsNullOrEmpty(SelectedDirectory) ? "" : SelectedDirectory;
            dialogService.ShowFolderBrowserDialog(this, dialog);
            SelectedDirectory = dialog.SelectedPath;
        }

        /// <summary>
        /// Method which determines if the Directory Browser can be shown or not
        /// Directly linked to the "..." button
        /// </summary>
        /// <param name="parameter">parameter</param>
        /// <returns>bool indicating if directory browser can be shown</returns>
        private bool CanShowDirectoryBrowser(object parameter)
        {
            return true;
        }
    }
}
