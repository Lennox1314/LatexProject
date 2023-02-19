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
        /* 2/7/23
         * Problems: need to make it clear old textboxes if they exist before creation or gets duplication error
         * If desired table is too big it will flow off the page
         * */
        { //KDN - Creates textbox grid for user input based on x and y values
            int rows = int.Parse(xCoord.Text);
            int columns = int.Parse(yCoord.Text);
            double textBoxWidth = 40;
            double textBoxHeight = 20;

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Width = textBoxWidth;
                    textBox.Height = textBoxHeight;
                    textBox.Name = "TextBox_" + i + "_" + j;
                    RegisterName(textBox.Name, textBox);
                    Canvas.SetLeft(textBox, j * textBoxWidth);
                    Canvas.SetTop(textBox, i * textBoxHeight);
                    canContainer.Children.Add(textBox);
                }
            }
        }
    }
}
