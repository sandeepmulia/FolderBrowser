using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolderBrowser.ViewModel
{
    /// <summary>
    /// ViewModel for the FolderBrowser Dialog (OS component)
    /// </summary>
    public class FolderBrowserDialogViewModel : IFolderBrowserDialog
    {
        #region IFolderBrowserDialog Members

        public string Description
        {
            get;
            set;
        }

        public string SelectedPath
        {
            get;
            set;
        }

        public bool ShowNewFolderButton
        {
            get;
            set;
        }
        #endregion
    }
}
