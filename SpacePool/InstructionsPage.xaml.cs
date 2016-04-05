using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class InstructionsPage : Page
    {
        private MediaElement clickElement;

        public async void LoadAudio()
        {
            clickElement = new MediaElement();
            clickElement.AutoPlay = false;
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync("click.wav");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            clickElement.SetSource(stream, file.ContentType);
        }

        public InstructionsPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchWindowingMode
               = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);

            LoadAudio();

        }

        // back to the main page
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
            clickElement.Play();
        }

        private void gameButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage));
            clickElement.Play();
        }
    }
}
