using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FolderBrowser.ViewModel.Implementation
{
    /// <summary>
    /// Class which is a wrapper for the FolderBrowser Dialog box
    /// </summary>
    public class FolderBrowserDialogWrapper
    {
        private readonly IFolderBrowserDialog folderBrowserDialogInt;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserImpl;

        public FolderBrowserDialogWrapper(IFolderBrowserDialog folderBrowserDialog_)
        {
            this.folderBrowserDialogInt = folderBrowserDialog_;
            folderBrowserImpl = new System.Windows.Forms.FolderBrowserDialog()
            {
                Description = folderBrowserDialog_.Description,
                SelectedPath = folderBrowserDialog_.SelectedPath,
                ShowNewFolderButton = folderBrowserDialog_.ShowNewFolderButton,
            };
        }

    }
}
