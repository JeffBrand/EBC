using AMP.Views;
using EBC_InkDemo.Extensions;
using EBC_InkDemo.ViewModels;
using Micosoft.MTC.SmartInk.Package;
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EBC_InkDemo.Views
{

    internal struct Icon
    {
        public string Id { get; private set; }
        public string Path { get; private set; }
        public Rect BoundingBox { get; private set; }

        public Icon(string path, Rect bounding)
        {
            Id = Guid.NewGuid().ToString();
            Path = path;
            BoundingBox = bounding;
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : NavAwarePage
    {
        PackageManager _packageManager = new PackageManager();

        DispatcherTimer _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(750) };
        List<InkStroke> _sessionStrokes = new List<InkStroke>();
        List<InkStroke> _allStrokes = new List<InkStroke>();

        private InkSynchronizer _inkSync;
        IReadOnlyList<InkStroke> _pendingDry;

        Dictionary<string, Icon> _icons = new Dictionary<string, Icon>();
        public MainPage()
        {
            this.InitializeComponent();
            _timer.Tick += async (s, e) => {
                _timer.Stop();
                var boundingBox = GetBoundingBox(_sessionStrokes);
                try
                {

                    

                }
                catch (Exception)
                {


                }
                finally
                {
                    _sessionStrokes.Clear();
                    win2dCanvas.Invalidate();
                }
            };

            inkCanvas.InkPresenter.StrokeInput.StrokeStarted += (s, e) =>
            {
                _timer.Stop();
            };

            inkCanvas.InkPresenter.StrokesCollected += (s, e) => {
                _timer.Stop();

                _pendingDry = _inkSync.BeginDry();
                foreach (var stroke in _pendingDry)
                    _sessionStrokes.Add(stroke);

                win2dCanvas.Invalidate();

                _timer.Start();
            };

            _inkSync = inkCanvas.InkPresenter.ActivateCustomDrying();

        }

        private Rect GetBoundingBox(IList<InkStroke> strokes)
        {
            Rect _boundingBox = Rect.Empty;
            foreach (var stroke in strokes)
            {
                if (_boundingBox == Rect.Empty)
                    _boundingBox = new Rect(stroke.BoundingRect.X, stroke.BoundingRect.Y, stroke.BoundingRect.Width, stroke.BoundingRect.Height);
                else
                    _boundingBox = _boundingBox.CombineWith(stroke.BoundingRect);
            }
            return _boundingBox;
        }

        private void listviewInstalledPackages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
                vm.PackageSelected.Execute(listviewInstalledPackages.SelectedItem);
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WelcomeGrid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Active", true);
        }
    }
}
