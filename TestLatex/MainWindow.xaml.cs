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
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int left, right, top, bottom = 0;

        Grid MainGrid = new Grid();
        //Grid MainGrid2 = new Grid();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCreateGrid_Click(object sender, RoutedEventArgs e)
        {
            int x = Int32.Parse(xCoord.Text);
            int y = Int32.Parse(yCoord.Text);
            int boxName = 0;
            TextBox[] gridBoxes = new TextBox[x + y];

            //  int numberOfBoxes = x * y;
            for (int j = 0; j < y; j++) // creates number of column based on y coordinate
            {

                for (int i = 0; i < x; i++) // creates number of rows bases on x coordinate
                {
                    TextBox dynamicTextBox = new TextBox();
                   // dynamicTextBox.Name = "gridBox" + boxName;
                    dynamicTextBox.Height = 18;
                    dynamicTextBox.Width = 25;
                    dynamicTextBox.Margin = new Thickness(left * 55, top * 55, right * 54, bottom *40);
                    this.MainGrid.Children.Add(dynamicTextBox);
                   // MainGrid.RegisterName("gridBox" + boxName, dynamicTextBox); not working for register?
                    right++;
                    boxName++;

                }
                right = 0;
                bottom++;
            }
            this.canContainer.Children.Add(MainGrid);
            


        }
        private void btnCreateOutput_Click(object sender, RoutedEventArgs e) // trying to extract input from grid of textboxes
        {
            object testObj = MainGrid.FindName("Gridbox0");

            if (testObj != null)
            {
                testOutput.Content = testObj.ToString();
            }
           
        }

    }
}
