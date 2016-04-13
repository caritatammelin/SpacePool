using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpacePool
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScorePage : Page
    {
        private MediaElement mediaElement;
        private MediaElement clickElement;

        private ObservableCollection<PlayerScore> scores = new ObservableCollection<PlayerScore>();

 
    
        public ScorePage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchWindowingMode
               = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);

            LoadAudio();

           // list of default scores
            scores.Add(new PlayerScore { Name = "Ajax", Score = 10000 });
            scores.Add(new PlayerScore { Name = "Angel Dust", Score = 8000 });
            scores.Add(new PlayerScore { Name = "Colossus", Score = 9500 });
            scores.Add(new PlayerScore { Name = "Negasonic Teenage Warhead", Score = 8500 });

            

     


           // ScoresListView.ItemsSource = Scores;
        }

        public async void LoadAudio()
        {
            mediaElement = new MediaElement();
            mediaElement.AutoPlay = true;
            mediaElement.IsLooping = true;
            StorageFolder folder1 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file1 = await folder1.GetFileAsync("spaceambience.wav");
            var stream1 = await file1.OpenAsync(FileAccessMode.Read);
            mediaElement.SetSource(stream1, file1.ContentType);
            clickElement = new MediaElement();
            clickElement.AutoPlay = false;
            StorageFolder folder2 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file2 = await folder2.GetFileAsync("click.wav");
            var stream2 = await file2.OpenAsync(FileAccessMode.Read);
            clickElement.SetSource(stream2, file2.ContentType);
        }


        private void againButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage));
            clickElement.Play();
        }

        private void scoreBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
