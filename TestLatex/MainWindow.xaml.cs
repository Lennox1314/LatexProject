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

namespace LatexProject
{ /* Problems:
   * Cant drag the window around? Probably because I disabled the window and used a grid instead? need to make grid interactable
   */
    /// <summary>
    /// Currently only contains the click actions for exit, minimize, maxmize of program
    /// </summary>
    public partial class MainWindow : Window
    {
        

        Grid MainGrid = new Grid();
        //Grid MainGrid2 = new Grid();
        public MainWindow()
        {
            InitializeComponent();
            ExitButton.Visibility = Visibility.Visible;
            MaximizeButton.Visibility = Visibility.Visible;
            MinimizeButton.Visibility = Visibility.Visible;
        }

   

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        { // KDN - Closes program on click
            this.Close();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        { // KDN - Maximize program on click or send back to normal size depending on current size
            if(this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else if(this.WindowState == WindowState.Normal) 
            { 
                this.WindowState = WindowState.Maximized;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        { // KDN - Minimized program on click
            this.WindowState = WindowState.Minimized;
        }
    }
      

    }
        
