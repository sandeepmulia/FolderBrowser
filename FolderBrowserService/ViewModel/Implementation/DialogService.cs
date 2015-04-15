using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace FolderBrowser.ViewModel.Implementation
{
    /// <summary>
    /// The DialogService class contains implementation of the ShowFolder browser and for the other
    /// dialogs. This class enforces MVVM pattern by always invoking ShowDialog using through viewmodels
    /// </summary>
    public sealed class DialogService : IDialogService
    {
        #region IDialogService Members
        /// <summary>
        /// The ShowFolderDialog encapsulating method
        /// </summary>
        /// <param name="ownerViewModel">owner viewmodel</param>
        /// <param name="folderDialog">child viewmodel</param>
        /// <returns></returns>
        public System.Windows.Forms.DialogResult ShowFolderBrowserDialog(object ownerViewModel, IFolderBrowserDialog folderDialog)
        {

            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = String.IsNullOrEmpty(folderDialog.SelectedPath) ? "" : folderDialog.SelectedPath;
            DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                folderDialog.SelectedPath = dialog.SelectedPath;            
            }

            return result;
        }

        /// <summary>
        /// Show Dialog
        /// </summary>
        /// <param name="ownerViewModel">Parent view model</param>
        /// <param name="viewModel">child viewmodel</param>
        /// <returns></returns>
        public bool? ShowDialog(object ownerViewModel, object viewModel)
        {
            Type type = ViewModelViewMapper.GetViewModelDialogMapping(viewModel.GetType());
            return ShowDialog(ownerViewModel, viewModel, type);
        }

        private bool? ShowDialog(object ownerViewModel, object viewModel, Type dialogType)
        {
            Window dialog = (Window)Activator.CreateInstance(dialogType);
            dialog.DataContext = viewModel;

            return dialog.ShowDialog();
        }

        #endregion
    }
}
