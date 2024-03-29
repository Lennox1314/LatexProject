﻿using LatexProject.GUI.MVVM.Model;
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

        // Instance variables
        public int left, right, top, bottom = 0;
        private int rows;
        private int columns;
        private string tableCaption = "";
        public Canvas canGrid;
        public ScrollViewer scrollViewer;
        private ColorWheelWindow myColorWheel;
        public Brush selectedColor;
        public bool paintCell = false;
        public bool changeTextColor = false;
        Dictionary<string, string> colorPairs = new Dictionary<string, string>();
        double textBoxWidth = 40;
        double textBoxHeight = 20;
        int horCells = 0;
        int vertCells = 0;
        int borderThickness = 1;

        private void btnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            // This function determines whether or not the buttons should be displayed.
            if(stkMenuDisplay.Visibility == Visibility.Collapsed)
            {
                stkMenuDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                stkMenuDisplay.Visibility = Visibility.Collapsed;
            }
        }

        private void btnImportCSV_Click(object sender, RoutedEventArgs e) {
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

        private void btnCreateCopyText_Click(object sender, RoutedEventArgs e)
        {
            // SDM - Copies the generated text to the user's clipboard.
            Clipboard.SetText(LaTeXCodeTextBox.Text);
        }

        private void BoldButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Bolds the selected text.
            if (rows > 0 && columns > 0)
            {
                foreach (TextBox tb in canGrid.Children.OfType<TextBox>().ToList())
                {
                    if (tb.IsSelectionActive && tb.FontWeight != FontWeights.Bold)
                    {
                        tb.FontWeight = FontWeights.Bold;
                    }
                    else if (tb.IsSelectionActive && tb.FontWeight == FontWeights.Bold)
                    {
                        tb.FontWeight = FontWeights.Normal;
                    }
                }
            }
        }

        private void ItalisizeButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Italicizes the selected text.
            if (rows > 0 && columns > 0)
            {
                foreach (TextBox tb in canGrid.Children.OfType<TextBox>().ToList())
                {
                    if (tb.IsSelectionActive && tb.FontStyle != FontStyles.Italic)
                    {
                        tb.FontStyle = FontStyles.Italic;
                    }
                    else if (tb.IsSelectionActive && tb.FontStyle == FontStyles.Italic)
                    {
                        tb.FontStyle = FontStyles.Normal;
                    }
                }
            }
        }

        private void RowsButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Prompts the user using an input box to enter the number of rows.
            // Known Error - When clicking cancel on the dialog box it displays the error message.
            string answer = Microsoft.VisualBasic.Interaction.InputBox("How many rows should there be?", "Enter Rows", rows.ToString());
            try
            {
                rows = Int32.Parse(answer);
            } catch
            {
                MessageBox.Show("Invalid input. Please enter an integer.");
            }
            if(rows > 0 && columns > 0)
            {
                CreateGrid();
            }
        }

        private void ColumnsButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Prompts the user using an input box to enter the number of columns.
            // Known Error - When clicking cancel on the dialog box it displays the error message.
            string answer = Microsoft.VisualBasic.Interaction.InputBox("How many columns should there be?", "Enter Rows", columns.ToString());
            try
            {
                columns = Int32.Parse(answer);
            }
            catch
            {
                MessageBox.Show("Invalid input. Please enter an integer.");
            }
            if(rows > 0 && columns > 0)
            {
                CreateGrid();
            }
        }

        private void TableCaptionButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Prompts the user for a table caption.
            tableCaption = Microsoft.VisualBasic.Interaction.InputBox("Enter the caption for the table:", "Table Caption", ""); 
        }

        private void TableFormatClearButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Clears all special formatting from the table.
            if (rows > 0 && columns > 0)
            {
                foreach (TextBox tb in canGrid.Children.OfType<TextBox>().ToList())
                {
                    tb.FontWeight = FontWeights.Normal;
                    tb.FontStyle = FontStyles.Normal;
                    tb.Background = Brushes.White;
                    tb.Foreground = Brushes.Black;
                    tb.TextAlignment = TextAlignment.Left;
                    tb.FontSize= 12;
                    tb.TextDecorations = null;
                    tb.Visibility = Visibility.Visible;
                    tb.Width = textBoxWidth;
                    tb.Height = textBoxHeight;
                }
                colorPairs.Clear();
                borderThickness = 1;
            }
        }

        private void ClearTableButton_Click(object sender, MouseButtonEventArgs e)
        {
            // KDN - clears the canvas of any current textboxes and any text inside the output box
            canContainer.Children.OfType<TextBox>().ToList().ForEach(tb => canContainer.Children.Remove(tb));
            if (canGrid != null)
            {
                canGrid.Children.OfType<TextBox>().ToList().ForEach(tb => tb.Clear());
                canGrid.Children.OfType<TextBox>().ToList().ForEach(tb => canGrid.Children.Remove(tb));
            }
            LaTeXCodeTextBox.Text = "";
            rows = 0;
            columns = 0;
            borderThickness = 1;
            tableCaption = "";
        }

        private string GetHexColor(Brush brush)
        {
            SolidColorBrush solidBrush = brush as SolidColorBrush;
            if (solidBrush != null)
            {
                return solidBrush.Color.ToString().Substring(3, 6);
            }
            return string.Empty;
        }

        private void btnCreateOutput_Click(object sender, RoutedEventArgs e)
            /* 2/7/23
             * Problems: 
             */
        { // KDN - Uses textbox input to create Latex code inside of textbox for easy copy/paste
          // SDM - Added extra functionality to allow for table headers to be used.
            StringBuilder latexCode = new StringBuilder();
            addTextColors(rows, columns);
            addCellColors(rows, columns);

            // Verifies whether the color package needs to be added and if so it adds it to the top of the output.
            if (colorPairs.Count > 0)
            {
                latexCode.Append("%***\tADD THE FOLLOWING TO THE TOP OF YOUR DOCUMENT\t***\n");
                latexCode.Append("\\usepackage[table]{xcolor}\n");
                try
                {
                    List<TextBox> textboxes = canGrid.Children.OfType<TextBox>().ToList();
                    foreach (TextBox textBox in textboxes)
                    {

                        if (textBox == null)
                        {
                            latexCode.Append(" & "); // need to put exception for null here ---------
                        }
                        else
                        {
                            int col = 0;
                            int row = 0;
                            int index = textboxes.IndexOf(textBox);
                            if (index > 0)
                            {
                                col = index % columns;
                                row = index / columns;
                            }

                            string cellColor = GetHexColor(textBox.Background);
                            string textColor = GetHexColor(textBox.Foreground);
                            string colorName = "color" + row + "_" + col;
                            string textColorName = "textcolor" + row + "_" + col;
                            // If the cell is colored, it defines the color at the top of the latex code.
                            if (!string.IsNullOrEmpty(cellColor))
                            {

                                if (colorPairs.ContainsValue(cellColor) && cellColor != "FFFFFF")
                                {
                                    latexCode.Append("\\definecolor{cellColor" + row + "_" + col + "}{HTML}{" + cellColor + "} \n");

                                }
                                if (colorPairs.ContainsValue(textColor) && textColor != "000000")
                                {
                                    latexCode.Append("\\definecolor{textColor" + row + "_" + col + "}{HTML}{" + textColor + "} \n");

                                }


                            }
                        }
                    }
                }
                catch (NullReferenceException except)
                {
                    MessageBox.Show("Ensure both columns and rows are greater than 0.");
                }
                latexCode.Append("%***\tEND\t***\n\n");
            }
            // Adds the border thickness to the latex output.
            latexCode.Append("\\setlength{\\arrayrulewidth}{" + borderThickness + "pt}\n");

            // Starts the table in the latex output.
            latexCode.AppendLine("\\begin{table}[h]");

            // Starts the table centered. - SDM
            latexCode.AppendLine("\\centering");

            latexCode.AppendLine("\\begin{tabular}{|");
            for (int j = 0; j < columns; j++)
            {
                latexCode.Append("l|");
            }
            latexCode.AppendLine("}");
            latexCode.AppendLine("\\hline");

            try {
                List<TextBox> textboxes = canGrid.Children.OfType<TextBox>().ToList();
                foreach (TextBox textBox in textboxes)
                {

                    if (textBox == null)
                    {
                        latexCode.Append(" & "); // need to put exception for null here ---------
                    } else if(textBox.Visibility == Visibility.Hidden)
                    {

                    }
                    else
                    {
                        string cellColor = GetHexColor(textBox.Background);
                        string textColor = GetHexColor(textBox.Foreground);
                        string colorName = colorPairs.FirstOrDefault(x => x.Value == cellColor).Key;
                        string textColorName = colorPairs.FirstOrDefault(x => x.Value == textColor).Key;
                        string fontSizeName = "";
                        int endBraces = 0;
                        int numOfColsWide = (int)(textBox.Width / textBoxWidth);
                        // If the font size is different than the default it adds the latex syntax to handle it.
                        if (textBox.FontSize == 6)
                        {
                            fontSizeName += "\\tiny";
                        }
                        else if (textBox.FontSize == 7)
                        {
                            fontSizeName += "\\scriptsize";
                        }
                        else if (textBox.FontSize == 8)
                        {
                            fontSizeName += "\\footnotesize";
                        }
                        else if (textBox.FontSize == 10)
                        {
                            fontSizeName += "\\small";
                        }
                        else if (textBox.FontSize == 14)
                        {
                            fontSizeName += "\\large";
                        }
                        else if (textBox.FontSize == 16)
                        {
                            fontSizeName += "\\Large";
                        }
                        else if (textBox.FontSize == 18)
                        {
                            fontSizeName += "\\LARGE";
                        }
                        else if (textBox.FontSize == 20)
                        {
                            fontSizeName += "\\huge";
                        }
                        else if (textBox.FontSize == 22)
                        {
                            fontSizeName += "\\Huge";
                        }
                        // Adds the multicolumn line of code if needed.
                        if (textBox.TextAlignment == TextAlignment.Center || textBox.TextAlignment == TextAlignment.Right || textBox.Width > textBoxWidth)
                        {
                            latexCode.Append("\\multicolumn{" + numOfColsWide + "}{|l|}");
                        }
                        // If the cell's text alignment is different than the default, it adds the latex code to handle that.
                        if (textBox.TextAlignment == TextAlignment.Center)
                        {
                            latexCode.Length -= 5;
                            latexCode.Append("{|c|}");
                        }
                        else if (textBox.TextAlignment == TextAlignment.Right)
                        {
                            latexCode.Length -= 5;
                            latexCode.Append("{|r|}");
                        }
                        latexCode.Append("{");
                        if (cellColor != null && cellColor != "FFFFFF")
                        {
                            latexCode.Append("\\cellcolor{" + colorName + "} ");
                        }
                        if (textColor != null && textColor != "000000")
                        {
                            latexCode.Append("\\color{" + textColorName + "} ");
                        }
                        if (fontSizeName != "")
                        {
                            latexCode.Append(fontSizeName + " ");
                        }
                        // The next 3 if statements handle underlining, bolding, and italicising text.
                        if (textBox.TextDecorations == TextDecorations.Underline)
                        {
                            latexCode.Append("\\underline{");
                            endBraces++;
                        }
                        if (textBox.FontWeight == FontWeights.Bold)
                        {
                            latexCode.Append("\\textbf{");
                            endBraces++;
                        }
                        if (textBox.FontStyle == FontStyles.Italic)
                        {
                            latexCode.Append("\\textit{");
                            endBraces++;
                        }

                        latexCode.Append(textBox.Text + "}");
                        for (int braces = 0; braces < endBraces; braces++)
                        {
                            latexCode.Append("}");
                        }

                        latexCode.Append(" & ");
                    }
                    int index = textboxes.IndexOf(textBox);
                    if ((index + 1) % columns == 0)
                    {
                        latexCode.Length -= 3;
                        latexCode.AppendLine("\\\\ \\hline");
                    }
                }
            }
            catch (NullReferenceException except)
            {
                MessageBox.Show("Ensure both columns and rows are greater than 0.");
            }

            latexCode.AppendLine("\\end{tabular}");
            if (tableCaption != "")
            {
                latexCode.AppendLine("\\caption{" + tableCaption + "}");
            }

            latexCode.AppendLine("\\end{table}");
            LaTeXCodeTextBox.Text = latexCode.ToString();
        }

        private void CellColor_Click(object sender, RoutedEventArgs e)
        { // kdn -  opens color wheel and sets paintcell to true/changetextcolor to false so that clicking on cells will change their color but not the text
            changeTextColor = false;
            paintCell = true;
            myColorWheel = new ColorWheelWindow(this);
            // Show the color wheel window as a dialog
            myColorWheel.ShowDialog();

        }
        private void PaintTextbox_Click(object sender, MouseButtonEventArgs e)
        { // kdn - event that takes currently selected color and changes the textboxes color to that color when clicked

            if (myColorWheel != null)
            {
                Brush brushcolor = myColorWheel.mybrush;
                if (brushcolor != null && paintCell == true)
                {
                    TextBox textbox = sender as TextBox;

                    if (textbox != null)
                    {
                        textbox.Background = brushcolor;
                    }
                }
            }
               
        }
        private void TextColor_Click(object sender, RoutedEventArgs e)
        { // kdn - opens color wheel and makes sure paintcell is false/changetextcolor is true so the cells dont change color but the text does
            changeTextColor = true;
            paintCell = false;
            myColorWheel = new ColorWheelWindow(this);
            // Show the color wheel window as a dialog
            myColorWheel.ShowDialog();

        }

        private void ChangeTextColor_Click(object sender, TextChangedEventArgs e)
        {
            if (myColorWheel != null)
            {
                Brush brushcolor = myColorWheel.mybrush;
                if (brushcolor != null && changeTextColor == true)
                {
                    TextBox textbox = sender as TextBox;

                    if (textbox != null)
                    {
                        textbox.Foreground = brushcolor;
                    }
                }
            }

        }

        private void LeftAlignButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Aligns the text in the textbox to the left
            if (rows > 0 && columns > 0)
            {
                foreach (TextBox tb in canGrid.Children.OfType<TextBox>().ToList())
                {
                    if (tb.IsSelectionActive)
                    {
                        tb.TextAlignment = TextAlignment.Left;
                    }
                }
            }
        }

        private void CenterAlignTextButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Aligned the text in the textbox to the center
            if (rows > 0 && columns > 0)
            {
                foreach (TextBox tb in canGrid.Children.OfType<TextBox>().ToList())
                {
                    if (tb.IsSelectionActive)
                    {
                        tb.TextAlignment = TextAlignment.Center;
                    }
                }
            }
        }

        private void RightAlignButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Aligned the text in the textbox to the right
            if (rows > 0 && columns > 0)
            {
                foreach (TextBox tb in canGrid.Children.OfType<TextBox>().ToList())
                {
                    if (tb.IsSelectionActive)
                    {
                        tb.TextAlignment = TextAlignment.Right;
                    }
                }
            }
        }

        private void FontSizeButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Changes the font size of the textbox selected.
            if(rows > 0 && columns > 0)
            {
                foreach(TextBox tb in canGrid.Children.OfType<TextBox>().ToList())
                {
                    if(tb.IsSelectionActive)
                    {
                        ListBoxDialog listBox = new ListBoxDialog(tb);
                        listBox.Show();
                    }
                }
            }
        }

        private void UnderlineButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Underlines the font of the textbox selected.
            if(rows > 0 && columns > 0)
            {
                foreach(TextBox tb in canGrid.Children.OfType<TextBox>().ToList())
                {
                    if (tb.IsSelectionActive && tb.TextDecorations != TextDecorations.Underline)
                    {
                        tb.TextDecorations = TextDecorations.Underline;
                    } else if(tb.IsSelectionActive)
                    {
                        tb.TextDecorations = null;
                    }
                }
            }
        }

        private void MergeCellButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Handles the merging of textboxes.
            if(rows > 0 && columns > 0)
            {
                List<TextBox> textboxes = canGrid.Children.OfType<TextBox>().ToList();
                foreach(TextBox tb in textboxes)
                {
                    if(tb.IsSelectionActive)
                    {
                        int numOfHorCells = (int)(tb.Width / textBoxWidth);
                        int numOfVertCells = (int)(tb.Height / textBoxHeight);
                        int col = 0;
                        int row = 0;
                        int index = textboxes.IndexOf(tb);
                        
                        if(index > 0)
                        {
                            col = (index) % columns;
                            row = (index) / columns;
                        }

                        bool mRight;
                        bool mDown;
                        MergeCellDialog dial = new MergeCellDialog(row + (numOfVertCells - 1), col + (numOfHorCells - 1), rows, columns);
                        dial.ShowDialog();
                        mRight = dial.doMergeRight();
                        mDown = dial.doMergeDown();

                        if(mRight)
                        {
                            int cellToMergeWidth = 0;
                            for (int i = 1; i <= numOfVertCells; i++)
                            {
                                TextBox cellToMerge = textboxes[(index + numOfHorCells) + (i - 1) * columns];

                                cellToMerge.Visibility = Visibility.Hidden;
                                cellToMerge.Clear();
                                cellToMergeWidth = (int)cellToMerge.Width;
                            }
                            tb.Width += cellToMergeWidth;

                        } else if(mDown)
                        {
                            int cellToMergeHeight = 0;
                            for (int i = 1; i <= numOfHorCells; i++)
                            {
                                TextBox cellToMerge = textboxes[(index + (columns * numOfVertCells) + (i - 1))];

                                cellToMerge.Visibility = Visibility.Hidden;
                                cellToMerge.Clear();
                                cellToMergeHeight = (int)cellToMerge.Height;
                            }
                            tb.Height += cellToMergeHeight;
                        }

                    }
                }
            }
        }

        private void SplitCellButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Handles the splitting of textboxes.
            if(rows > 0 && columns > 0)
            {
                List<TextBox> textboxes = canGrid.Children.OfType<TextBox>().ToList();
                foreach(TextBox tb in textboxes)
                {
                    if(tb.IsSelectionActive)
                    {
                        if(tb.Width == textBoxWidth && tb.Height == textBoxHeight)
                        {
                            MessageBox.Show("Cannot split the cell as it's only a single cell.");
                        } else
                        {
                            int numHorSplit = (int)(tb.Width / textBoxWidth);
                            int numVertSplit = (int)(tb.Height / textBoxHeight);

                            int col = 0;
                            int row = 0;
                            int index = textboxes.IndexOf(tb);
                            if(index > 0)
                            {
                                col = index % columns;
                                row = index / columns;
                            }

                            tb.Width = textBoxWidth;
                            tb.Height = textBoxHeight;
                        }
                    }
                    if(tb.Visibility == Visibility.Hidden)
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void BordersButton_Click(object sender, MouseButtonEventArgs e)
        {
            // SDM - Changes the borders of the table.
            if(rows > 0 && columns > 0)
            {
                BorderDialog dialog= new BorderDialog();
                dialog.ShowDialog();
                borderThickness = dialog.getBorderThickness();
                foreach(TextBox tb in canGrid.Children.OfType<TextBox>().ToList())
                {
                    tb.BorderThickness = new Thickness(borderThickness);
                }
            }
        }

        public void addTextColors(int rows, int columns)
        {
            List<TextBox> textboxes = canGrid.Children.OfType<TextBox>().ToList();
            foreach (TextBox textBox in textboxes)
            {
                int col = 0;
                int row = 0;
                int index = textboxes.IndexOf(textBox);
                if (index > 0)
                {
                    col = index % columns;
                    row = index / columns;
                }

                string textColor = GetHexColor(textBox.Foreground);
                string textColorName = "textColor" + row + "_" + col;
                if (!colorPairs.ContainsKey(textColor) && !colorPairs.ContainsKey(textColorName) && textColor != "000000")
                {
                    colorPairs.Add(textColorName, textColor);
                }

            }
        }
        public void addCellColors(int rows, int columns)
        {
            List<TextBox> textboxes = canGrid.Children.OfType<TextBox>().ToList();
            foreach (TextBox textBox in textboxes)
            {
                int col = 0;
                int row = 0;
                int index = textboxes.IndexOf(textBox);
                if(index > 0)
                {
                    col = index % columns;
                    row = index / columns;
                }
                string cellColor = GetHexColor(textBox.Background);
                string cellColorName = "cellColor" + row + "_" + col;
                if (!colorPairs.ContainsKey(cellColor) && !colorPairs.ContainsKey(cellColorName) && cellColor != "FFFFFF")
                {
                    colorPairs.Add(cellColorName, cellColor);
                }

            }
        }

        private void CreateGrid()
        {
            // KDN - creates a grid of textboxes that after a certain size will become scrollable 

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
                    textBox.PreviewMouseLeftButtonUp += PaintTextbox_Click;
                    textBox.TextChanged += ChangeTextColor_Click;
                    textBox.TextWrapping = TextWrapping.Wrap;
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
