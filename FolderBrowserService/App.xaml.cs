using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using FolderBrowser.ViewModel;
using FolderBrowser.ViewModel.Implementation;
using FolderBrowser.ViewModel.Interface;

namespace FolderBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceLocator.RegisterSingleton<IDialogService, DialogService>();
            ServiceLocator.Register<IFolderBrowserDialog, FolderBrowserDialogViewModel>();
            ServiceLocator.RegisterSingleton<IDisplayBrowserDialog, DisplayBrowserDialogViewModel>();
            ServiceLocator.RegisterSingleton<IDetailFileInfo, DetailFileInfoViewModel>();


            FolderBrowser.MainWindow view = new FolderBrowser.MainWindow();
            view.DataContext = new MainDialogViewModel();
            view.Show();

        }
    }
}
