using AMP.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        private IList<ISmartInkPackage> _packages;
        private ISmartInkPackage _currentPackage;
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

        public ObservableCollection<ISmartInkPackage> InstalledPackages { get; private set; } = new ObservableCollection<ISmartInkPackage>();


        public RelayCommand<ISmartInkPackage> PackageSelected { get; set; }

        public MainViewModel()
        {
            Samples = new ReadOnlyObservableCollection<string>(_samples);

            this.PackageSelected = new RelayCommand<ISmartInkPackage>(async(package) => {
                CurrentPackage = package;
                await LoadSampleGalleryAsync(CurrentPackage.Name);
            });
        }

        protected override async Task InitializeAsync()
        {
            await LoadInstalledSmartInkPackagesAsync();
        }

        private async Task LoadInstalledSmartInkPackagesAsync()
        {
            InstalledPackages.Clear();
            _packages = await _packageManager.GetInstalledPackagesAsync();
            foreach (var package in _packages)
                InstalledPackages.Add(package);
      
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
