using ImageManager.Data;
using ImageManager.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageManager.Views
{
    public partial class MainWindow : Window
    {
        private readonly ImageService _svc;
        private readonly ImageDbContext _db;
        private Models.Image? _selectedImage = null;
        public ObservableCollection<Models.Image> Items { get; set; } = null!;

        public MainWindow() : base()
        {
            this.InitializeComponent();
            _db = new ImageDbContext();
            _svc = new ImageService(_db);
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _db.Database.EnsureCreated();
            Items = new(_svc.LoadFromDatabase());
            ThumbnailsControl.ItemsSource = Items;
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
            new ImageViewer(Items.ToList(), img).ShowDialog();
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
    }
}
