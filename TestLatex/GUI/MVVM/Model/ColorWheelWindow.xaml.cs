using LatexProject.GUI.MVVM.View;
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
    /// Interaction logic for ColorWheelWindow.xaml
    /// </summary>
    public partial class ColorWheelWindow : Window
    {
        public event EventHandler<ColorSelectedEventArgs> ColorSelected;
        public Brush mybrush;
        private CreateTableView owner;
        public ColorWheelWindow(CreateTableView owner)
        {
            InitializeComponent();
            this.owner = owner;
        }

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
    "SelectedColor", typeof(Color), typeof(ColorWheelWindow), new PropertyMetadata(default(Color), OnSelectedColorChanged));

        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorWheelWindow colorWheelWindow = d as ColorWheelWindow;
            colorWheelWindow.PreviewTextBlock.Background = new SolidColorBrush((Color)e.NewValue);
        }

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            mybrush = PreviewTextBlock.Background;

            ((CreateTableView)owner).selectedColor = mybrush;


            // Close the color wheel window
            this.Close();
        }
        private double GetHueFromThumbPosition()
        {
            Point pos = Thumb.TranslatePoint(new Point(0, 0), this);
            double angle = Math.Atan2(pos.Y - 90, pos.X - 150) * 180 / Math.PI;
            if (angle < 0) angle += 360;
            return angle / 360;
        }

        private Color ColorFromHsv(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, (byte)v, (byte)t, (byte)p);
            else if (hi == 1)
                return Color.FromArgb(255, (byte)q, (byte)v, (byte)p);
            else if (hi == 2)
                return Color.FromArgb(255, (byte)p, (byte)v, (byte)t);
            else if (hi == 3)
                return Color.FromArgb(255, (byte)p, (byte)q, (byte)v);
            else if (hi == 4)
                return Color.FromArgb(255, (byte)t, (byte)p, (byte)v);
            else
                return Color.FromArgb(255, (byte)v, (byte)p, (byte)q);
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ellipse = (Ellipse)sender;
            var position = e.GetPosition(ellipse);

            // Calculate the hue and saturation values based on the mouse position
            var hue = position.X / ellipse.ActualWidth * 360.0;
            var saturation = position.Y / ellipse.ActualHeight;



            // Create a new color with the calculated hue, saturation, and maximum brightness values
            var selectedColor = ColorFromHsv(hue, 1, 1.0);

            SolidColorBrush myBrush = new SolidColorBrush(selectedColor);
            PreviewTextBlock.Background = myBrush;
            // Update the SelectedColor property of the view model
            // var viewModel = (ColorWheelWindow)DataContext;
            // viewModel.SelectedColor = new SolidColorBrush(selectedColor).Color;
        }
        private void Ellipse_MouseLeftButtonDownBlackandWhite(object sender, MouseButtonEventArgs e)
        {
            var ellipse = (Ellipse)sender;
            var position = e.GetPosition(ellipse);

            // Calculate the hue and saturation values based on the mouse position
            var value = position.X / ellipse.ActualWidth;
            var saturation = position.Y / ellipse.ActualHeight;


            


            // Create a new color with the calculated hue, saturation, and maximum brightness values
            var selectedColor = ColorFromHsv(0, 0, value);

            SolidColorBrush myBrush = new SolidColorBrush(selectedColor);
            PreviewTextBlock.Background = myBrush;
        }

    }

    public class ColorSelectedEventArgs : EventArgs
    {
        public Color SelectedColor { get; set; }

        public ColorSelectedEventArgs(Color selectedColor)
        {
            SelectedColor = selectedColor;
        }
    }
}
