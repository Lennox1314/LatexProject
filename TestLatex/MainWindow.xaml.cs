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



        private void btnCreateOutput_Click(object sender, RoutedEventArgs e)
        {
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
        {
            int rows = int.Parse(xCoord.Text);
            int columns = int.Parse(yCoord.Text);
            double textBoxWidth = 50;
            double textBoxHeight = 25;

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
        
