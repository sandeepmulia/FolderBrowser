using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FolderBrowser.Service;
using FolderBrowser.ViewModel;

namespace FolderBrowser
{
    /// <summary>
    /// Interaction logic for DisplayBrowser.xaml
    /// </summary>
    public partial class DisplayBrowser : Window
    {             
        public DisplayBrowser()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
