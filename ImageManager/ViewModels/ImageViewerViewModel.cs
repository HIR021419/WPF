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
        private DispatcherTimer? _slideshowTimer;
        private bool _isPlaying = false;
        private string _slideshowButtonText = "▶️ Play";
        #endregion

        #region commands
        public ICommand _exitCommand = null!;
        public ICommand ToggleSlideshowCommand { get; set; } = null!;
        #endregion

        #region getter / setter
        public string SlideshowButtonText
        {
            get => _slideshowButtonText;
            set
            {
                _slideshowButtonText = value;
                OnPropertyChanged(nameof(SlideshowButtonText));
            }
        }

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
            ToggleSlideshowCommand = new RelayCommand(_ => ToggleSlideshow());
        }

        private void ToggleSlideshow()
        {
            if (_isPlaying)
                StopSlideshow();
            else
                StartSlideshow();
        }

        private void Close(object sender)
        {
            StopSlideshow();
            CloseSignal = true;
        }

        private void StartSlideshow()
        {
            _isPlaying = true;
            SlideshowButtonText = "⏸️ Pause";

            _slideshowTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _slideshowTimer.Tick += (s, e) => NextImage();
            _slideshowTimer.Start();

            NextImage();
        }

        private void StopSlideshow()
        {
            _isPlaying = false;
            SlideshowButtonText = "▶️ Play";

            _slideshowTimer?.Stop();
            _slideshowTimer = null;
        }

        private void NextImage()
        {
            if (_images == null || !_images.Any()) return;

            _currentIndex = (_currentIndex + 1) % _images.Count;
            OnPropertyChanged(nameof(CurrentImage));

            // Animation fade-in
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = Application.Current.Windows.OfType<Window>()
                    .FirstOrDefault(w => w is Views.ImageViewer) as Views.ImageViewer;
                if (window == null) return;

                var sb = (Storyboard)window.Resources["FadeTransitionStoryboard"];
                Storyboard.SetTarget(sb, window.FullImage);
                sb.Begin();
            });
        }
    }
}
