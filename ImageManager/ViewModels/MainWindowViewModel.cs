using ImageManager.Data;
using ImageManager.Services;
using ImageManager.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageManager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ImageService _svc;
        private readonly ImageDbContext _db;

        private Models.Image? _selectedImage;
        public Models.Image? SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }
        public ObservableCollection<Models.Image> Items { get; set; } = null!;

        public ImageViewer imageViewer;
        public ICommand RotateCommand { get; }

        public MainWindowViewModel() : base()
        {
            base.DisplayName = "ImageManager";
            _db = new ImageDbContext();
            _db.Database.EnsureCreated();
            _svc = new ImageService(_db);
            Items = new(_svc.LoadFromDatabase());

            imageViewer = new Views.ImageViewer();
            ImageViewerViewModel imageViewerViemModel = new();
            imageViewer.DataContext = imageViewerViemModel;
            RotateCommand = new RelayCommand(OnRotateExecute, CanRotateExecute);
        }

        private void OnImportClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFolderDialog();
            if (dlg.ShowDialog() != true) return;
            foreach (var img in _svc.LoadFromDirectory(dlg.FolderName)) Items.Add(img);
        }

        private void OnRotateClick(object sender, RoutedEventArgs e)
        {
            if (_selectedImage == null) return;

            _svc.RotateImage(_selectedImage);
        }

        private void OnThumbnailClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Button btn || btn.DataContext is not Models.Image img) return;
            _selectedImage = img;
            // TODO
        }

        private void OnThumbnailDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Button btn || btn.DataContext is not Models.Image img) return;
            _selectedImage = img;
            //new ImageViewer(Items.ToList(), img).ShowDialog();
        }

        private void onSortClicked(object sender, RoutedEventArgs e)
        {
            // TODO : tri
        }

        private void onFilterChanged(object sender, RoutedEventArgs e)
        {
            Items.Clear();
            // TODO : filtre
        }

        private void onTagManagementClicked(object sender, RoutedEventArgs e)
        {
            // TODO : gestion des tags
        }

        private void onTagFilterChanged(object sender, RoutedEventArgs e)
        {
            Items.Clear();
            // TODO : filtre par tag
        }

        private void OnRotateExecute(object? parameter)
        {
            if (SelectedImage == null) return;
            _svc.RotateImage(SelectedImage);
        }

        private bool CanRotateExecute(object? parameter)
        {
            return SelectedImage != null;
        }
    }
}
