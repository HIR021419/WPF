using CommunityToolkit.Mvvm.Input;
using ImageManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageManager.ViewModels
{
    public class ImageViewModel
    {
        public ObservableCollection<Image> Photos { get; } = new();
        public ICommand LoadCommand { get; }
        public ImageViewModel()
        {
            LoadCommand = new RelayCommand<string>(dir => {
                // TODO
            });
        }
    }
}
