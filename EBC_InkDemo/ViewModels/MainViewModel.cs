using AMP.ViewModels;
using GalaSoft.MvvmLight;
using Micosoft.MTC.SmartInk.Package;
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
        private PackageManager _packageManager = new PackageManager();

        private ObservableCollection<string> _samples = new ObservableCollection<string>();
        public ReadOnlyObservableCollection<string> Samples { get; private set; }

        private ISmartInkPackage _currentPackage;
        private IList<ISmartInkPackage> _packages;

        public ISmartInkPackage CurrentPackage
        {
            get { return _currentPackage; }
            set
            {
                if (_currentPackage == value)
                    return;
                _currentPackage = value;
                RaisePropertyChanged(nameof(CurrentPackage));
            }
        }


        public MainViewModel()
        {
            Samples = new ReadOnlyObservableCollection<string>(_samples);
        }

        protected override async Task InitializeAsync()
        {
            await LoadInstalledSmartInkPackagesAsync();
        }

        private async Task LoadInstalledSmartInkPackagesAsync()
        {
            _packages = await _packageManager.GetInstalledPackagesAsync();
            if (_packages.Count > 0)
            {
                CurrentPackage = _packages[0];
                await LoadSampleGalleryAsync(CurrentPackage.Name);
            }
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
