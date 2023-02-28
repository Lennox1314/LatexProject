using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

        private void chkbox_tableHeader_Checked(object sender, RoutedEventArgs e)
        {
            txt_tableHeader.Visibility = Visibility.Visible;
            lbl_tableHeaderContent.Visibility = Visibility.Visible;
        }

        private void chkbox_tableHeader_Unhecked(object sender, RoutedEventArgs e)
        {
            txt_tableHeader.Visibility = Visibility.Collapsed;
            lbl_tableHeaderContent.Visibility = Visibility.Collapsed;
        }

        private void chkbox_tableCaption_Checked(object sender, RoutedEventArgs e)
        {
            txt_tableCaption.Visibility = Visibility.Visible;
            lbl_tableCaption.Visibility = Visibility.Visible;
        }

        private void chkbox_tableCaption_Unchecked(object sender, RoutedEventArgs e)
        {
            txt_tableCaption.Visibility = Visibility.Collapsed;
            lbl_tableCaption.Visibility = Visibility.Collapsed;
        }

        private void btnImportCSV_Click(object sender, RoutedEventArgs e) {
            /*string[,] testvalues = new string[5, 5];

            int test = 0;
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 5; j++) {
                    testvalues[i, j] = test.ToString();
                    test += 1;
                }
            }*/

            string filePath = "";

            // Opens a file dialog for the user to select a csv file to import
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true) {
                filePath = fileDialog.FileName;
            }

            var reader = new StreamReader(File.OpenRead(filePath));
            List<List<string>> lines = new List<List<string>>();
            while (!reader.EndOfStream) {
                List<string> valuesList = new List<string>();
                var line = reader.ReadLine();
                var values = line.Split(',');
                foreach(string value in values){
                    valuesList.Add(value);
                }
                lines.Add(valuesList);
            }

            int rows = lines.Count;
            int columns = lines.First().Count;
            xCoord.Text = rows.ToString();
            yCoord.Text = columns.ToString();
            double textBoxWidth = 40;
            double textBoxHeight = 20;

            canContainer.Children.OfType<TextBox>().ToList().ForEach(tb => canContainer.Children.Remove(tb)); // clears any old textboxes if any

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {

                    TextBox textBox = new TextBox();
                    string textBoxName = "TextBox_" + i + "_" + j;
                    if (canContainer.FindName(textBoxName) != null) {
                        canContainer.UnregisterName(textBoxName);
                        canContainer.Children.Remove(canContainer.FindName(textBoxName) as TextBox);
                    }
                    textBox.Width = textBoxWidth;
                    textBox.Height = textBoxHeight;
                    textBox.Name = textBoxName;
                    textBox.Text = lines[i][j];
                    RegisterName(textBox.Name, textBox);
                    Canvas.SetLeft(textBox, j * textBoxWidth);
                    Canvas.SetTop(textBox, i * textBoxHeight);
                    canContainer.Children.Add(textBox);
                }
            }
        }

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
          // SDM - Added extra functionality to allow for table headers to be used.
            int rows = int.Parse(xCoord.Text);
            int columns = int.Parse(yCoord.Text);
            bool tableHeader = false;
            bool tableCaption = false;

            // Checks if tableheader checkbox is ticked and if so updates the bool value. - SDM
            if(chkbox_tableHeader.IsChecked == true)
            {
                tableHeader = true;
            }

            // Checks if tableCaption checkbox is ticked and if so updates the bool value. - SDM
            if(chkbox_tableCaption.IsChecked == true)
            {
                tableCaption = true;
            }

            StringBuilder latexCode = new StringBuilder();
            latexCode.AppendLine("\\begin{table}[h]");

            // Starts the table centered. - SDM
            latexCode.AppendLine("\\centering");

            latexCode.AppendLine("\\begin{tabular}{|");
            for (int j = 0; j < columns; j++)
            {
                latexCode.Append("c|");
            }
            latexCode.AppendLine("}");
            latexCode.AppendLine("\\hline");

            // Adds the header row. - SDM
            if (tableHeader)
            {
                latexCode.AppendLine("\\multicolumn{" + columns.ToString() + "}{|c|}{" + txt_tableHeader.Text + "} \\\\ \n \\hline");
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    TextBox textBox = (TextBox)canContainer.FindName("TextBox_" + i + "_" + j);
                    latexCode.Append(textBox.Text + " & ");
                }
                latexCode.Length -= 3;
                latexCode.AppendLine("\\\\ \\hline");
            }

            latexCode.AppendLine("\\end{tabular}");

            if(tableCaption)
            {
                latexCode.AppendLine("\\caption{" + txt_tableCaption.Text + "}");
            }

            latexCode.AppendLine("\\end{table}");

            LaTeXCodeTextBox.Text = latexCode.ToString();
        }



        private void btnCreateGrid_Click(object sender, RoutedEventArgs e)
        /* 2/7/23
         * Problems:
         * If desired table is too big it will flow off the page
         * */
        { //KDN - Creates textbox grid for user input based on x and y values
            int rows = int.Parse(xCoord.Text);
            int columns = int.Parse(yCoord.Text);
            double textBoxWidth = 40;
            double textBoxHeight = 20;

            canContainer.Children.OfType<TextBox>().ToList().ForEach(tb => canContainer.Children.Remove(tb)); // clears any old textboxes if any

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    
                    TextBox textBox = new TextBox();
                    string textBoxName = "TextBox_" + i + "_" + j;
                    if (canContainer.FindName(textBoxName) != null)
                    {
                        canContainer.UnregisterName(textBoxName);
                        canContainer.Children.Remove(canContainer.FindName(textBoxName) as TextBox);
                    }
                    textBox.Width = textBoxWidth;
                    textBox.Height = textBoxHeight;
                    textBox.Name = textBoxName;
                    RegisterName(textBox.Name, textBox);
                    Canvas.SetLeft(textBox, j * textBoxWidth);
                    Canvas.SetTop(textBox, i * textBoxHeight);
                    canContainer.Children.Add(textBox);
                }
            }
        }
    }
}
