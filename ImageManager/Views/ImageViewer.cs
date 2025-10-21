using ImageManager.Models;
using System.Windows;

namespace ImageManager.Views
{
    public partial class ImageViewer : Window
    {
        public ImageViewer(Image image)
        {
            InitializeComponent();
            DataContext = image;
        }
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape) Close();
        }
    }
}
