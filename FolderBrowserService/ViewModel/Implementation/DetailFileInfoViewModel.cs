using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderBrowser.ViewModel.Interface;
using FolderBrowser.Service;
using System.Collections.ObjectModel;

namespace FolderBrowser.ViewModel.Implementation
{
    /// <summary>
    /// View Model for the File detail display modal window.
    /// </summary>
    public class DetailFileInfoViewModel : IDetailFileInfo
    {
        #region IDetailFileInfo Members
        private ObservableCollection<FileDetailObject> _list = new ObservableCollection<FileDetailObject>();

        public ObservableCollection<FileDetailObject> SelectedFileItem
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

        #endregion
    }
}
