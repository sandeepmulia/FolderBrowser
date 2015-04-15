using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolderBrowser.ViewModel
{
    /// <summary>
    /// Interface representing the Windows Folder browser
    /// </summary>
    public interface IFolderBrowserDialog
    {
        /// <summary>
        /// The Description of the Window
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Selected Path in the browser
        /// </summary>
        string SelectedPath { get; set; }

        /// <summary>
        /// Show the Create New Folder button ?
        /// </summary>
        bool ShowNewFolderButton { get; set; }
    }
}
