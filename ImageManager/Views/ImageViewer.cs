using ImageManager.Models;
using System.Windows;
using System.Windows.Media.Animation;

namespace ImageManager.Views
{
    public partial class ImageViewer : Window
    {
        private readonly List<Image> _images;
        private int _currentIndex;
        private Timer? _slideshowTimer;
        private bool _isPlaying = false;

        public ImageViewer(List<Image> images, Image current)
        {
            InitializeComponent();

            _images = images;
            _currentIndex = _images.FindIndex(i => i.Path == current.Path);
            DataContext = _images[_currentIndex];
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StopSlideshow();
            Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                StopSlideshow();
                Close();
            }
        }

        private void SlideshowButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
                StopSlideshow();
            else
                StartSlideshow();
        }

        private void StartSlideshow()
        {
            _isPlaying = true;
            SlideshowButton.Content = "⏸️";
            _slideshowTimer = new Timer((s) => Dispatcher.Invoke(NextImage), null, 0, 3000);
        }

        private void StopSlideshow()
        {
            _isPlaying = false;
            SlideshowButton.Content = "▶️";
            _slideshowTimer?.Dispose();
            _slideshowTimer = null;
        }

        private void NextImage()
        {
            if (_images == null || !_images.Any()) return;

            _currentIndex = (_currentIndex + 1) % _images.Count;
            var next = _images[_currentIndex];

            FadeTransition(next);
        }

        private void FadeTransition(Models.Image next)
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(350));
            fadeOut.FillBehavior = FillBehavior.Stop;
            fadeOut.Completed += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    DataContext = next;
                    FullImage.Opacity = 0;
                    var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(350));
                    fadeIn.FillBehavior = FillBehavior.Stop;
                    fadeIn.Completed += (s2, e2) =>
                    {
                        FullImage.Opacity = 1;
                    };
                    FullImage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                });
            };

            FullImage.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }
    }
}
