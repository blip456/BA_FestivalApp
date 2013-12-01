using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ypsilon2.model;

namespace Ypsilon2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Band inputTest = new Band();
            inputTest.Name = "Billy Talent";
            inputTest.Picture = "no picture";
            inputTest.Descr = "Dead Silence is one of the best albums they made";
            inputTest.Twitter = "no twitter";
            inputTest.Facebook = "no facebook";
            inputTest.ID = "4";
            

            Band.EditBand(inputTest);
            ObservableCollection<Band> lstTest = new ObservableCollection<Band>();
            lstTest = Band.GetBands();
        }
    }
}
