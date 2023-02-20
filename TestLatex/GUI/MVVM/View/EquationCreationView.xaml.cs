using LatexProject.GUI.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace LatexProject.GUI.MVVM.View
{
    /// <summary>
    /// Interaction logic for EquationCreationView.xaml
    /// </summary>
    public partial class EquationCreationView : UserControl
    {
        public EquationCreationView()
        {
            InitializeComponent();
            // DataContext = new EquationCreationViewModel();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                string latexCode = menuItem.Tag as string;
                if (latexCode != null)
                {
                    TextBox.Text += latexCode;
                }
            }
        }


    }
}