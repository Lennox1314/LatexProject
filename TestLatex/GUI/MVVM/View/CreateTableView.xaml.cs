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
        private int rows;
        private int columns;
        public Canvas canGrid;
        public ScrollViewer scrollViewer;


        private void btnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            if(stkMenuDisplay.Visibility == Visibility.Collapsed)
            {
                stkMenuDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                stkMenuDisplay.Visibility = Visibility.Collapsed;
            }
        }
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
            try
            {
                var reader = new StreamReader(File.OpenRead(filePath));
                List<List<string>> lines = new List<List<string>>();
                while (!reader.EndOfStream)
                {
                    List<string> valuesList = new List<string>();
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    foreach (string value in values)
                    {
                        valuesList.Add(value);
                    }
                    lines.Add(valuesList);
                }


                rows = lines.Count;
                columns = lines.First().Count;
                double textBoxWidth = 40;
                double textBoxHeight = 20;


                // create a canvas to hold the grid
                canGrid = new Canvas();
                canGrid.Width = textBoxWidth * columns;
                canGrid.Height = textBoxHeight * rows;

                NameScope.SetNameScope(canGrid, new NameScope());

                // clear any old textboxes
                canContainer.Children.OfType<TextBox>().ToList().ForEach(tb => canContainer.Children.Remove(tb));

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
                        textBox.Text = lines[i][j];
                        canGrid.Children.Add(textBox);
                        Canvas.SetLeft(textBox, j * textBoxWidth);
                        Canvas.SetTop(textBox, i * textBoxHeight);

                        canGrid.RegisterName(textBoxName, textBox);
                    }
                }

                // create a scroll viewer to hold the canvas
                scrollViewer = new ScrollViewer();
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollViewer.Width = 300;
                scrollViewer.Height = 160;
                scrollViewer.Content = canGrid;

                // add the scroll viewer to the main container
                canContainer.Children.Clear();
                canContainer.Children.Add(scrollViewer);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error ocurred. Please try again.");
            }
        }

        private void btnClearScreen_Click(object sender, RoutedEventArgs e)
        { // KDN - clears the canvas of any current textboxes and any text inside the output box
            canContainer.Children.OfType<TextBox>().ToList().ForEach(tb => canContainer.Children.Remove(tb));
            canGrid.Children.OfType<TextBox>().ToList().ForEach(tb => canGrid.Children.Remove(tb));
            LaTeXCodeTextBox.Text = "";

        }

        private void btnCreateOutput_Click(object sender, RoutedEventArgs e)
            /* 2/7/23
             * Problems: 
             */
        { // KDN - Uses textbox input to create Latex code inside of textbox for easy copy/paste
          // SDM - Added extra functionality to allow for table headers to be used.
            bool tableHeader = false;
            bool tableCaption = false;

            // Checks if tableheader checkbox is ticked and if so updates the bool value. - SDM
           // if(chkbox_tableHeader.IsChecked == true)
           // {
            //    tableHeader = true;
           // }

            // Checks if tableCaption checkbox is ticked and if so updates the bool value. - SDM
           // if(chkbox_tableCaption.IsChecked == true)
           // {
           //     tableCaption = true;
           // }

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
                    TextBox textBox = (TextBox)canGrid.FindName("TextBox_" + i + "_" + j);
                    if (textBox == null)
                    {
                        latexCode.Append(" & "); // need to put exception for null here ---------
                    }
                    else
                    {
                        latexCode.Append(textBox.Text + " & ");
                    }
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
        { // KDN - creates a grid of textboxes that after a certain size will become scrollable 
            // get the dimensions of the grid
            if (xCoord.Text != "" && yCoord.Text != "")
            {
                rows = int.Parse(xCoord.Text);
                columns = int.Parse(yCoord.Text);
            }

            // set the dimensions of each cell
            double textBoxWidth = 40;
            double textBoxHeight = 20;

            // create a canvas to hold the grid
            canGrid = new Canvas();
            canGrid.Width = textBoxWidth * columns;
            canGrid.Height = textBoxHeight * rows;

            NameScope.SetNameScope(canGrid, new NameScope());

            // clear any old textboxes
            canContainer.Children.OfType<TextBox>().ToList().ForEach(tb => canContainer.Children.Remove(tb));

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

                    canGrid.RegisterName(textBoxName, textBox);
                }
            }

            // create a scroll viewer to hold the canvas
            scrollViewer = new ScrollViewer();
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
