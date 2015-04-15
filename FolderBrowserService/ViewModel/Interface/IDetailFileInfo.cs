using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderBrowser.Service;

namespace FolderBrowser.ViewModel.Interface
{
    /// <summary>
    /// Interface containing a representation fo the Selected File Item in the DataGrid.
    /// </summary>
    public interface IDetailFileInfo
    {
        ObservableCollection<FileDetailObject> SelectedFileItem { get; set; }
    }
}
