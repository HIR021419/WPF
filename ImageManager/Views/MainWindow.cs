using ImageManager.Data;
using ImageManager.Helpers;
using ImageManager.Models;
using ImageManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImageManager.Views
{
    partial class MainWindow : Window
    {
        private readonly ImageService _svc;
        private readonly ImageDbContext _db;
        // public ObservableCollection<Image> Items { get; } = new();

        public MainWindow() : base()
        {
            // this.InitializeComponent();
            _db = new ImageDbContext();
            _svc = new ImageService(_db);
            // ThumbnailsControl.ItemsSource = Items;
        }
        
        private void OnImportClick(object sender, RoutedEventArgs e)
        {
            // TODO
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
