using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FolderBrowser.ViewModel.Implementation;
using FolderBrowser.ViewModel;
using FolderBrowser.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Windows.Data;
using System.Threading;
using System.Diagnostics;
using FolderBrowser.ViewModel.Interface;

namespace FolderBrowser.ViewModel
{
    /// <summary>
    /// The second most important ViewModel class handling all aspects of the Display Browser
    /// which shows the files found and processes requests in a background worker thread
    /// </summary>
    public class DisplayBrowserDialogViewModel : ViewModelBase, IDisplayBrowserDialog
    {
        #region "Private Member Fields"
        private BackgroundWorker worker;
        private static readonly object _syncLock = new object();
        private ObservableCollection<FileObject> _list = new ObservableCollection<FileObject>();
        private bool _recurseDirectories = false;
        private double _percentUpdate = 0.0;
        private readonly IDialogService dialogService;
        private string _searchString = String.Empty;
        private readonly ICollectionView _fileListView;
        #endregion

        #region "Public Properties"
        public ICommand CancelCommand  { get; set; }
        public ICommand OpenCommand    { get; set; }
        public ICommand DetailsCommand { get; set; }
        public ICommand ShowInFinderCommand {get;set;}
        public ICommand CopyPathCommand {get;set;}

        /// <summary>
        /// Contains the Count which indicates the number of files found
        /// </summary>
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// The Search Text String to search for in the datagrid
        /// </summary>
        public string SearchString
        {
            get
            {
                return _searchString;
            }

            set
            {
                _searchString = value;
                OnPropertyChanged("SearchString");
                _fileListView.Refresh();
            }
        }

        /// <summary>
        /// The Items View Collection
        /// </summary>
        public ICollectionView FileListView
        {
            get { return _fileListView; }
        }

        /// <summary>
        /// The Core List of Items bound to the DataGrid
        /// This contains the list of files bound to the datagrid
        /// </summary>
        public ObservableCollection<FileObject> FileList
        {
            get
            {
                return _list;
            }
            set
            {
                _list = value;
            }
        }

        /// <summary>
        /// The Percent Update Property bound to the View
        /// </summary>
        public double PercentUpdate
        {
            get
            {
                return _percentUpdate;
            }
            set
            {
                _percentUpdate = value;
                OnPropertyChanged("PercentUpdate");
            }
        }

        /// <summary>
        /// Property to store the state of the RecurseDirectory Checkbox
        /// </summary>
        public bool RecurseDirectories
        {
            get
            {
                return _recurseDirectories;
            }
            set
            {
                _recurseDirectories = value;
                OnPropertyChanged("RecurseDirectories");
            }
        }

        /// <summary>
        /// Contains the Object selected/highlighted in the datagrid
        /// for carrying out further operations
        /// </summary>
        public FileObject ItemToOpen
        {
            get;
            set;
        }

        #region "Command Implementation"
        /// <summary>
        /// Opens File in the associated file viewer
        /// </summary>
        /// <param name="parameter"></param>
        public void OpenFile(object parameter)
        {
            if (ItemToOpen != null)
            {
                Process.Start(Path.Combine(ItemToOpen.DirectoryName, ItemToOpen.Name));
            }
            else
            {
                MessageBox.Show("Please Select an entry from the Data Grid", "Grid Item Not Selected", MessageBoxButton.OKCancel);
            }
        }

        /// <summary>
        /// File Detail display modal window
        /// </summary>
        /// <param name="parameter"></param>
        public void DetailFileInfo(object parameter)
        {
            if (ItemToOpen != null)
            {
                IDetailFileInfo infoDialog = new DetailFileInfoViewModel();
                var properties = new ObservableCollection<FileDetailObject>();
                properties.Add(new FileDetailObject() { Key = "FullName", Value = Path.Combine(ItemToOpen.DirectoryName, ItemToOpen.Name) });
                properties.Add(new FileDetailObject() { Key = "ParentDirectory", Value = ItemToOpen.DirectoryName });
                properties.Add(new FileDetailObject() { Key = "Attributes", Value = ItemToOpen.Attributes.ToString() });
                properties.Add(new FileDetailObject() { Key = "CreationTime", Value = ItemToOpen.CreationTime.ToString() });
                properties.Add(new FileDetailObject() { Key = "DirectoryName", Value = ItemToOpen.DirectoryName });
                properties.Add(new FileDetailObject() { Key = "Extension", Value = ItemToOpen.Extension });
                properties.Add(new FileDetailObject() { Key = "LastAccessTime", Value = ItemToOpen.LastAccessTime.ToString() });
                infoDialog.SelectedFileItem = properties;
                dialogService.ShowDialog(this, infoDialog);
            }
            else
            {
                MessageBox.Show("Please Select an entry from the Data Grid", "Grid Item Not Selected", MessageBoxButton.OKCancel);
            }
        }

        /// <summary>
        /// Opens the selected file in the windows explorer
        /// </summary>
        /// <param name="parameter"></param>
        public void ShowInFinder(object parameter)
        {
            if (ItemToOpen != null)
            {
                ProcessStartInfo info = new ProcessStartInfo("explorer");
                info.Arguments = ItemToOpen.DirectoryName;
                info.UseShellExecute = true;
                info.WindowStyle = ProcessWindowStyle.Normal;

                Process.Start(info);
            }
            else
            {
                MessageBox.Show("Please Select an entry from the Data Grid", "Grid Item Not Selected", MessageBoxButton.OKCancel);
            }
        }

        /// <summary>
        /// Copy Selected Grid File Full Path into Clipboard for later use
        /// </summary>
        /// <param name="parameter"></param>
        public void CopyToClipBoard(object parameter)
        {
          //Clipboard.SetData("FileObject",ItemToOpen);
            Clipboard.SetText(Path.Combine(ItemToOpen.DirectoryName, ItemToOpen.Name));
        }
        #endregion

        #endregion

        /// <summary>
        /// Zero Argument constructor
        /// </summary>
        public DisplayBrowserDialogViewModel() : this (ServiceLocator.Resolve<IDialogService>())
        {
        }

        /// <summary>
        /// The Secondary Constructor invoked by the no argument constructor
        /// Initializes commands, registers background worker callbacks, performs task of finding directories
        /// Reports progress
        /// </summary>
        /// <param name="dialogService"></param>
        public DisplayBrowserDialogViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += worker_ProgressChanged;

            this.PropertyChanged += DisplayBrowserDialogViewModel_PropertyChanged;
            BindingOperations.EnableCollectionSynchronization(_list, _syncLock);//most important
            this.OpenCommand = new DelegateCommand(OpenFile);
            this.DetailsCommand = new DelegateCommand(DetailFileInfo);
            this.ShowInFinderCommand = new DelegateCommand(ShowInFinder);
            this.CopyPathCommand = new DelegateCommand(CopyToClipBoard);

            _fileListView = CollectionViewSource.GetDefaultView(FileList);
            _fileListView.Filter = model => SearchInDataGrid(model as FileObject);
            //_fileListView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Search Function implementation
        /// </summary>
        /// <param name="model">FileObject being searched</param>
        /// <returns>bool which indicates if the search pattern was found or not</returns>
        private bool SearchInDataGrid(FileObject model)
        {            
            if (String.IsNullOrEmpty(SearchString))
            {
                return true;
            }

            if (model.Name.ToLowerInvariant().Contains(SearchString.ToLowerInvariant()))
            {
                return true;
            }
            
            return false;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PercentUpdate = e.ProgressPercentage;
        }

        void DisplayBrowserDialogViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("RecurseDirectories"))
            {
                worker.RunWorkerAsync();
                _fileListView.MoveCurrentToFirst();
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
                DoWork();
        }

        /// <summary>
        /// The Background DoWork Task which enumerates all the files under the specified directory
        /// recursively/non-recursively based on the checkbox RecurseDirectories
        /// </summary>
        private void DoWork()
        {
            //Since, I start the background worker in the constructor and the ShowDialog
            //takes time to pop up the window, the Directory.GetFiles throws ArgumentNullException
            //indicating the path value was null. In reality, peeking through debugger all values
            //are present. This clearly indicates a race condition between thread start and dialog popup
            //Ideally the thread must be started after window loads up. I don't want to move the
            //background processor job into the *view* in which case this problem would have been
            //non existant ! I would like the view to be as lightweight as possible and heavyweight
            //tasks are to be done in ViewModel - my 2 cents
            Thread.Sleep(20);
            var fileSet = Directory.GetFiles(this.SelectedPath, "*.*", (RecurseDirectories ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly));

            var results = from file in fileSet
                          let fileinfo = new FileInfo(file)
                          select new FileObject() {                              
                              Name = fileinfo.Name,
                              Type = File.GetAttributes(file).ToString(),
                              Size = fileinfo.Length,
                              DirectoryName = new FileInfo(file).DirectoryName,
                              IsReadOnly = fileinfo.IsReadOnly,
                              Attributes = fileinfo.Attributes,
                              CreationTime = fileinfo.CreationTime,
                              LastAccessTime = fileinfo.LastAccessTime,
                              Extension = fileinfo.Extension
                          };

            _list.Clear();
            Count = results.Count();
            var percentTick = (int) Math.Round(100.0 / Count);
            var percentUpdate = 0;

            results.ToList().ForEach((item) =>
            {
                worker.ReportProgress(percentUpdate);
                _list.Add(item);
                percentUpdate += percentTick;
                Thread.Sleep(50); // Slow down UI update to see the glorious progress bar at work :). Otherwise the progress bar updates too fast !!
            });  
        }


        #region IDisplayBrowserDialog Members

        /// <summary>
        /// Description if any
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        #endregion

        #region IDisplayBrowserDialog Members

        /// <summary>
        /// SelectedPath in the Folderbrowser
        /// </summary>
        public string SelectedPath
        {
            get;
            set;
        }

        #endregion
    }
}
