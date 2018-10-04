using AMP.ViewModels;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBC_InkDemo.ViewModels
{
    public class MainViewModel : AsyncViewModel
    {

        private ObservableCollection<string> _samples = new ObservableCollection<string>();
        public ReadOnlyObservableCollection<string> Samples { get; private set; }

        public MainViewModel()
        {
            Samples = new ReadOnlyObservableCollection<string>(_samples);
        }

        protected override async Task InitializeAsync()
        {
            await LoadSampleGalleryAsync("AzureArchitect");
        }

        private async Task LoadSampleGalleryAsync(string galleryName)
        {
            _samples.Clear();
            try
            {
                var assets = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets"); ;
                var folder = await assets.GetFolderAsync(galleryName);
                var files = await folder.GetFilesAsync();
                foreach (var file in files)
                    _samples.Add(file.Path);
            }
            catch (Exception ex)
            {

                
            }
        }
    }
}
