using System;
using System.Windows.Input;
using Atlassian.ViewModel;
using Atlassian.ViewModel.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    /// <summary>
    /// [NOTE] : PLEASE NOTE NOT ALL UNIT TESTS ARE INCLUDED IN THIS PROJECT
    /// </summary>
    [TestClass]
    public class ViewModelUnitTests
    {
        [TestMethod]
        public void TestMainWindow()
        {
            ServiceLocator.RegisterSingleton<IDialogService, DialogService>();
            ServiceLocator.Register<IFolderBrowserDialog, FolderBrowserDialogViewModel>();
            ServiceLocator.RegisterSingleton<IDisplayBrowserDialog, DisplayBrowserDialogViewModel>();

            var mainDialogVm = new MainDialogViewModel();
            mainDialogVm.SelectedDirectory = "Blah";
            ICommand res = mainDialogVm.NextCommand;
            Assert.AreEqual(false,res.CanExecute(null));

            mainDialogVm.SelectedDirectory = @"C:\Windows";
            ICommand res1 = mainDialogVm.NextCommand;
            Assert.AreEqual(true,res1.CanExecute(null));
        }

        [TestMethod]
        public void TestDisplayBrowser()
        {           
            var mainDialogVm = new MainDialogViewModel();
            mainDialogVm.SelectedDirectory = @"C:\Windows";
            ICommand res1 = mainDialogVm.NextCommand;
            Assert.AreEqual(true, res1.CanExecute(null));
            res1.Execute(null);
            ICommand cancel = mainDialogVm.CancelCommand;
            cancel.Execute(null);            
        }

        [TestMethod]
        public void TestFolderBrowser()
        {
            var mainDialogVm = new MainDialogViewModel();
            mainDialogVm.SelectedDirectory = @"C:\Windows";

            ICommand dirBrowser = mainDialogVm.ShowDirectoryBrowserCommand;
            dirBrowser.Execute(null);
        }

        [TestMethod]
        public void TestDetailWindow()
        {
            ServiceLocator.RegisterSingleton<IDialogService, DialogService>();
            ServiceLocator.Register<IFolderBrowserDialog, FolderBrowserDialogViewModel>();
            ServiceLocator.RegisterSingleton<IDisplayBrowserDialog, DisplayBrowserDialogViewModel>();

            var mainDialogVm = new MainDialogViewModel();
            mainDialogVm.SelectedDirectory = @"C:\Windows";

            IDisplayBrowserDialog browserDialog = new DisplayBrowserDialogViewModel();
            browserDialog.SelectedPath = @"C:\Windows";
            bool? ret = ServiceLocator.Resolve<IDialogService>().ShowDialog(mainDialogVm, browserDialog);
            Assert.AreEqual(false, ret.Value);
        }
    }
}
