using System;
using System.Collections.Generic;
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
using Xceed.Wpf.Toolkit;

namespace Ypsilon2.view
{
    /// <summary>
    /// Interaction logic for LineUp.xaml
    /// </summary>
    public partial class LineUp : UserControl
    {
        public LineUp()
        {
            InitializeComponent();
        }

        private void ButtonSpinner_Spin(object sender, Xceed.Wpf.Toolkit.SpinEventArgs e)
        {
            // code komt van de officiele ExtendedToolkit documentatie
            // https://wpftoolkit.codeplex.com/wikipage?title=ButtonSpinner&referringTitle=Documentation
            ButtonSpinner spinner = (ButtonSpinner)sender;
            TextBox txtValues = (TextBox)spinner.Content;
            int iValue = string.IsNullOrEmpty(txtValues.Text) ? 0 : Convert.ToInt16(txtValues.Text);    // korte IF structuur ? true statement : false statement
            
                
          
        }
    }
}
