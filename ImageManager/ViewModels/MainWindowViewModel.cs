using ImageManager.Data;
using ImageManager.Services;
using ImageManager.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace ImageManager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region variables
        private readonly ImageService _svc = null!;
        private readonly ImageDbContext _db = null!;
        private ObservableCollection<Models.Image> _images = null!;
        private Models.Image? _selectedImage = null;
        private string _searchPattern = "";

        private ObservableCollection<Models.Tag> _tages = null!;
        private string _selectedSortOption = "Name";
        private string _selectedFilterOption = "All";

        ListSortDirection sortDirection = ListSortDirection.Ascending;
        private string _sortDirectionText = "Asc";
        private string _newTagText = "";
        #endregion

        #region commands
        private ICommand _importCommand = null!;
        private ICommand _rotateCommand = null!;
        private ICommand _viewImageCommand = null!;
        private ICommand _sortCommand = null!;
        private ICommand _deleteImageCommand = null!;
        private ICommand _addTagCommand = null!;
        private ICommand _removeTagCommand = null!;
        private ICommand _saveChangesCommand = null!;
        #endregion

        #region getter / setter
        public string SortDirectionText
        {
            get => _sortDirectionText;
            set
            {
                if (_sortDirectionText != value)
                {
                    _sortDirectionText = value;
                    OnPropertyChanged(nameof(SortDirectionText));
                }
            }
        }

        public Models.Image? SelectedImage
        {
            get => _selectedImage;
            set
            {
                if (_selectedImage != value)
                {
                    _selectedImage = value;
                    OnPropertyChanged(nameof(SelectedImage));
                }
            }
        }

        public string SearchPattern
        {
            get => _searchPattern;
            set
            {
                if (_searchPattern != value)
                {
                    _searchPattern = value;

                    ICollectionView myView = CollectionViewSource.GetDefaultView(Images);
                    myView.Filter = (item) =>
                    {
                        if (item is not Models.Image img) return false;

                        return (img.Title?.Contains(value, StringComparison.OrdinalIgnoreCase) == true)
                               || img.FileName.Contains(value, StringComparison.OrdinalIgnoreCase);
                    };

                    OnPropertyChanged(nameof(SearchPattern));
                }
            }
        }

        public ObservableCollection<Models.Image> Images
        {
            get => _images;
            set
            {
                if (_images != value)
                {
                    _images = value;
                    OnPropertyChanged(nameof(Images));
                }
            }
        }

        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (_selectedSortOption != value)
                {
                    _selectedSortOption = value;
                    OnPropertyChanged(nameof(SelectedSortOption));
                    ApplySort();
                }
            }
        }
        public string SelectedFilterOption
        {
            get => _selectedFilterOption;
            set
            {
                if (_selectedFilterOption != value)
                {
                    _selectedFilterOption = value;
                    OnPropertyChanged(nameof(SelectedFilterOption));
                    ApplyFilter();
                }
            }
        }

        public ObservableCollection<Models.Tag> Tags
        {
            get { return _tages; }
            set
            {
                if (_tages != value)
                {
                    _tages = value;
                    OnPropertyChanged(nameof(Tags));
                }
            }
        }

        public ICommand ImportCommand
        {
            get => _importCommand;
            set => _importCommand = value;
        }

        public ICommand RotateCommand
        {
            get => _rotateCommand;
            set => _rotateCommand = value;
        }

        public ICommand ViewImageCommand
        {
            get => _viewImageCommand;
            set => _viewImageCommand = value;
        }

        public ICommand SortCommand
        {
            get => _sortCommand;
            set => _sortCommand = value;
        }

        public ICommand DeleteImageCommand
        {
            get => _deleteImageCommand;
            set => _deleteImageCommand = value;
        }

        public ICommand AddTagCommand
        {
            get => _addTagCommand;
            set => _addTagCommand = value;
        }

        public ICommand RemoveTagCommand
        {
            get => _removeTagCommand;
            set => _removeTagCommand = value;
        }

        public ICommand SaveChangesCommand
        {
            get => _saveChangesCommand;
            set => _saveChangesCommand = value;
        }
        #endregion

        public ICollectionView ImagesView { get; }

        public string NewTagText
        {
            get => _newTagText;
            set
            {
                if (_newTagText != value)
                {
                    _newTagText = value;
                    OnPropertyChanged(nameof(NewTagText));
                }
            }
        }

        public MainWindowViewModel() : base()
        {
            base.DisplayName = "ImageManager";
            _db = new ImageDbContext();
            _db.Database.Migrate();
            _svc = new ImageService(_db);

            Images = new(_svc.LoadFromDatabase());

            ImportCommand = new RelayCommand(OnImportExecute);
            RotateCommand = new RelayCommand(OnRotateExecute, CanExecuteOnSelectedImage);
            ViewImageCommand = new RelayCommand(OnViewImageExecute, CanExecuteOnSelectedImage);
            SortCommand = new RelayCommand(OnSortExecute);
            DeleteImageCommand = new RelayCommand(OnDeleteImageExecute, CanExecuteOnSelectedImage);
            AddTagCommand = new RelayCommand(OnAddTagExecute, CanExecuteOnSelectedImage);
            RemoveTagCommand = new RelayCommand(OnRemoveTagExecute, CanExecuteOnSelectedImage);
            SaveChangesCommand = new RelayCommand(OnSaveChangesExecute);

            ImagesView = CollectionViewSource.GetDefaultView(Images);
            ApplySort();
        }

        private void ApplySort()
        {
            ImagesView.SortDescriptions.Clear();

            switch (SelectedSortOption)
            {
                case "Name":
                    ImagesView.SortDescriptions.Add(new SortDescription(nameof(Models.Image.FileName), sortDirection));
                    break;
                case "Date":
                    ImagesView.SortDescriptions.Add(new SortDescription(nameof(Models.Image.Date), sortDirection));
                    break;
                case "Size":
                    ImagesView.SortDescriptions.Add(new SortDescription(nameof(Models.Image.Size), sortDirection));
                    break;
            }
        }

        private void ApplyFilter()
        {
            ImagesView.Filter = null;
            ImagesView.Filter = (item) =>
            {
                if (item is not Models.Image image)
                    return false;

                string extension = System.IO.Path.GetExtension(image.FileName)?.ToLower();
                return SelectedFilterOption switch
                {
                    "All" => true,
                    "Jpg" => extension == ".jpg" || extension == ".jpeg",
                    "Png" => extension == ".png",
                    "Bmp" => extension == ".bmp",
                    _ => true
                };
            };

            ImagesView.Refresh();
        }


        private void OnImportExecute(object? parameter)
        {
            var dlg = new OpenFolderDialog();
            if (dlg.ShowDialog() != true) return;
            foreach (var img in _svc.LoadFromDirectory(dlg.FolderName)) Images.Add(img);
        }

        private void OnViewImageExecute(object? parameter)
        {
            if (SelectedImage == null) return;
            var imageViewer = new Views.ImageViewer();
            var imageViewerViewModel = new ImageViewerViewModel(SelectedImage, Images.ToList());
            imageViewer.DataContext = imageViewerViewModel;
            imageViewer.InitializeComponent();
            imageViewer.Show();
        }

        private void OnSortExecute(object? parameter)
        {
            sortDirection = (ListSortDirection)((int)(sortDirection + 1) % 2);
            SortDirectionText = (sortDirection == ListSortDirection.Ascending) ? "Asc" : "Desc";
            ApplySort();
        }

        private void OnRotateExecute(object? parameter)
        {
            if (SelectedImage == null) return;
            _svc.RotateImage(SelectedImage);
            OnPropertyChanged(nameof(SelectedImage));
            OnPropertyChanged(nameof(Images));
        }

        private void OnDeleteImageExecute(object? obj)
        {
            if (SelectedImage == null) return;
            _svc.DeleteImage(SelectedImage);
            Images.Remove(SelectedImage);
            SelectedImage = null;
            OnPropertyChanged(nameof(Images));
        }

        private void OnAddTagExecute(object? obj)
        {
            if (SelectedImage == null) return;
            var text = (NewTagText ?? "").Trim();
            if (string.IsNullOrWhiteSpace(text)) return;

            if (SelectedImage.Tags.Any(t => t.Name.Equals(text, StringComparison.OrdinalIgnoreCase)))
                return;

            var tag = new Models.Tag { Name = text, ImageId = SelectedImage.Id, Image = SelectedImage };

            _db.Tags.Add(tag);
            _db.SaveChanges();

            NewTagText = "";
        }

        private void OnRemoveTagExecute(object? obj)
        {
            if (SelectedImage == null || obj is not Models.Tag tag) return;

            _db.Tags.Remove(tag);
            _db.SaveChanges();

            SelectedImage.Tags.Remove(tag);
        }

        private void OnSaveChangesExecute(object? obj)
        {
            _db.SaveChanges();
        }

        private bool CanExecuteOnSelectedImage(object? parameter) => SelectedImage != null;
    }
}
