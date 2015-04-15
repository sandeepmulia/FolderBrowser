using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace FolderBrowser.ViewModel
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows a Folder Browser Dialog
        /// </summary>
        /// <param name="ownerViewModel"> A ViewModel that represents the owner window of the dialog</param>
        /// <param name="folderDialog">The FolderBrowserDialog to display</param>
        /// <returns>The DialogResult object</returns>
        DialogResult ShowFolderBrowserDialog(object ownerViewModel, IFolderBrowserDialog folderDialog);

        /// <summary>
        /// Shows a dialog.
        /// </summary>
        /// <param name="ownerViewModel">A ViewModel that represents the owner window of the dialog.</param>
        /// <param name="viewModel">The ViewModel of the new dialog.</param>
        /// <returns>A nullable value of type bool that signifies how a window was closed by the user </returns>
        bool? ShowDialog(object ownerViewModel, object viewModel);

    }
}
