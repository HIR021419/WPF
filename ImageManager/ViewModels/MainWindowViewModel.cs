using ImageManager.Data;
using ImageManager.Services;
using ImageManager.Views;
using Microsoft.Win32;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private string _selectedSortOption= "Name";
        ListSortDirection sortDirection = ListSortDirection.Ascending;
        private string _sortDirectionText = "Asc";
        #endregion

        #region commands
        private ICommand _importCommand = null!;
        private ICommand _rotateCommand = null!;
        private ICommand _viewImageCommand = null!;
        private ICommand _sortCommand = null!;
        private ICommand _filterCommand = null!;
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
            get { return _searchPattern; }
            set
            {
                if (_searchPattern != value)
                {
                    _searchPattern = value;
                    System.ComponentModel.ICollectionView myView = CollectionViewSource.GetDefaultView(Images);
                    myView.Filter = (item) =>
                    {
                        if (item as Models.Image == null) return false;

                        Models.Image ImageView = (Models.Image)item;
                        if (ImageView.Title?.Contains(value) == true ||
                            ImageView.FileName.Contains(value))
                            return true;

                        return false;
                    };
                    OnPropertyChanged(nameof(SearchPattern));
                }
            }
        }

        public ObservableCollection<Models.Image> Images
        {
            get { return _images; }
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
            get { return _importCommand; }
            set { _importCommand = value; }
        }

        public ICommand RotateCommand
        {
            get { return _rotateCommand; }
            set { _rotateCommand = value; }
        }

        public ICommand ViewImageCommand
        {
            get { return _viewImageCommand; }
            set { _viewImageCommand = value; }
        }

        public ICommand SortCommand
        {
            get { return _sortCommand; }
            set { _sortCommand = value; }
        }

        public ICommand FilterCommand
        {
            get { return _filterCommand; }
            set { _filterCommand = value; }
        }

        public ICommand DeleteImageCommand
        {
            get { return _deleteImageCommand; }
            set { _deleteImageCommand = value; }
        }

        public ICommand AddTagCommand
        {
            get { return _addTagCommand; }
            set { _addTagCommand = value; }
        }

        public ICommand RemoveTagCommand
        {
            get { return _removeTagCommand; }
            set { _removeTagCommand = value; }
        }

        public ICommand SaveChangesCommand
        {
            get { return _saveChangesCommand; }
            set { _saveChangesCommand = value; }
        }
        #endregion

        public ICollectionView ImagesView { get; }

        public MainWindowViewModel() : base()
        {
            base.DisplayName = "ImageManager";
            _db = new ImageDbContext();
            _db.Database.EnsureCreated();
            _svc = new ImageService(_db);
            Images = new(_svc.LoadFromDatabase());
            Tags = new(_db.Tags);

            ImportCommand = new RelayCommand(OnImportExecute);
            RotateCommand = new RelayCommand(OnRotateExecute, CanExecuteOnSelectedImage);
            ViewImageCommand = new RelayCommand(OnViewImageExecute, CanExecuteOnSelectedImage);
            SortCommand = new RelayCommand(onSortExecute);
            FilterCommand = new RelayCommand(onFilterExecute);
            DeleteImageCommand = new RelayCommand(onDeleteImageExecute, CanExecuteOnSelectedImage);
            AddTagCommand = new RelayCommand(onAddTagExecute, CanExecuteOnSelectedImage);
            RemoveTagCommand = new RelayCommand(onRemoveTagExecute, CanExecuteOnSelectedImage);
            SaveChangesCommand = new RelayCommand(onSaveChangesExecute);

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
                case "Taille":
                    ImagesView.SortDescriptions.Add(new SortDescription(nameof(Models.Image.Date), sortDirection));
                    break;
            }

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
            ImageViewer imageViewer = new Views.ImageViewer();
            ImageViewerViewModel imageViewerViemModel = new(SelectedImage, Images.ToList());
            imageViewer.DataContext = imageViewerViemModel;
            imageViewer.InitializeComponent();
            imageViewer.Show();
        }

        private void onSortExecute(object? parameter)
        {
            sortDirection = (ListSortDirection)((int)(sortDirection + 1) % 2);
            SortDirectionText = (sortDirection == ListSortDirection.Ascending) ? "Asc" : "Desc";
            ApplySort();
        }

        private void onFilterExecute(object? parameter)
        {
            // TODO : filtre
        }

        private void onTagManagementExecute(object? parameter)
        {
            // TODO : gestion des tags
        }

        private void onTagFilterChanged(object? parameter)
        {
            // TODO : filtre par tag
        }

        private void OnRotateExecute(object? parameter)
        {
            if (SelectedImage == null) return;
            _svc.RotateImage(SelectedImage);
            OnPropertyChanged(nameof(SelectedImage));
            OnPropertyChanged(nameof(Images));
        }

        private void onDeleteImageExecute(object obj)
        {
            if (SelectedImage == null) return;
            _svc.DeleteImage(SelectedImage);
            Images.Remove(SelectedImage);
            SelectedImage = null;
            OnPropertyChanged(nameof(Images));
        }

        private void onAddTagExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private void onRemoveTagExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private void onSaveChangesExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanExecuteOnSelectedImage(object? parameter)
        {
            return SelectedImage != null;
        }
    }
}
