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
    /// Interaction logic for MergeCellDialog.xaml
    /// </summary>
    public partial class MergeCellDialog : Window
    {
        int row, col, numRows, numCols;
        bool mRight, mDown;
        public MergeCellDialog(int row, int col, int numRows, int numCols)
        {
            InitializeComponent();
            this.row = row;
            this.col = col;
            this.numRows = numRows;
            this.numCols = numCols;
            this.mRight = false;
            this.mDown = false;
        }

        public void btnMergeRight_Click(object sender, EventArgs e)
        {
            if(col == numCols - 1)
            {
                MessageBox.Show("Cannot merge current cell to the right as there is no cell to the right to merge.");
            } else
            {
                mRight = true;
            }
            this.Close();
        }

        public void btnMergeDown_Click(object sender, EventArgs e)
        {
            if(row == numRows - 1)
            {
                MessageBox.Show("Cannot merge current cell downwards as there is no cell beneath to merge.");
            } else
            {
                mDown = true;
            }
            this.Close();
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool doMergeRight()
        {
            return mRight;
        }

        public bool doMergeDown()
        {
            return mDown;
        }
    }
}
