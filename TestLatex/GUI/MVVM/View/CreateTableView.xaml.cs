using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Display for when user wants to create a table
    /// Has x and y coordinate for user to supply to create table of x by y textboxes
    /// After user enters all desired values into new textboxes it changes those values into Latex table format
    /// </summary>
    public partial class CreateTableView : UserControl
    {
        public CreateTableView()
        {
            InitializeComponent();
        }
        public int left, right, top, bottom = 0;

        private void btnClearScreen_Click(object sender, RoutedEventArgs e)
        { // KDN - clears the canvas of any current textboxes and any text inside the output box
            canContainer.Children.OfType<TextBox>().ToList().ForEach(tb => canContainer.Children.Remove(tb));

            LaTeXCodeTextBox.Text = "";

        }

        private void btnCreateOutput_Click(object sender, RoutedEventArgs e)
            /* 2/7/23
             * Problems: Can overflow is table was too large causing textbox to not fit on screen
             */
        { // KDN - Uses textbox input to create Latex code inside of textbox for easy copy/paste
            int rows = int.Parse(xCoord.Text);
            int columns = int.Parse(yCoord.Text);

            StringBuilder latexCode = new StringBuilder();
            latexCode.AppendLine("\\begin{tabular}{|");
            for (int j = 0; j < columns; j++)
            {
                latexCode.Append("c|");
            }
            latexCode.AppendLine("}");
            latexCode.AppendLine("\\hline");

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    TextBox textBox = (TextBox)canContainer.FindName("TextBox_" + i + "_" + j);
                    latexCode.Append(textBox.Text + " & ");
                }
                latexCode.Length -= 3;
                latexCode.AppendLine("\\\\ \\hline");
            }

            latexCode.AppendLine("\\end{tabular}");

            LaTeXCodeTextBox.Text = latexCode.ToString();
        }



        private void btnCreateGrid_Click(object sender, RoutedEventArgs e)
        { // KDN - creates a grid of textboxes that after a certain size will become scrollable 
            // get the dimensions of the grid
            int rows = int.Parse(xCoord.Text);
            int columns = int.Parse(yCoord.Text);

            // set the dimensions of each cell
            double textBoxWidth = 40;
            double textBoxHeight = 20;

            // clear any old textboxes
            canContainer.Children.OfType<TextBox>().ToList().ForEach(tb => canContainer.Children.Remove(tb));

            // create a canvas to hold the grid
            Canvas canGrid = new Canvas();
            canGrid.Width = textBoxWidth * columns;
            canGrid.Height = textBoxHeight * rows;


            // create the textboxes and add them to the canvas
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    TextBox textBox = new TextBox();
                    string textBoxName = "TextBox_" + i + "_" + j;
                    if (canGrid.FindName(textBoxName) != null)
                    {
                        canGrid.UnregisterName(textBoxName);
                        canGrid.Children.Remove(canGrid.FindName(textBoxName) as TextBox);
                    }
                    textBox.Width = textBoxWidth;
                    textBox.Height = textBoxHeight;
                    textBox.Name = textBoxName;
                    canGrid.Children.Add(textBox);
                    Canvas.SetLeft(textBox, j * textBoxWidth);
                    Canvas.SetTop(textBox, i * textBoxHeight);
                }
            }

            // create a scroll viewer to hold the canvas
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Width = 300;
            scrollViewer.Height = 160;
            scrollViewer.Content = canGrid;

            // add the scroll viewer to the main container
            canContainer.Children.Clear();
            canContainer.Children.Add(scrollViewer);
        }

        
    }
}
