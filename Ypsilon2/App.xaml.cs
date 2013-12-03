using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ypsilon2.model;

namespace Ypsilon2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static RoutedCommand testBindingCommand = new RoutedCommand();

        private void OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("OnExecuted");
        }

        private void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("OnCanExecute");

            e.CanExecute = true;
        }
    }
}
