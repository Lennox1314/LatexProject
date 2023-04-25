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
using System.Windows.Shapes;

namespace LatexProject.GUI.MVVM.Model
{
    /// <summary>
    /// Interaction logic for BorderDialog.xaml
    /// </summary>
    public partial class BorderDialog : Window
    {
        int borderThickness = 1;
        public BorderDialog()
        {
            InitializeComponent();
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void btnNoBorders_Click(object sender, EventArgs e)
        {
            borderThickness = 0;
            this.Close();
        }

        public void btnThinBorders_Click(object sender, EventArgs e)
        {
            borderThickness = 1;
            this.Close();
        }

        public void btnThickBorders_Click(object sender, EventArgs e)
        {
            borderThickness = 2;
            this.Close();
        }

        public int getBorderThickness()
        {
            return borderThickness;
        }
    }
}
