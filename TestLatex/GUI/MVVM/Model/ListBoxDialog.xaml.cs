using LatexProject.GUI.MVVM.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ListBoxDialog.xaml
    /// </summary>
    public partial class ListBoxDialog : Window
    {
        public int result;
        List<FontItem> list;
        TextBox tb;
        public ListBoxDialog(TextBox tb)
        {
            InitializeComponent();
            list = new List<FontItem>();
            list.Add(new FontItem("tiny", 6));
            list.Add(new FontItem("script", 7));
            list.Add(new FontItem("footnote", 8));
            list.Add(new FontItem("small", 10));
            list.Add(new FontItem("normal", 12));
            list.Add(new FontItem("large", 14));
            list.Add(new FontItem("larger", 16));
            list.Add(new FontItem("even larger", 18));
            list.Add(new FontItem("huge", 20));
            list.Add(new FontItem("huger", 22));
            this.tb = tb;
            fontsizes.ItemsSource = list;
        }



        public class FontItem
        {
            public string Title { get; set; }
            public int Size { get; set; }
            public FontItem(string title, int size) { Title = title; Size = size; }
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (fontsizes.SelectedIndex >= 0)
            {
                result = list.ElementAt(fontsizes.SelectedIndex).Size;
            } else
            {
                result = 0;
            }
            updateFontSize();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            result = 0;
            updateFontSize();
            this.Close();
        }

        private void updateFontSize()
        {
            if (result > 0)
            {
                tb.FontSize = result;
            }
        }
    }
}
