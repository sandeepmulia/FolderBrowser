using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace FolderBrowser.ViewModel
{
    /// <summary>
    /// The Base Class for ViewModels
    /// Implements IDisposable, INotifyPropertyChanged and the DependencyObject
    /// </summary>
    public abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged, IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {            
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
