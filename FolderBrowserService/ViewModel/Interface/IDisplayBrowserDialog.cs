using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolderBrowser.ViewModel
{
    /// <summary>
    /// Interface representing the Display file Browser dialog box
    /// </summary>
    public interface IDisplayBrowserDialog
    {
        /// <summary>
        /// Description property
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Selected Path in the Folder Browser
        /// </summary>
        string SelectedPath { get; set; }

    }
}
