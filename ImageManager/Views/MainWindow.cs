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
        public ObservableCollection<dynamic> Items { get; } = new();

        public MainWindow() : base()
        {
            InitializeComponent();
            _db = new ImageDbContext();
            _svc = new ImageService(_db);
            ThumbnailsControl.ItemsSource = Items;
        }

        /*
        private void OnImportClick(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var photos = _svc.LoadFromDirectory(dlg.SelectedPath).ToList();
                _svc.SaveImages(photos);
                Items.Clear();
                foreach (var p in photos)
                {
                    Items.Add(new
                    {
                        Model = p,
                        Thumbnail = ImageHelper.LoadThumbnail(p.Path, 300)
                    });
                }
            }
        }

        private void OnThumbnailDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && ((FrameworkElement)e.OriginalSource).DataContext is dynamic ctx)
            {
                var photo = (Image) ctx.Model;
                var win = new ImageDetail(photo.Path);
                win.ShowDialog();
            }
        }

        // TODO : tri, filtre et diaporama
        */
    }
}
