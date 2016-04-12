using System;
using System.Collections.Generic;
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
    
        public ScorePage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchWindowingMode
               = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);

            XmlDocument xdoc = new XmlDocument();
            //xdoc.LoadXml();
            //xdoc.Save("scores.xml");

        }

        private void againButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage));
        }

        private void scoreBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }

        /*public void Main()
        {
            var scores = ReadScoresFromFile("Highscores.txt");

            scores.ForEach(s => Console.WriteLine(s));

            Console.ReadKey();
        }*/
    }
}
