using ImageManager.Data;
using ImageManager.Models;
using ImageManager.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace ImageManager.Views
{
    public partial class MainWindow : Window
    {
        private readonly ImageService _svc;
        private readonly ImageDbContext _db;
        public ObservableCollection<Image> Items { get; set; } = null!;

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

        private void OnThumbnailDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // TODO
        }

        private void onSortClicked(object sender, RoutedEventArgs e)
        {
            // TODO : tri
        }

        private void onFilterSelected(object sender, RoutedEventArgs e)
        {
            // TODO : filtre
        }

        private void onTagManagementClicked(object sender, RoutedEventArgs e)
        {
            // TODO : gestion des tags
        }

        private void onDiaporamaClicked(object sender, RoutedEventArgs e)
        {
            // TODO : diaporama
        }
    }
}
