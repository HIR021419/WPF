using ImageManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ImageManager.ViewModels
{
    internal class ImageViewerViewModel : BaseViewModel
    {
        #region variables
        private readonly List<Models.Image> _images;
        private bool _closeSignal = false;
        private int _currentIndex;
        private Timer? _slideshowTimer;
        private bool _isPlaying = false;
        #endregion

        #region commands
        public ICommand _exitCommand = null!;
        #endregion

        #region getter / setter
        public bool CloseSignal
        {
            get { return _closeSignal; }
            set
            {
                if (_closeSignal != value)
                {
                    _closeSignal = value;
                    OnPropertyChanged(nameof(CloseSignal));
                }
            }
        }

        public Models.Image CurrentImage
        {
            get
            {
                if (_images != null && _images.Any() && _currentIndex >= 0 && _currentIndex < _images.Count)
                    return _images[_currentIndex];
                return null;
            }
        }

        public ICommand ExitCommand
        {
            get { return _exitCommand; }
            set { _exitCommand = value; }
        }
        #endregion

        public ImageViewerViewModel(Image current, List<Image> images) : base()
        {
            base.DisplayName = "Image Viewer";
            _images = images;
            _currentIndex = _images.FindIndex(i => i.Path == current.Path);

            ExitCommand = new RelayCommand(Close);
        }

        private void Close(object sender)
        {
            StopSlideshow();
            CloseSignal = true;
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
            //SlideshowButton.Content = "⏸️";
            //_slideshowTimer = new Timer((s) => Dispatcher.Invoke(NextImage), null, 0, 3000);
        }

        private void StopSlideshow()
        {
            _isPlaying = false;
            //SlideshowButton.Content = "▶️";
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
                /*
                Dispatcher.Invoke(() =>
                {
                    //DataContext = next;
                    //FullImage.Opacity = 0;
                    var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(350));
                    fadeIn.FillBehavior = FillBehavior.Stop;
                    fadeIn.Completed += (s2, e2) =>
                    {
                        //FullImage.Opacity = 1;
                    };
                    //FullImage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                });*/
            };

            //FullImage.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }
    }
}
